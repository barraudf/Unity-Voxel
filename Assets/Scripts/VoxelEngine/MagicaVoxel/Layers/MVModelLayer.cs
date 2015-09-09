using UnityEngine;
using System.Collections;
using System;

public class MVModelLayer : MVLayer
{
	public MVModelLayer(string filePath)
		:base(filePath)
	{

	}

	public override void ReadPalette(MVChunk chunk, int index, float r, float g, float b,float a)
	{
		chunk.Palette[index] = new Color(r, g, b, a);
	}

	public override void ReadSize(MVChunk chunk, int sizeX, int sizeY, int sizeZ)
	{
		chunk.InitBlocks(sizeX, sizeY, sizeZ);
	}

	public override void ReadVolxel(MVChunk chunk, int x, int y, int z, byte index)
	{
		MVBlock block = chunk.GetBlock(x, y, z) as MVBlock;
		if (block == null)
		{
			block = new MVBlock(chunk);
			chunk.SetBlock(x, y, z, block);
		}

		block.ColorIndex = index;
	}

	public override void InitPalette(MVChunk chunk)
	{
		chunk.Palette = new Color[256];
	}

	public override void InitDefaultPalette(MVChunk chunk)
	{
		chunk.Palette = MVChunk.DefaultPalatte;
	}

	public override void ReadVersion(MVChunk chunk, byte[] version)
	{
		chunk.Version = version;
	}
}
