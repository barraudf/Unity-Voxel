﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class LoadChunks : MonoBehaviour
{
    public World world;

    private ConcurrentQueue<Vector3i> updateList;
    private ConcurrentQueue<Vector3i> buildList;
    int timer = 0;

    private static Vector3i[] chunkPositions = {   new Vector3i( 0, 0,  0), new Vector3i(-1, 0,  0), new Vector3i( 0, 0, -1), new Vector3i( 0, 0,  1), new Vector3i( 1, 0,  0),
                             new Vector3i(-1, 0, -1), new Vector3i(-1, 0,  1), new Vector3i( 1, 0, -1), new Vector3i( 1, 0,  1), new Vector3i(-2, 0,  0),
                             new Vector3i( 0, 0, -2), new Vector3i( 0, 0,  2), new Vector3i( 2, 0,  0), new Vector3i(-2, 0, -1), new Vector3i(-2, 0,  1),
                             new Vector3i(-1, 0, -2), new Vector3i(-1, 0,  2), new Vector3i( 1, 0, -2), new Vector3i( 1, 0,  2), new Vector3i( 2, 0, -1),
                             new Vector3i( 2, 0,  1), new Vector3i(-2, 0, -2), new Vector3i(-2, 0,  2), new Vector3i( 2, 0, -2), new Vector3i( 2, 0,  2),
                             new Vector3i(-3, 0,  0), new Vector3i( 0, 0, -3), new Vector3i( 0, 0,  3), new Vector3i( 3, 0,  0), new Vector3i(-3, 0, -1),
                             new Vector3i(-3, 0,  1), new Vector3i(-1, 0, -3), new Vector3i(-1, 0,  3), new Vector3i( 1, 0, -3), new Vector3i( 1, 0,  3),
                             new Vector3i( 3, 0, -1), new Vector3i( 3, 0,  1), new Vector3i(-3, 0, -2), new Vector3i(-3, 0,  2), new Vector3i(-2, 0, -3),
                             new Vector3i(-2, 0,  3), new Vector3i( 2, 0, -3), new Vector3i( 2, 0,  3), new Vector3i( 3, 0, -2), new Vector3i( 3, 0,  2),
                             new Vector3i(-4, 0,  0), new Vector3i( 0, 0, -4), new Vector3i( 0, 0,  4), new Vector3i( 4, 0,  0), new Vector3i(-4, 0, -1),
                             new Vector3i(-4, 0,  1), new Vector3i(-1, 0, -4), new Vector3i(-1, 0,  4), new Vector3i( 1, 0, -4), new Vector3i( 1, 0,  4),
                             new Vector3i( 4, 0, -1), new Vector3i( 4, 0,  1), new Vector3i(-3, 0, -3), new Vector3i(-3, 0,  3), new Vector3i( 3, 0, -3),
                             new Vector3i( 3, 0,  3), new Vector3i(-4, 0, -2), new Vector3i(-4, 0,  2), new Vector3i(-2, 0, -4), new Vector3i(-2, 0,  4),
                             new Vector3i( 2, 0, -4), new Vector3i( 2, 0,  4), new Vector3i( 4, 0, -2), new Vector3i( 4, 0,  2), new Vector3i(-5, 0,  0),
                             new Vector3i(-4, 0, -3), new Vector3i(-4, 0,  3), new Vector3i(-3, 0, -4), new Vector3i(-3, 0,  4), new Vector3i( 0, 0, -5),
                             new Vector3i( 0, 0,  5), new Vector3i( 3, 0, -4), new Vector3i( 3, 0,  4), new Vector3i( 4, 0, -3), new Vector3i( 4, 0,  3),
                             new Vector3i( 5, 0,  0), new Vector3i(-5, 0, -1), new Vector3i(-5, 0,  1), new Vector3i(-1, 0, -5), new Vector3i(-1, 0,  5),
                             new Vector3i( 1, 0, -5), new Vector3i( 1, 0,  5), new Vector3i( 5, 0, -1), new Vector3i( 5, 0,  1), new Vector3i(-5, 0, -2),
                             new Vector3i(-5, 0,  2), new Vector3i(-2, 0, -5), new Vector3i(-2, 0,  5), new Vector3i( 2, 0, -5), new Vector3i( 2, 0,  5),
                             new Vector3i( 5, 0, -2), new Vector3i( 5, 0,  2), new Vector3i(-4, 0, -4), new Vector3i(-4, 0,  4), new Vector3i( 4, 0, -4),
                             new Vector3i( 4, 0,  4), new Vector3i(-5, 0, -3), new Vector3i(-5, 0,  3), new Vector3i(-3, 0, -5), new Vector3i(-3, 0,  5),
                             new Vector3i( 3, 0, -5), new Vector3i( 3, 0,  5), new Vector3i( 5, 0, -3), new Vector3i( 5, 0,  3), new Vector3i(-6, 0,  0),
                             new Vector3i( 0, 0, -6), new Vector3i( 0, 0,  6), new Vector3i( 6, 0,  0), new Vector3i(-6, 0, -1), new Vector3i(-6, 0,  1),
                             new Vector3i(-1, 0, -6), new Vector3i(-1, 0,  6), new Vector3i( 1, 0, -6), new Vector3i( 1, 0,  6), new Vector3i( 6, 0, -1),
                             new Vector3i( 6, 0,  1), new Vector3i(-6, 0, -2), new Vector3i(-6, 0,  2), new Vector3i(-2, 0, -6), new Vector3i(-2, 0,  6),
                             new Vector3i( 2, 0, -6), new Vector3i( 2, 0,  6), new Vector3i( 6, 0, -2), new Vector3i( 6, 0,  2), new Vector3i(-5, 0, -4),
                             new Vector3i(-5, 0,  4), new Vector3i(-4, 0, -5), new Vector3i(-4, 0,  5), new Vector3i( 4, 0, -5), new Vector3i( 4, 0,  5),
                             new Vector3i( 5, 0, -4), new Vector3i( 5, 0,  4), new Vector3i(-6, 0, -3), new Vector3i(-6, 0,  3), new Vector3i(-3, 0, -6),
                             new Vector3i(-3, 0,  6), new Vector3i( 3, 0, -6), new Vector3i( 3, 0,  6), new Vector3i( 6, 0, -3), new Vector3i( 6, 0,  3),
                             new Vector3i(-7, 0,  0), new Vector3i( 0, 0, -7), new Vector3i( 0, 0,  7), new Vector3i( 7, 0,  0), new Vector3i(-7, 0, -1),
                             new Vector3i(-7, 0,  1), new Vector3i(-5, 0, -5), new Vector3i(-5, 0,  5), new Vector3i(-1, 0, -7), new Vector3i(-1, 0,  7),
                             new Vector3i( 1, 0, -7), new Vector3i( 1, 0,  7), new Vector3i( 5, 0, -5), new Vector3i( 5, 0,  5), new Vector3i( 7, 0, -1),
                             new Vector3i( 7, 0,  1), new Vector3i(-6, 0, -4), new Vector3i(-6, 0,  4), new Vector3i(-4, 0, -6), new Vector3i(-4, 0,  6),
                             new Vector3i( 4, 0, -6), new Vector3i( 4, 0,  6), new Vector3i( 6, 0, -4), new Vector3i( 6, 0,  4), new Vector3i(-7, 0, -2),
                             new Vector3i(-7, 0,  2), new Vector3i(-2, 0, -7), new Vector3i(-2, 0,  7), new Vector3i( 2, 0, -7), new Vector3i( 2, 0,  7),
                             new Vector3i( 7, 0, -2), new Vector3i( 7, 0,  2), new Vector3i(-7, 0, -3), new Vector3i(-7, 0,  3), new Vector3i(-3, 0, -7),
                             new Vector3i(-3, 0,  7), new Vector3i( 3, 0, -7), new Vector3i( 3, 0,  7), new Vector3i( 7, 0, -3), new Vector3i( 7, 0,  3),
                             new Vector3i(-6, 0, -5), new Vector3i(-6, 0,  5), new Vector3i(-5, 0, -6), new Vector3i(-5, 0,  6), new Vector3i( 5, 0, -6),
                             new Vector3i( 5, 0,  6), new Vector3i( 6, 0, -5), new Vector3i( 6, 0,  5) };


    public LoadChunks()
    {
        updateList = new ConcurrentQueue<Vector3i>();
        buildList = new ConcurrentQueue<Vector3i>();
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

                if (distance > 256)
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
}
