using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public abstract class Block
{
	#region Fields
	/// <summary>
	/// Stores the color of each block type loaded
	/// </summary>
	[NonSerialized]
	protected static Dictionary<Type, Color32> Colors = new Dictionary<Type, Color32>();
	#endregion Fields

	/// <summary>
	/// Determines if we should draw our neighboor's face in the opposite direction
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public virtual bool IsSolid(Direction direction)
	{
		return true;
	}

	/// <summary>
	/// Determines the color of the block
	/// </summary>
	/// <returns>the color of the block</returns>
	public virtual Color32 GetBlockColor()
	{
		return Colors[GetType()];
	}

	/// <summary>
	/// Determines if a face should be visible, given the block adjacent to this face
	/// </summary>
	/// <param name="direction">Direction (from [adjacentBlock] to current block)</param>
	/// <param name="adjacentBlock">the block next to this one</param>
	/// <returns>True if the face should be renderer, false otherwise</returns>
	public virtual bool IsFaceVisible(Direction direction, Block adjacentBlock)
	{
		return adjacentBlock == null || !adjacentBlock.IsSolid(direction);
	}
}
