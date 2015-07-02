using UnityEngine;
using System.Collections.Generic;
using System.Threading;

using Object = System.Object;
using System.Collections.Concurrent;

/// <summary>
/// The WorldNavigator is responsible for loading the chunks around the GameObject position
/// and unloading them as they get too far away
/// </summary>
public class WorldNavigator : MonoBehaviour
{
    public World World;
    public int RenderDistance = 10;
    public float DelayBetweenUnloads = 0.5f;

    private static Vector3i[] ChunkOffsets;
    private int SquaredRenderDistance;

    protected Vector3i? LastChunkPosition = null;
    protected ConcurrentQueue<Vector3i> ChunkLoadQueue;
    protected Vector3i CurrentPosition;

    private void Awake()
    {
        SquaredRenderDistance = RenderDistance * RenderDistance;
        ChunkOffsets = GenerateChunkOffsets();
        ChunkLoadQueue = new ConcurrentQueue<Vector3i>();
    }

    private void Start()
    {
    }
    
    private void Update()
    {
        CurrentPosition = Chunk.GetChunkCoordinates((Vector3i)transform.position);
        CurrentPosition.y = 0;

        if (!LastChunkPosition.HasValue || !LastChunkPosition.Equals(CurrentPosition))
        {
            FindChunksToLoad();
        }

        Vector3i chunkPosition;
        if(ChunkLoadQueue.TryDequeue(out chunkPosition))
        {
            LoadChunk(chunkPosition);
        }

        if (!LastChunkPosition.HasValue || !LastChunkPosition.Equals(CurrentPosition))
        {
            //UnloadChunks();
        }

        if(!LastChunkPosition.HasValue || !LastChunkPosition.Equals(CurrentPosition))
            LastChunkPosition = CurrentPosition;
    }

    /// <summary>
    /// Generate a list of relative chunk coordinates which are less than *RenderDistance* chunks away from a point ordered by distance
    /// </summary>
    /// <returns></returns>
    public Vector3i[] GenerateChunkOffsets()
    {
        List<Vector3i> chunksSortedByDistance = new List<Vector3i>();

        for (int x = -RenderDistance; x <= RenderDistance; x++)
            for (int z = -RenderDistance; z <= RenderDistance; z++)
            {
                if (x * x + z * z <= SquaredRenderDistance)
                    chunksSortedByDistance.Add(new Vector3i(x, 0, z));
            }

        chunksSortedByDistance.Sort((firstChunk, secondChunk) => CompareDistanceFromPosition(firstChunk, secondChunk, Vector3i.Zero));

        return chunksSortedByDistance.ToArray();
    }

    /// <summary>
    /// Compare the distance of two values from a given position
    /// </summary>
    /// <param name="value1">The first value to compare</param>
    /// <param name="value2">The second value to copare</param>
    /// <param name="position">The position</param>
    /// <returns>relative value depending on the distance</returns>
    private static int CompareDistanceFromPosition(Vector3i value1, Vector3i value2, Vector3i position)
    {
        return Vector3i.DistanceSquared(value1, position).CompareTo(Vector3i.DistanceSquared(value2, position));
    }

    /// <summary>
    /// Find chunks which are less than *RenderDistance* chunks away from the current position and load them if they aren't already
    /// </summary>
    void FindChunksToLoad()
    {
        for (int i = 0; i < ChunkOffsets.Length; i++)
        {
            Vector3i targetPos = (ChunkOffsets[i] * Vector3i.ChunkSize) + CurrentPosition;

            Chunk target = World.GetChunk(targetPos);

            if (target != null && target.DataLoaded)
                continue;

            ChunkLoadQueue.Enqueue(targetPos);
        }
    }

    protected void LoadChunk(Vector3i targetPos)
    {
        // Load the target chunk and the chunks next to it in all directions
        for (int x = targetPos.x - World.CHUNK_SIZE_H; x <= targetPos.x + World.CHUNK_SIZE_H; x+= World.CHUNK_SIZE_H)
            for (int z = targetPos.z - World.CHUNK_SIZE_H; z <= targetPos.z + World.CHUNK_SIZE_H; z += World.CHUNK_SIZE_H)
            {
                bool isTarget = (x == targetPos.x && z == targetPos.z);
                Vector3i chunkPos = new Vector3i(x, 0, z);// +targetPos;
                Chunk chunk = World.GetChunk(chunkPos);

                if (chunk == null)
                {
                    chunk = World.CreateChunk(chunkPos, isTarget);
                }
                else if (isTarget && chunk.DataLoaded == false)
                {
                    // The current target has already been created for a past target's rendering, but hasn't been rendered itself
                    chunk.DataLoaded = true;
                }
            }

        if (World.MultithreadProcessing)
            ThreadPool.QueueUserWorkItem(p => RenderChunkAfterLoad((Vector3i)p), targetPos);
        else
            RenderChunkAfterLoad(targetPos);
    }

    private void UnloadChunks()
    {
        Chunk currentChunk = World.GetChunk(CurrentPosition);

        if (currentChunk != null)
        {
            List<Vector3i> chunksToDelete = new List<Vector3i>();
            foreach (KeyValuePair<Vector3i, Chunk> chunk in World.Chunks)
            {
                double distance = Vector3i.DistanceSquared(chunk.Value.Position / Vector3i.ChunkSize, currentChunk.Position / Vector3i.ChunkSize);

                float max = SquaredRenderDistance * World.CHUNK_SIZE_H * World.CHUNK_SIZE_H;
                double dist = distance * distance;

                if (dist > max)
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (Vector3i chunk in chunksToDelete)
            {
                Debug.LogFormat("chunkunload {0}", chunk);
                World.DestroyChunk(chunk);
            }
        }

        //Invoke("UnloadChunks", DelayBetweenUnloads);
    }

    /// <summary>
    /// Wait for a chunk to be loaded, then build its mesh
    /// </summary>
    /// <param name="chunkPos">The position of the chunk to render</param>
    private void RenderChunkAfterLoad(Vector3i chunkPos)
    {
        Chunk c;

        do
        {
            c = World.GetChunk(chunkPos);

            // Don't suspend the main thread!
            if (World.MultithreadProcessing)
                Thread.Sleep(100);
        }
        while (c == null || !c.DataLoaded);

        c.StartMeshBuilding();
    }
}
