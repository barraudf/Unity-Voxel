using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{
    float stoneBaseHeight = 42;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = 30;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.Position.x; x < chunk.Position.x + Chunk.CHUNK_SIZE_H; x++)
            for (int z = chunk.Position.z; z < chunk.Position.z + Chunk.CHUNK_SIZE_H; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }

        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(new Vector3i(x, 0, z), stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(new Vector3i(x, 0, z), stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(new Vector3i(x, 100, z), dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.Position.y; y < chunk.Position.y + Chunk.CHUNK_SIZE_V; y++)
        {
            Vector3i position = new Vector3i(x, y, z) - chunk.Position;
            if (y <= stoneHeight)
            {
                chunk.SetBlock(position, new BlockRock());
            }
            else if (y <= dirtHeight)
            {
                chunk.SetBlock(position, new BlockGrass());
            }
            else
            {
                chunk.SetBlock(position, new BlockAir());
            }

        }

        return chunk;
    }

    public static int GetNoise(Vector3i coord, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(coord.x * scale, coord.y * scale, coord.z * scale) + 1f) * (max / 2f));
    }
}
