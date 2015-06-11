using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    #region Fields
    public GameObject ChunkPrefab;

    private Dictionary<Vector3i, Chunk> Chunks;
    #endregion Fields

    public World()
    {
        Chunks = new Dictionary<Vector3i, Chunk>();
    }

    #region MonoBehaviour
    void Start()
    {
        CreateSampleWorld();
    }
    #endregion MonoBehaviour

    #region Chunk management
    public Chunk CreateChunk(Vector3i position)
    {
        //TODO: object pooling!
        GameObject newChunkObject = Instantiate(ChunkPrefab, position.ToVector3(), Quaternion.Euler(Vector3.zero)) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.Position = position;
        newChunk.World = this;

        Chunks.Add(position, newChunk);

        return newChunk;
    }

    public Chunk GetChunk(Vector3i blockPosition)
    {
        float chunkSizeF = Chunk.CHUNK_SIZE;
        int x = Mathf.FloorToInt(blockPosition.x / chunkSizeF) * Chunk.CHUNK_SIZE;
        int y = Mathf.FloorToInt(blockPosition.y / chunkSizeF) * Chunk.CHUNK_SIZE;
        int z = Mathf.FloorToInt(blockPosition.z / chunkSizeF) * Chunk.CHUNK_SIZE;

        Vector3i chunkPosition = new Vector3i(x,y,z);
 
        Chunk containerChunk = null;

        Chunks.TryGetValue(chunkPosition, out containerChunk);
  
        return containerChunk;
    }

    public void DestroyChunk(Vector3i position)
    {
        Chunk chunk = null;
        if (Chunks.TryGetValue(position, out chunk))
        {
            //Object pooling
            Object.Destroy(chunk.gameObject);
            Chunks.Remove(position);
        }
    }
    #endregion Chunk management

    #region Block management
    public Block GetBlock(Vector3i blockPosition)
    {
        Chunk containerChunk = GetChunk(blockPosition);

        if (containerChunk != null)
        {
            Block chunkBlock = containerChunk.GetBlock(blockPosition - containerChunk.Position);

            if (chunkBlock != null)
            return chunkBlock;
        }

        return new BlockAir(null);
    }

    public void SetBlock(Vector3i blockPosition, Block block)
    {
        Chunk chunk = GetChunk(blockPosition);

        if (chunk != null)
        {
            chunk.SetBlock(blockPosition - chunk.Position, block);

            UpdateIfEqual(blockPosition.x - chunk.Position.x, 0, blockPosition + Vector3i.left);
            UpdateIfEqual(blockPosition.x - chunk.Position.x, Chunk.CHUNK_SIZE - 1, blockPosition + Vector3i.right);
            UpdateIfEqual(blockPosition.y - chunk.Position.y, 0, blockPosition + Vector3i.down);
            UpdateIfEqual(blockPosition.y - chunk.Position.y, Chunk.CHUNK_SIZE - 1, blockPosition + Vector3i.up);
            UpdateIfEqual(blockPosition.z - chunk.Position.z, 0, blockPosition + Vector3i.forward);
            UpdateIfEqual(blockPosition.z - chunk.Position.z, Chunk.CHUNK_SIZE - 1, blockPosition + Vector3i.backward);
        }
    }

    public static Vector3i GetBlockPosition(Vector3 pos)
    {
        Vector3i blockPos = new Vector3i(
        Mathf.RoundToInt(pos.x),
        Mathf.RoundToInt(pos.y),
        Mathf.RoundToInt(pos.z)
        );

        return blockPos;
    }

    public static Vector3i GetBlockPosition(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPosition(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
                pos += (norm / 2);
            else
                pos -= (norm / 2);
        }

        return (float)pos;
    }

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        Vector3i pos = GetBlockPosition(hit, adjacent);

        chunk.World.SetBlock(pos, block);

        return true;
    }

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        Vector3i pos = GetBlockPosition(hit, adjacent);

        Block block = chunk.World.GetBlock(pos);

        return block;
    }
    #endregion Block management

    private void CreateSampleWorld()
    {
        for (int x = -2; x < 2; x++)
            for (int y = -1; y < 1; y++)
                for (int z = -1; z < 1; z++)
                    CreateSampleChunk(new Vector3i(x * Chunk.CHUNK_SIZE, y * Chunk.CHUNK_SIZE, z * Chunk.CHUNK_SIZE));

        //CreateSampleChunk(new Vector3i(0,0,0));
    }

    private Chunk CreateSampleChunk(Vector3i chunkPosition)
    {
        Chunk chunk = CreateChunk(chunkPosition);

        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
        for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
        {
            Block block = y <= 7 ? (Block)new BlockGrass(chunk) : (Block)new BlockAir(chunk);
            Vector3i blockPosition = new Vector3i(x, y, z);

            SetBlock(chunkPosition + blockPosition, block);
        }

        return chunk;
    }

    void UpdateIfEqual(int value1, int value2, Vector3i pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos);
            if (chunk != null)
                chunk.Invalidate();
        }
    }
}
