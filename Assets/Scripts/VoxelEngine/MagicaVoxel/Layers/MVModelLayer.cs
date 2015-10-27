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
		Block block = chunk.GetBlock(x, y, z);
		block.Type = Block.BlockTypes.Solid;
		if (chunk.Palette.Length > index - 1)
		{
			block.Color = chunk.Palette[index - 1];
		}
		else
		{
			block.Color = Color.magenta;
			Debug.LogWarningFormat("MVChunk \"{0}\" palette has no color at index {1} (Array index {2})", chunk.Name, index, index - 1);
		}
		chunk.UpdateColorIndex(x, y, z, index);
		chunk.SetBlock(x, y, z, block);
	}

	public override void InitPalette(MVChunk chunk)
	{
		chunk.Palette = new Color[256];
	}

	public override void InitDefaultPalette(MVChunk chunk)
	{
		chunk.Palette = MVChunk.DefaultPalette;
	}

	public override void ReadVersion(MVChunk chunk, byte[] version)
	{
		chunk.Version = version;
	}
}
