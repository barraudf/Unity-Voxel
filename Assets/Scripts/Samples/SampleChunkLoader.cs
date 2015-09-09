using UnityEngine;
using System.Collections;
using System;

public class SampleChunkLoader : ChunkLoader
{
	public override void LoadChunk(Chunk chunk)
	{
		WorldChunk wChunk = chunk as WorldChunk;
		if (wChunk == null)
			return;

		chunk.InitBlocks(wChunk.World.ChunkSizeX, wChunk.World.ChunkSizeY, wChunk.World.ChunkSizeZ);
		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					//if ((x + y + z) % 2 == 0)
					//if(y < chunk.SizeY / 2)
						chunk.SetBlock(x, y, z, SampleBlock.Instance);
				}
	}
}
