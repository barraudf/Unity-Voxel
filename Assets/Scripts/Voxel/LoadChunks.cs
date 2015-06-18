using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class LoadChunks : MonoBehaviour
{
    public World world;

    public int RenderDistance = 10;

    private ConcurrentQueue<Vector3i> updateList;
    private ConcurrentQueue<Vector3i> buildList;
    int timer = 0;

    private static Vector3i[] chunkPositions;


    public LoadChunks()
    {
        updateList = new ConcurrentQueue<Vector3i>();
        buildList = new ConcurrentQueue<Vector3i>();
    }

    void Start()
    {
        chunkPositions = CreateMapChunksForGeneration();
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        Vector3i playerPos = new Vector3i(
            Mathf.FloorToInt(transform.position.x / Chunk.CHUNK_SIZE_H) * Chunk.CHUNK_SIZE_H,
            Mathf.FloorToInt(transform.position.y / Chunk.CHUNK_SIZE_V) * Chunk.CHUNK_SIZE_V,
            Mathf.FloorToInt(transform.position.z / Chunk.CHUNK_SIZE_H) * Chunk.CHUNK_SIZE_H
            );

        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                Vector3i newChunkPos = new Vector3i(
                    chunkPositions[i].x * Chunk.CHUNK_SIZE_H + playerPos.x,
                    0,
                    chunkPositions[i].z * Chunk.CHUNK_SIZE_H + playerPos.z
                    );

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(newChunkPos);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null && newChunk.rendered)
                    continue;

                //load a column of chunks in this position
                for (int x = newChunkPos.x - Chunk.CHUNK_SIZE_H; x <= newChunkPos.x + Chunk.CHUNK_SIZE_H; x += Chunk.CHUNK_SIZE_H)
                for (int z = newChunkPos.z - Chunk.CHUNK_SIZE_H; z <= newChunkPos.z + Chunk.CHUNK_SIZE_H; z += Chunk.CHUNK_SIZE_H)
                {
                    buildList.Enqueue(new Vector3i(x, 0, z));
                }
                updateList.Enqueue(new Vector3i(newChunkPos.x, 0, newChunkPos.z));
                return;
            }
        }
    }

    bool BuildChunk(Vector3i pos)
    {
        if (world.GetChunk(pos) == null)
        {
            world.CreateChunk(pos);
            return true;
        }
        else
        {
            return false;
        }
    }

    void LoadAndRenderChunks()
    {
        Vector3i chunkPos;
        while (buildList.TryDequeue(out chunkPos))
        {
            if (!BuildChunk(chunkPos))
                continue;

            //If chunks were built return early
            return;
        }

        while (updateList.TryDequeue(out chunkPos))
        {
            Chunk chunk = world.GetChunk(chunkPos);
            if (chunk != null)
            {
                if (chunk.UpdateNeeded == true)
                    continue;

                chunk.UpdateNeeded = true;

                return;
            }
        }
    }

    void Update()
    {
        if (DeleteChunks())
            return;

        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    bool DeleteChunks()
    {
        if (timer == 10)
        {
            var chunksToDelete = new List<Vector3i>();
            foreach (var chunk in world.Chunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.Position.x, 0, chunk.Value.Position.z),
                    new Vector3(transform.position.x, 0, transform.position.z));

                if (distance > (RenderDistance + 5) * Chunk.CHUNK_SIZE_H)
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }

    public Vector3i[] CreateMapChunksForGeneration()
    {
        List<Vector3i> chunksSortedByDistance = new List<Vector3i>();
        for (int x = -RenderDistance; x < RenderDistance; x++)
            for (int z = -RenderDistance; z < RenderDistance; z++)
            {
                chunksSortedByDistance.Add(new Vector3i(x, 0, z));
            }

        // sort them now, by distance
        chunksSortedByDistance.Sort((firstChunk, secondChunk) => ChunksComparedByDistanceFromMapCenter(firstChunk, Vector3i.zero, secondChunk));

        return chunksSortedByDistance.ToArray();
    }

    // this is the actual comparison method that compares them by distance
    private static int ChunksComparedByDistanceFromMapCenter(Vector3i firstChunk, Vector3i mapCenter, Vector3i secondChunk)
    {
        return Vector3i.DistanceSquared(firstChunk, mapCenter).CompareTo(Vector3i.DistanceSquared(secondChunk, mapCenter));
    }
}
