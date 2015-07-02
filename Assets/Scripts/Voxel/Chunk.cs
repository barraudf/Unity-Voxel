using UnityEngine;
using System.Collections.Concurrent;
using System.Threading;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    #region Fields
    public World World;
    public Vector3i Position;
    public Block[,,] Blocks;
    public Vector3i StartRenderingAt;

    public bool IsBusy = false;
    public bool MeshBuild = false;
    public bool MarkForDeletion = false;
    public bool DataLoaded = false;
    public bool Rendered = false;


    protected MeshFilter Filter;
    protected MeshCollider Collider;
    protected MeshData MeshData;
    protected Chunk MeshChild;
    protected Chunk DataParent;
    protected ConcurrentQueue<Vector3i> ChildCreationQueue;
    protected bool UpdateNeeded = false;
    #endregion Fields

    private void Awake()
    {
        Filter = gameObject.GetComponent<MeshFilter>();
        Collider = gameObject.GetComponent<MeshCollider>();
        MeshData = new MeshData();
        ChildCreationQueue = new ConcurrentQueue<Vector3i>();
        Clear();
    }

    private void Update()
    {
        if (MarkForDeletion)
        {
            Deactivate();
            return;
        }

        if (MeshBuild)
        {
            MeshBuild = false;
            RenderMesh();
            IsBusy = false;
        }
        else if(UpdateNeeded)
        {
            UpdateNeeded = false;
            StartMeshBuilding();
        }

        Vector3i blockPosition;
        while (ChildCreationQueue.TryDequeue(out blockPosition))
        {
            MeshChild = CreateMeshChild(blockPosition);
            if (MeshChild != null)
                MeshChild.StartMeshBuilding();
        }
    }

    public void CreateSampleChunk()
    {
        for (int x = 0; x < World.CHUNK_SIZE_H; x++)
            for (int y = 0; y < World.CHUNK_SIZE_V; y++)
                for (int z = 0; z < World.CHUNK_SIZE_H; z++)
                {
                    if(y > 32)
                    //if( (x+y+z)%2 == 1)
                        Blocks[x, y, z] = new BlockAir();
                    else
                        Blocks[x, y, z] = new BlockGrass();
                }

    }

    public void StartMeshBuilding()
    {
        if (!IsBusy)
        {
            IsBusy = true;

            if(World.MultithreadProcessing)
                ThreadPool.QueueUserWorkItem(nothing => { BuildMesh(); });
            else
                BuildMesh();
        }
    }

    public void BuildMesh()
    {
        MeshData.Clear();
        if(MeshChild != null)
        {
            //World.DestroyChunk(MeshChild);
            MeshChild.Deactivate();
            MeshChild = null;
        }
        Rendered = false;

        bool breakX = false;
        bool breakY = false;

        for (int x = StartRenderingAt.x; x < World.CHUNK_SIZE_H; x++)
        {
            for (int y = StartRenderingAt.y; y < World.CHUNK_SIZE_V; y++)
            {
                for (int z = StartRenderingAt.z; z < World.CHUNK_SIZE_H; z++)
                {
                    if (MarkForDeletion)
                        return;

                    Vector3i blockPosition = new Vector3i(x, y, z);

                    if (MeshData.Vertices.Count + 24 <= System.UInt16.MaxValue)
                    {
                        Block[,,] blocks = GetBlocks();
                        MeshData = blocks[x, y, z].BuildMesh(this, blockPosition, MeshData);
                    }
                    else
                    {
                        ChildCreationQueue.Enqueue(blockPosition);

                        breakX = breakY = true;
                        break;
                    }
                    StartRenderingAt.z = 0;
                }
                if (breakY)
                    break;
                StartRenderingAt.y = 0;
            }
            if (breakX)
                break;
            StartRenderingAt.x = 0;
        }
        MeshBuild = true;
    }

    private Block[,,] GetBlocks()
    {
        if (DataParent != null)
            return DataParent.GetBlocks();
        else
            return Blocks;
    }

    private void RenderMesh()
    {
        if (MeshChild != null)
            MeshChild.RenderMesh();

        if (Filter.mesh == null)
            Filter.mesh = new Mesh();

        MeshData.ToMeshFilter(Filter.mesh);

        Collider.sharedMesh = null;
        Collider.sharedMesh = MeshData.ToMeshCollider();

        Rendered = true;
    }

    public Block GetBlock(Vector3i blockPosition)
    {
        Block block;

        if (IsLocalPosition(blockPosition))
        {
            block = GetBlocks()[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            block = World.GetBlock(blockPosition + Position);
        }

        return block;
    }

    protected Chunk CreateMeshChild(Vector3i startingBlock)
    {
        Chunk child = World.ActivateChunk(Position);
        if (child != null)
        {
            MeshChild = child;
            child.DataParent = this;
            child.StartRenderingAt = startingBlock;
        }

        return child;
    }

    public void Clear()
    {
        Blocks = new Block[World.CHUNK_SIZE_H, World.CHUNK_SIZE_V, World.CHUNK_SIZE_H];
        IsBusy = false;
        MeshBuild = false;
        Rendered = false;
        DataLoaded = false;
        StartRenderingAt = Vector3i.Zero;
        DataParent = null;
        MeshData.Clear();
        if (Filter.mesh == null)
            Filter.mesh.Clear();
        Collider.sharedMesh = null;

        if (MeshChild != null)
        {
            //World.DestroyChunk(MeshChild);
            MeshChild.Clear();
            MeshChild.Deactivate();
            MeshChild = null;
        }
    }

    public Chunk GetTopParent()
    {
        Chunk parent = this;

        while (parent.DataParent != null)
            parent = parent.DataParent;

        return parent;
    }

    protected static bool InRangeH(int index)
    {
        if (index < 0 || index >= World.CHUNK_SIZE_H)
            return false;

        return true;
    }

    protected static bool InRangeV(int index)
    {
        if (index < 0 || index >= World.CHUNK_SIZE_V)
            return false;

        return true;
    }

    public static bool IsLocalPosition(Vector3i position)
    {
        return InRangeH(position.x) && InRangeV(position.y) && InRangeH(position.z);
    }

    private void Deactivate()
    {
        if (MeshChild != null)
            MeshChild.Deactivate();

        gameObject.SetActive(false);
    }

    public static Vector3i GetChunkCoordinates(Vector3i blockPosition)
    {
        int x = Mathf.FloorToInt(blockPosition.x / (float)World.CHUNK_SIZE_H) * World.CHUNK_SIZE_H;
        int y = Mathf.FloorToInt(blockPosition.y / (float)World.CHUNK_SIZE_V) * World.CHUNK_SIZE_V;
        int z = Mathf.FloorToInt(blockPosition.z / (float)World.CHUNK_SIZE_H) * World.CHUNK_SIZE_H;

        Vector3i retour = new Vector3i(x, y, z);

        return retour;
    }

    public void Invalidate()
    {
        UpdateNeeded = true;
    }

    public void SetBlock(Vector3i blockPosition, Block block)
    {
        if (IsLocalPosition(blockPosition))
        {
            GetBlocks()[blockPosition.x, blockPosition.y, blockPosition.z] = block;
            Invalidate();
        }
        else
            World.SetBlock(Position + blockPosition, block);
    }

    public override string ToString()
    {
        return string.Format("Chunk({0},{1},{2})", Position.x, Position.y, Position.z);
    }
}
