using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct Block
{
	public enum BlockTypes : byte { Air, Solid };

	#region Fields
	public Color32 Color;
	public BlockTypes Type;
	#endregion Fields

	/// <summary>
	/// Determines if we should draw our neighboor's face in the opposite direction
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public bool IsSolid(Direction direction)
	{
		return Type != BlockTypes.Air;
	}

	/// <summary>
	/// Determines which submesh the block belongs to
	/// </summary>
	/// <returns>the submesh</returns>
	public SubMeshes GetSubMesh()
	{
		return Color.a == 255 ? SubMeshes.Opac : SubMeshes.Transparent;
	}

	/// <summary>
	/// Determines if a face should be visible, given the block adjacent to this face
	/// </summary>
	/// <param name="direction">Direction (from [adjacentBlock] to current block)</param>
	/// <param name="adjacentBlock">the block next to this one</param>
	/// <returns>True if the face should be renderer, false otherwise</returns>
	public bool IsFaceVisible(Direction direction, Block adjacentBlock)
	{
		return IsSolid(direction) && (!adjacentBlock.IsSolid(direction) || adjacentBlock.Color.a != Color.a);
	}
}
