using UnityEngine;
using System.Collections;
using System;

public class MVChunk : Chunk
{
	#region default_palette
	public static Color[] DefaultPalette = new Color[] {
		new Color(1.000000f, 1.000000f, 1.000000f),
		new Color(1.000000f, 1.000000f, 0.800000f),
		new Color(1.000000f, 1.000000f, 0.600000f),
		new Color(1.000000f, 1.000000f, 0.400000f),
		new Color(1.000000f, 1.000000f, 0.200000f),
		new Color(1.000000f, 1.000000f, 0.000000f),
		new Color(1.000000f, 0.800000f, 1.000000f),
		new Color(1.000000f, 0.800000f, 0.800000f),
		new Color(1.000000f, 0.800000f, 0.600000f),
		new Color(1.000000f, 0.800000f, 0.400000f),
		new Color(1.000000f, 0.800000f, 0.200000f),
		new Color(1.000000f, 0.800000f, 0.000000f),
		new Color(1.000000f, 0.600000f, 1.000000f),
		new Color(1.000000f, 0.600000f, 0.800000f),
		new Color(1.000000f, 0.600000f, 0.600000f),
		new Color(1.000000f, 0.600000f, 0.400000f),
		new Color(1.000000f, 0.600000f, 0.200000f),
		new Color(1.000000f, 0.600000f, 0.000000f),
		new Color(1.000000f, 0.400000f, 1.000000f),
		new Color(1.000000f, 0.400000f, 0.800000f),
		new Color(1.000000f, 0.400000f, 0.600000f),
		new Color(1.000000f, 0.400000f, 0.400000f),
		new Color(1.000000f, 0.400000f, 0.200000f),
		new Color(1.000000f, 0.400000f, 0.000000f),
		new Color(1.000000f, 0.200000f, 1.000000f),
		new Color(1.000000f, 0.200000f, 0.800000f),
		new Color(1.000000f, 0.200000f, 0.600000f),
		new Color(1.000000f, 0.200000f, 0.400000f),
		new Color(1.000000f, 0.200000f, 0.200000f),
		new Color(1.000000f, 0.200000f, 0.000000f),
		new Color(1.000000f, 0.000000f, 1.000000f),
		new Color(1.000000f, 0.000000f, 0.800000f),
		new Color(1.000000f, 0.000000f, 0.600000f),
		new Color(1.000000f, 0.000000f, 0.400000f),
		new Color(1.000000f, 0.000000f, 0.200000f),
		new Color(1.000000f, 0.000000f, 0.000000f),
		new Color(0.800000f, 1.000000f, 1.000000f),
		new Color(0.800000f, 1.000000f, 0.800000f),
		new Color(0.800000f, 1.000000f, 0.600000f),
		new Color(0.800000f, 1.000000f, 0.400000f),
		new Color(0.800000f, 1.000000f, 0.200000f),
		new Color(0.800000f, 1.000000f, 0.000000f),
		new Color(0.800000f, 0.800000f, 1.000000f),
		new Color(0.800000f, 0.800000f, 0.800000f),
		new Color(0.800000f, 0.800000f, 0.600000f),
		new Color(0.800000f, 0.800000f, 0.400000f),
		new Color(0.800000f, 0.800000f, 0.200000f),
		new Color(0.800000f, 0.800000f, 0.000000f),
		new Color(0.800000f, 0.600000f, 1.000000f),
		new Color(0.800000f, 0.600000f, 0.800000f),
		new Color(0.800000f, 0.600000f, 0.600000f),
		new Color(0.800000f, 0.600000f, 0.400000f),
		new Color(0.800000f, 0.600000f, 0.200000f),
		new Color(0.800000f, 0.600000f, 0.000000f),
		new Color(0.800000f, 0.400000f, 1.000000f),
		new Color(0.800000f, 0.400000f, 0.800000f),
		new Color(0.800000f, 0.400000f, 0.600000f),
		new Color(0.800000f, 0.400000f, 0.400000f),
		new Color(0.800000f, 0.400000f, 0.200000f),
		new Color(0.800000f, 0.400000f, 0.000000f),
		new Color(0.800000f, 0.200000f, 1.000000f),
		new Color(0.800000f, 0.200000f, 0.800000f),
		new Color(0.800000f, 0.200000f, 0.600000f),
		new Color(0.800000f, 0.200000f, 0.400000f),
		new Color(0.800000f, 0.200000f, 0.200000f),
		new Color(0.800000f, 0.200000f, 0.000000f),
		new Color(0.800000f, 0.000000f, 1.000000f),
		new Color(0.800000f, 0.000000f, 0.800000f),
		new Color(0.800000f, 0.000000f, 0.600000f),
		new Color(0.800000f, 0.000000f, 0.400000f),
		new Color(0.800000f, 0.000000f, 0.200000f),
		new Color(0.800000f, 0.000000f, 0.000000f),
		new Color(0.600000f, 1.000000f, 1.000000f),
		new Color(0.600000f, 1.000000f, 0.800000f),
		new Color(0.600000f, 1.000000f, 0.600000f),
		new Color(0.600000f, 1.000000f, 0.400000f),
		new Color(0.600000f, 1.000000f, 0.200000f),
		new Color(0.600000f, 1.000000f, 0.000000f),
		new Color(0.600000f, 0.800000f, 1.000000f),
		new Color(0.600000f, 0.800000f, 0.800000f),
		new Color(0.600000f, 0.800000f, 0.600000f),
		new Color(0.600000f, 0.800000f, 0.400000f),
		new Color(0.600000f, 0.800000f, 0.200000f),
		new Color(0.600000f, 0.800000f, 0.000000f),
		new Color(0.600000f, 0.600000f, 1.000000f),
		new Color(0.600000f, 0.600000f, 0.800000f),
		new Color(0.600000f, 0.600000f, 0.600000f),
		new Color(0.600000f, 0.600000f, 0.400000f),
		new Color(0.600000f, 0.600000f, 0.200000f),
		new Color(0.600000f, 0.600000f, 0.000000f),
		new Color(0.600000f, 0.400000f, 1.000000f),
		new Color(0.600000f, 0.400000f, 0.800000f),
		new Color(0.600000f, 0.400000f, 0.600000f),
		new Color(0.600000f, 0.400000f, 0.400000f),
		new Color(0.600000f, 0.400000f, 0.200000f),
		new Color(0.600000f, 0.400000f, 0.000000f),
		new Color(0.600000f, 0.200000f, 1.000000f),
		new Color(0.600000f, 0.200000f, 0.800000f),
		new Color(0.600000f, 0.200000f, 0.600000f),
		new Color(0.600000f, 0.200000f, 0.400000f),
		new Color(0.600000f, 0.200000f, 0.200000f),
		new Color(0.600000f, 0.200000f, 0.000000f),
		new Color(0.600000f, 0.000000f, 1.000000f),
		new Color(0.600000f, 0.000000f, 0.800000f),
		new Color(0.600000f, 0.000000f, 0.600000f),
		new Color(0.600000f, 0.000000f, 0.400000f),
		new Color(0.600000f, 0.000000f, 0.200000f),
		new Color(0.600000f, 0.000000f, 0.000000f),
		new Color(0.400000f, 1.000000f, 1.000000f),
		new Color(0.400000f, 1.000000f, 0.800000f),
		new Color(0.400000f, 1.000000f, 0.600000f),
		new Color(0.400000f, 1.000000f, 0.400000f),
		new Color(0.400000f, 1.000000f, 0.200000f),
		new Color(0.400000f, 1.000000f, 0.000000f),
		new Color(0.400000f, 0.800000f, 1.000000f),
		new Color(0.400000f, 0.800000f, 0.800000f),
		new Color(0.400000f, 0.800000f, 0.600000f),
		new Color(0.400000f, 0.800000f, 0.400000f),
		new Color(0.400000f, 0.800000f, 0.200000f),
		new Color(0.400000f, 0.800000f, 0.000000f),
		new Color(0.400000f, 0.600000f, 1.000000f),
		new Color(0.400000f, 0.600000f, 0.800000f),
		new Color(0.400000f, 0.600000f, 0.600000f),
		new Color(0.400000f, 0.600000f, 0.400000f),
		new Color(0.400000f, 0.600000f, 0.200000f),
		new Color(0.400000f, 0.600000f, 0.000000f),
		new Color(0.400000f, 0.400000f, 1.000000f),
		new Color(0.400000f, 0.400000f, 0.800000f),
		new Color(0.400000f, 0.400000f, 0.600000f),
		new Color(0.400000f, 0.400000f, 0.400000f),
		new Color(0.400000f, 0.400000f, 0.200000f),
		new Color(0.400000f, 0.400000f, 0.000000f),
		new Color(0.400000f, 0.200000f, 1.000000f),
		new Color(0.400000f, 0.200000f, 0.800000f),
		new Color(0.400000f, 0.200000f, 0.600000f),
		new Color(0.400000f, 0.200000f, 0.400000f),
		new Color(0.400000f, 0.200000f, 0.200000f),
		new Color(0.400000f, 0.200000f, 0.000000f),
		new Color(0.400000f, 0.000000f, 1.000000f),
		new Color(0.400000f, 0.000000f, 0.800000f),
		new Color(0.400000f, 0.000000f, 0.600000f),
		new Color(0.400000f, 0.000000f, 0.400000f),
		new Color(0.400000f, 0.000000f, 0.200000f),
		new Color(0.400000f, 0.000000f, 0.000000f),
		new Color(0.200000f, 1.000000f, 1.000000f),
		new Color(0.200000f, 1.000000f, 0.800000f),
		new Color(0.200000f, 1.000000f, 0.600000f),
		new Color(0.200000f, 1.000000f, 0.400000f),
		new Color(0.200000f, 1.000000f, 0.200000f),
		new Color(0.200000f, 1.000000f, 0.000000f),
		new Color(0.200000f, 0.800000f, 1.000000f),
		new Color(0.200000f, 0.800000f, 0.800000f),
		new Color(0.200000f, 0.800000f, 0.600000f),
		new Color(0.200000f, 0.800000f, 0.400000f),
		new Color(0.200000f, 0.800000f, 0.200000f),
		new Color(0.200000f, 0.800000f, 0.000000f),
		new Color(0.200000f, 0.600000f, 1.000000f),
		new Color(0.200000f, 0.600000f, 0.800000f),
		new Color(0.200000f, 0.600000f, 0.600000f),
		new Color(0.200000f, 0.600000f, 0.400000f),
		new Color(0.200000f, 0.600000f, 0.200000f),
		new Color(0.200000f, 0.600000f, 0.000000f),
		new Color(0.200000f, 0.400000f, 1.000000f),
		new Color(0.200000f, 0.400000f, 0.800000f),
		new Color(0.200000f, 0.400000f, 0.600000f),
		new Color(0.200000f, 0.400000f, 0.400000f),
		new Color(0.200000f, 0.400000f, 0.200000f),
		new Color(0.200000f, 0.400000f, 0.000000f),
		new Color(0.200000f, 0.200000f, 1.000000f),
		new Color(0.200000f, 0.200000f, 0.800000f),
		new Color(0.200000f, 0.200000f, 0.600000f),
		new Color(0.200000f, 0.200000f, 0.400000f),
		new Color(0.200000f, 0.200000f, 0.200000f),
		new Color(0.200000f, 0.200000f, 0.000000f),
		new Color(0.200000f, 0.000000f, 1.000000f),
		new Color(0.200000f, 0.000000f, 0.800000f),
		new Color(0.200000f, 0.000000f, 0.600000f),
		new Color(0.200000f, 0.000000f, 0.400000f),
		new Color(0.200000f, 0.000000f, 0.200000f),
		new Color(0.200000f, 0.000000f, 0.000000f),
		new Color(0.000000f, 1.000000f, 1.000000f),
		new Color(0.000000f, 1.000000f, 0.800000f),
		new Color(0.000000f, 1.000000f, 0.600000f),
		new Color(0.000000f, 1.000000f, 0.400000f),
		new Color(0.000000f, 1.000000f, 0.200000f),
		new Color(0.000000f, 1.000000f, 0.000000f),
		new Color(0.000000f, 0.800000f, 1.000000f),
		new Color(0.000000f, 0.800000f, 0.800000f),
		new Color(0.000000f, 0.800000f, 0.600000f),
		new Color(0.000000f, 0.800000f, 0.400000f),
		new Color(0.000000f, 0.800000f, 0.200000f),
		new Color(0.000000f, 0.800000f, 0.000000f),
		new Color(0.000000f, 0.600000f, 1.000000f),
		new Color(0.000000f, 0.600000f, 0.800000f),
		new Color(0.000000f, 0.600000f, 0.600000f),
		new Color(0.000000f, 0.600000f, 0.400000f),
		new Color(0.000000f, 0.600000f, 0.200000f),
		new Color(0.000000f, 0.600000f, 0.000000f),
		new Color(0.000000f, 0.400000f, 1.000000f),
		new Color(0.000000f, 0.400000f, 0.800000f),
		new Color(0.000000f, 0.400000f, 0.600000f),
		new Color(0.000000f, 0.400000f, 0.400000f),
		new Color(0.000000f, 0.400000f, 0.200000f),
		new Color(0.000000f, 0.400000f, 0.000000f),
		new Color(0.000000f, 0.200000f, 1.000000f),
		new Color(0.000000f, 0.200000f, 0.800000f),
		new Color(0.000000f, 0.200000f, 0.600000f),
		new Color(0.000000f, 0.200000f, 0.400000f),
		new Color(0.000000f, 0.200000f, 0.200000f),
		new Color(0.000000f, 0.200000f, 0.000000f),
		new Color(0.000000f, 0.000000f, 1.000000f),
		new Color(0.000000f, 0.000000f, 0.800000f),
		new Color(0.000000f, 0.000000f, 0.600000f),
		new Color(0.000000f, 0.000000f, 0.400000f),
		new Color(0.000000f, 0.000000f, 0.200000f),
		new Color(0.933333f, 0.000000f, 0.000000f),
		new Color(0.866667f, 0.000000f, 0.000000f),
		new Color(0.733333f, 0.000000f, 0.000000f),
		new Color(0.666667f, 0.000000f, 0.000000f),
		new Color(0.533333f, 0.000000f, 0.000000f),
		new Color(0.466667f, 0.000000f, 0.000000f),
		new Color(0.333333f, 0.000000f, 0.000000f),
		new Color(0.266667f, 0.000000f, 0.000000f),
		new Color(0.133333f, 0.000000f, 0.000000f),
		new Color(0.066667f, 0.000000f, 0.000000f),
		new Color(0.000000f, 0.933333f, 0.000000f),
		new Color(0.000000f, 0.866667f, 0.000000f),
		new Color(0.000000f, 0.733333f, 0.000000f),
		new Color(0.000000f, 0.666667f, 0.000000f),
		new Color(0.000000f, 0.533333f, 0.000000f),
		new Color(0.000000f, 0.466667f, 0.000000f),
		new Color(0.000000f, 0.333333f, 0.000000f),
		new Color(0.000000f, 0.266667f, 0.000000f),
		new Color(0.000000f, 0.133333f, 0.000000f),
		new Color(0.000000f, 0.066667f, 0.000000f),
		new Color(0.000000f, 0.000000f, 0.933333f),
		new Color(0.000000f, 0.000000f, 0.866667f),
		new Color(0.000000f, 0.000000f, 0.733333f),
		new Color(0.000000f, 0.000000f, 0.666667f),
		new Color(0.000000f, 0.000000f, 0.533333f),
		new Color(0.000000f, 0.000000f, 0.466667f),
		new Color(0.000000f, 0.000000f, 0.333333f),
		new Color(0.000000f, 0.000000f, 0.266667f),
		new Color(0.000000f, 0.000000f, 0.133333f),
		new Color(0.000000f, 0.000000f, 0.066667f),
		new Color(0.933333f, 0.933333f, 0.933333f),
		new Color(0.866667f, 0.866667f, 0.866667f),
		new Color(0.733333f, 0.733333f, 0.733333f),
		new Color(0.666667f, 0.666667f, 0.666667f),
		new Color(0.533333f, 0.533333f, 0.533333f),
		new Color(0.466667f, 0.466667f, 0.466667f),
		new Color(0.333333f, 0.333333f, 0.333333f),
		new Color(0.266667f, 0.266667f, 0.266667f),
		new Color(0.133333f, 0.133333f, 0.133333f),
		new Color(0.066667f, 0.066667f, 0.066667f),
		new Color(0.000000f, 0.000000f, 0.000000f)
	};
	#endregion

	public Color[] Palette;
	public string Name;
	public byte[] Version;

	private byte[,,] _ColorIndexes;

	public MVChunk(MVModel model)
		:base(model)
	{

	}

	public override void InitBlocks(int sizeX, int sizeY, int sizeZ)
	{
		base.InitBlocks(sizeX, sizeY, sizeZ);
		_ColorIndexes = new byte[sizeX, sizeY, sizeZ];
	}

	public void LoadPalette(Texture2D tex)
	{
		if(tex.height != 1 || tex.width != 256)
		{
			Debug.LogErrorFormat("Error loading palette from texture \"{0}\" : wrong size. Expected 1x256, actual size is {1}x{2}.", tex.name, tex.width, tex.height);
			return;
		}

		Palette = tex.GetPixels();
		UpdateColors();
    }

	public override bool GetHitBox(RaycastHit hit, out GridPosition position, out Vector3 size)
	{
		position = GridPosition.Zero;
		size = Vector3.Scale(new Vector3(SizeX, SizeY, SizeZ), BlockScale);
		return true;
	}

	public void UpdateColors()
	{
		for (int x = 0; x < SizeX; x++)
			for (int y = 0; y < SizeY; y++)
				for (int z = 0; z < SizeZ; z++)
				{
					int index = _ColorIndexes[x, y, z];
                    if (Palette.Length > index - 1)
					{
						Color32 c = Palette[index - 1];
						Blocks[x, y, z].Color.r = c.r;
						Blocks[x, y, z].Color.g = c.g;
						Blocks[x, y, z].Color.b = c.b;
					}
					else
					{
						Blocks[x, y, z].Color = Color.magenta;
						Debug.LogWarningFormat("MVChunk \"{0}\" palette has no color at index {1} (Array index {2})", Name, index, index - 1);
					}
				}
	}

	public void UpdateColorIndex(int x, int y, int z, byte index)
	{
		_ColorIndexes[x, y, z] = index;
    }
}
