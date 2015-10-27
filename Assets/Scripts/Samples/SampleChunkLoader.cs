using UnityEngine;
using System.Collections;
using System;
using SimplexNoise;

public class SampleChunkLoader : ChunkLoader
{
	float stoneBaseHeight = 24;
	float stoneBaseNoise = 0.05f;
	float stoneBaseNoiseHeight = 4;

	float stoneMountainHeight = 48;
	float stoneMountainFrequency = 0.008f;
	float stoneMinHeight = -12;

	float dirtBaseHeight = 1;
	float dirtNoise = 0.04f;
	float dirtNoiseHeight = 3;

	public override void LoadChunk(Chunk chunk)
	{
		WorldChunk wChunk = chunk as WorldChunk;
		if (wChunk == null)
			return;

		chunk.InitBlocks(wChunk.World.ChunkSizeX, wChunk.World.ChunkSizeY, wChunk.World.ChunkSizeZ);

		for (int x = 0; x < wChunk.SizeX; x++)
			for (int z = 0; z < wChunk.SizeZ; z++)
			{
				int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
				stoneHeight += GetNoise(wChunk.Position.x * wChunk.SizeX + x, 0, wChunk.Position.z * wChunk.SizeZ + z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

				if (stoneHeight < stoneMinHeight)
					stoneHeight = Mathf.FloorToInt(stoneMinHeight);

				stoneHeight += GetNoise(wChunk.Position.x * wChunk.SizeX + x, 0, wChunk.Position.z * wChunk.SizeZ + z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

				int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
				dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));


				for (int y = 0; y < wChunk.SizeY; y++)
				{
					if (y <= stoneHeight)
					{
						wChunk.SetBlock(x, y, z, new Block { Color = new Color32() { a = 255, r = 127, g = 127, b = 127 }, Type = Block.BlockTypes.Solid } , false);
					}
					else if (y <= dirtHeight)
					{
						wChunk.SetBlock(x, y, z, new Block { Color = new Color32() { a = 255, r = 50, g = 255, b = 50 }, Type = Block.BlockTypes.Solid }, false);
					}
				}
			}
	}

	public static int GetNoise(int x, int y, int z, float scale, int max)
	{
		return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
	}
}
