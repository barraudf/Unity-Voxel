using UnityEngine;

public class SampleSimpleChunkLoader : ChunkLoader
{
	public override void LoadChunk(Chunk chunk)
	{
		WorldChunk wChunk = chunk as WorldChunk;
		if (wChunk == null)
			return;

		chunk.InitBlocks(wChunk.World.ChunkSizeX, wChunk.World.ChunkSizeY, wChunk.World.ChunkSizeZ);

		for (int x = 0; x < wChunk.SizeX; x++)
			for (int z = 0; z < wChunk.SizeZ; z++)
			{
				if (x == 0 && z != 0)
					wChunk.SetBlock(x, 0, z, SampleBlockXAxis.Instance, false);
				else if (z == 0 && x != 0)
					wChunk.SetBlock(x, 0, z, SampleBlockZAxis.Instance, false);
				else if ((x + z) % 2 == 0)
					wChunk.SetBlock(x, 0, z, SampleBlock.Instance, false);
				else
					wChunk.SetBlock(x, 0, z, SampleBlock2.Instance, false);
			}

		for (int y = 0; y < chunk.SizeX; y++)
			wChunk.SetBlock(0, y, 0, SampleBlockYAxis.Instance, false);
	}
}
