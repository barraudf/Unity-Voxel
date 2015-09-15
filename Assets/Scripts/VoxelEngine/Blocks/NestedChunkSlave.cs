using UnityEngine;
using System.Collections;

public class NestedChunkSlave : Block
{
	public NestedChunkRoot Root;

	public NestedChunkSlave(NestedChunkRoot root)
		:base()
	{
		Root = root;
	}

	public override bool IsSolid(Direction direction)
	{
		return false;
	}

	public override bool IsFaceVisible(Direction direction, Block adjacentBlock)
	{
		return false;
	}
}
