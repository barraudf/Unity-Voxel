using UnityEngine;
using System.Collections;

public class NestedChunkRoot : Block
{
	public float Scale;
	public Chunk NestedChunk;

	public NestedChunkRoot(Chunk nestedChunk, float scale)
		: base()
	{
		Scale = scale;
		NestedChunk = nestedChunk;
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
