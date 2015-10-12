﻿using UnityEngine;
using System.Collections;
using System;

public class MVChunk : Chunk
{
	#region default_palatte
	public static Color[] DefaultPalatte = new Color[] {
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

	public MVChunk(MVModel model)
		:base(model)
	{

	}

	protected override Block GetExternalBlock(int x, int y, int z)
	{
		return null;
	}

	protected override void SetExternalBlock(int x, int y, int z, Block block)
	{
	}

	public void LoadPalette(Texture2D tex)
	{
		if(tex.height != 1 || tex.width != 256)
		{
			Debug.LogErrorFormat("Error loading palette from texture \"{0}\" : wrong size. Expected 1x256, actual size is {1}x{2}.", tex.name, tex.width, tex.height);
			return;
		}

		Palette = tex.GetPixels();
	}

	public override bool GetHitBox(RaycastHit hit, out GridPosition position, out Vector3 size)
	{
		position = GridPosition.Zero;
		size = new Vector3(
			SizeX * BlockScale,
			SizeY * BlockScale,
			SizeZ * BlockScale);
		return true;
	}
}
