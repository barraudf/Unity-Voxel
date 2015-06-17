using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Save
{
    public Dictionary<Vector3i, Block> Blocks;

    public Save(Chunk chunk)
    {
        Blocks = new Dictionary<Vector3i, Block>();

        for (int x = 0; x < Chunk.CHUNK_SIZE_H; x++)
        for (int y = 0; y < Chunk.CHUNK_SIZE_V; y++)
        for (int z = 0; z < Chunk.CHUNK_SIZE_H; z++)
        {
            if (!chunk.Blocks[x, y, z].changed)
                continue;

            Vector3i pos = new Vector3i(x, y, z);
            Blocks.Add(pos, chunk.Blocks[x, y, z]);
        }
    }
}
