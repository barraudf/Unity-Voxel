using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class MVBlock : Block
{
	#region Fields
	[NonSerialized]
	protected MVChunk _Chunk;
	private byte _Alpha;

	/// <summary>
	/// ColorIndex 1 = position 0 in palette array. ColorIndex 255 = position 254 in palette array.
	/// That's the way MagicaVoxel manages color indexes.
	/// </summary>
	public byte ColorIndex;

	/// <summary>
	/// Alpha range (from 1 (transparent) to 255 (opaque)). 0 will be considered opaque too.
	/// </summary>
	public byte Alpha
	{
		get { return _Alpha; }
		set { _Alpha = (value == 0) ? (byte)255 : value; }
	}
	#endregion Fields

	#region Constructor
	public MVBlock(MVChunk parent)
	{
		_Chunk = parent;

		ColorIndex = 0;
		_Alpha = 255;
	}
	#endregion Constructor

	public override Color32 GetBlockColor()
	{
		Color32 color;
        if (_Chunk.Palette.Length > ColorIndex - 1)
		{
			color = _Chunk.Palette[ColorIndex - 1];
			color.a = Alpha;
		}
		else
		{
			color = Color.magenta;
			Debug.LogWarningFormat("MVChunk \"{0}\" palette has no color at index {1} (Array index {2})", _Chunk.Name, ColorIndex, ColorIndex - 1);
		}

		return color;
	}

	public override bool IsFaceVisible(Direction direction, Block adjacentBlock)
	{
        MVBlock block = adjacentBlock as MVBlock;
		if (block == null)
			return base.IsFaceVisible(direction, adjacentBlock);
		else
			return base.IsFaceVisible(direction, adjacentBlock) || block.Alpha != Alpha;
	}
}
