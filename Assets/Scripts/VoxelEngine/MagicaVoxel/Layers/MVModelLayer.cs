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
		chunk.Blocks = new Block[sizeX, sizeY, sizeZ];
	}

	public override void ReadVolxel(MVChunk chunk, int x, int y, int z, byte index)
	{
		if (chunk.Blocks[x, y, z] == null)
			chunk.Blocks[x, y, z] = new MVBlock(chunk);

		((MVBlock)chunk.Blocks[x, y, z]).ColorIndex = index;
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
