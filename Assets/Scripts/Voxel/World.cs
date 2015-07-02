using UnityEngine;
using System.Collections.Generic;
using System.Threading;


public class World : MonoBehaviour
{
    #region Fields
    public const float BLOCK_SIZE = 1f;
    public const int CHUNK_SIZE_H = 16;
    public const int CHUNK_SIZE_V = 64;

    public ObjectPooling ChunkPool;
    public bool MultithreadProcessing;
    public Dictionary<Vector3i, Chunk> Chunks = new Dictionary<Vector3i, Chunk>();
    #endregion Fields

    private void Start()
    {
        if (ChunkPool == null)
            Debug.LogError("ChunkPool has not been set");
    }

    #region Chunk management
    public Chunk CreateChunk(Vector3i blockPosition, bool markAsLoaded)
    {
        if (ChunkPool == null)
        {
            return null;
        }

        Chunk chunk = ActivateChunk(blockPosition);

        chunk.DataLoaded = markAsLoaded;

        Chunks.Add(chunk.Position, chunk);

        if (MultithreadProcessing)
            ThreadPool.QueueUserWorkItem(c => InitializeChunk((Chunk)c), chunk);
        else
            InitializeChunk(chunk);

        return chunk;
    }

    public Chunk ActivateChunk(Vector3i blockPosition)
    {
        GameObject chunkGO = ChunkPool.NextObject();
        if (chunkGO == null)
        {
            return null;
        }

        chunkGO.transform.position = blockPosition;

        Chunk chunk = chunkGO.GetComponent<Chunk>();

        chunk.Position = blockPosition;
        chunk.World = this;
        chunkGO.name = chunk.ToString();

        return chunk;
    }

    public void InitializeChunk(Chunk chunk)
    {
        //charger les données
        chunk.CreateSampleChunk();
    }

    public Chunk GetChunk(Vector3i blockPosition)
    {
        Vector3i chunkPosition = Chunk.GetChunkCoordinates(blockPosition);

        Chunk chunk = null;
        Chunks.TryGetValue(chunkPosition, out chunk);

        return chunk;
    }

    public void DestroyChunk(Vector3i position)
    {
        Chunk chunk = null;
        if(Chunks.TryGetValue(position, out chunk))
        {
            if(MultithreadProcessing)
                ThreadPool.QueueUserWorkItem(c => DestroyChunkThread((Chunk)c), chunk);
            else
                DestroyChunkThread(chunk);

            Chunks.Remove(chunk.Position);

            chunk.Clear();
        }
    }

    private void DestroyChunkThread(Chunk chunk)
    {
        //Debug.LogWarning("Ajouter l'enregistrement dans DestoyChunkThread");
        chunk.MarkForDeletion = true;
    }
    #endregion Chunk management

    public static Vector3i GetBlockPosition(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return (Vector3i)pos;
    }

    protected static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == BLOCK_SIZE / 2 || pos - (int)pos == -BLOCK_SIZE / 2)
        {
            if (adjacent)
                pos += (norm / 2);
            else
                pos -= (norm / 2);
        }

        return (float)pos;
    }

    public Block GetBlock(Vector3i blockPosition)
    {
        Chunk chunk = GetChunk(blockPosition);

        if (chunk != null)
        {
            Vector3i localPosition = blockPosition - chunk.Position;
            if (!Chunk.IsLocalPosition(localPosition))
            {
                Debug.LogError(string.Format("Error while getting block {0} on chunk {1} at position {3}, localposition {2}", blockPosition, chunk, localPosition, chunk.Position));
                return new BlockAir();
            }

            return chunk.GetBlock(localPosition);
        }
        else
        {
            return new BlockAir();
        }
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

    public void SetBlock(Vector3i blockPosition, Block block)
    {
        Chunk chunk = GetChunk(blockPosition);

        if (chunk != null)
        {
            chunk.SetBlock(blockPosition - chunk.Position, block);

            UpdateIfEqual(blockPosition.x - chunk.Position.x, 0, blockPosition + Vector3i.Left);
            UpdateIfEqual(blockPosition.x - chunk.Position.x, CHUNK_SIZE_H - 1, blockPosition + Vector3i.Right);
            UpdateIfEqual(blockPosition.y - chunk.Position.y, 0, blockPosition + Vector3i.Down);
            UpdateIfEqual(blockPosition.y - chunk.Position.y, CHUNK_SIZE_V - 1, blockPosition + Vector3i.Up);
            UpdateIfEqual(blockPosition.z - chunk.Position.z, 0, blockPosition + Vector3i.Forward);
            UpdateIfEqual(blockPosition.z - chunk.Position.z, CHUNK_SIZE_H - 1, blockPosition + Vector3i.Backward);
        }
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
