using UnityEngine;
using System.Collections;

public abstract class MVLayer
{
	public string FilePath;

	public MVLayer(string filePath)
	{
		FilePath = filePath;
	}

	public abstract void ReadVersion(MVChunk chunk, byte[] version);
	public abstract void ReadSize(MVChunk chunk, int sizeX, int sizeY, int sizeZ);
	public abstract void ReadVolxel(MVChunk chunk, int x, int y, int z, byte index);

	public abstract void InitPalette(MVChunk chunk);
	public abstract void InitDefaultPalette(MVChunk chunk);
	public abstract void ReadPalette(MVChunk chunk, int index, float r, float g, float b, float a);
}
