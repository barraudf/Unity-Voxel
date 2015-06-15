﻿using UnityEngine;
using System.Collections.Generic;

public class LoadChunks : MonoBehaviour
{
    public World world;

    private List<Vector3i> updateList;
    private List<Vector3i> buildList;
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
        updateList = new List<Vector3i>();
        buildList = new List<Vector3i>();
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        Vector3i playerPos = new Vector3i(
            Mathf.FloorToInt(transform.position.x / Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE,
            Mathf.FloorToInt(transform.position.y / Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE,
            Mathf.FloorToInt(transform.position.z / Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE
            );

        //If there aren't already chunks to generate
        if (buildList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                Vector3i newChunkPos = new Vector3i(
                    chunkPositions[i].x * Chunk.CHUNK_SIZE + playerPos.x,
                    0,
                    chunkPositions[i].z * Chunk.CHUNK_SIZE + playerPos.z
                    );

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(newChunkPos);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

                //load a column of chunks in this position
                for (int y = -4; y < 4; y++)
                {
                    buildList.Add(new Vector3i(
                        newChunkPos.x, y * Chunk.CHUNK_SIZE, newChunkPos.z));
                }
                return;
            }
        }
    }

    void BuildChunk(Vector3i pos)
    {
        for (int y = pos.y - Chunk.CHUNK_SIZE; y <= pos.y + Chunk.CHUNK_SIZE; y += Chunk.CHUNK_SIZE)
        {
            if (y > 64 || y < -64)
                continue;

            for (int x = pos.x - Chunk.CHUNK_SIZE; x <= pos.x + Chunk.CHUNK_SIZE; x += Chunk.CHUNK_SIZE)
            {
                for (int z = pos.z - Chunk.CHUNK_SIZE; z <= pos.z + Chunk.CHUNK_SIZE; z += Chunk.CHUNK_SIZE)
                {
                    Vector3i coord = new Vector3i(x, y, z);
                    if (world.GetChunk(coord) == null)
                        world.CreateChunk(coord);
                }
            }
        }

        updateList.Add(pos);
    }

    void LoadAndRenderChunks()
    {
        for (int i = 0; i < 4; i++)
        {
            if (buildList.Count != 0)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }
        }

        for (int i = 0; i < updateList.Count; i++)
        {
            Chunk chunk = world.GetChunk(updateList[0]);
            if (chunk != null)
                chunk.Invalidate();
            updateList.RemoveAt(0);
        }
    }

    void Update()
    {
        DeleteChunks();
        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    void DeleteChunks()
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
        }

        timer++;
    }
}
