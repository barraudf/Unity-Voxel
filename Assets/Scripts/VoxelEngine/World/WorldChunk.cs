using UnityEngine;
using System.Collections;
using System;

public class WorldChunk : Chunk
{
	protected World _World;

	protected override Block GetExternalBlock(int x, int y, int z)
	{
		throw new NotImplementedException();
	}
}
