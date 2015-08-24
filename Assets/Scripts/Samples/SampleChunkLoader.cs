using UnityEngine;
using System.Collections;
using System;

public class SampleChunkLoader : ChunkLoader
{
	public override void LoadChunk(Chunk chunk)
	{
		chunk.Blocks = new Block[16, 256, 16];
		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					if ((x + y + z) % 2 == 0)
						chunk.Blocks[x, y, z] = new SampleBlock();
				}
	}
}
