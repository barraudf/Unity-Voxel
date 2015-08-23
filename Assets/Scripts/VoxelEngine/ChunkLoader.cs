using UnityEngine;
using System.Collections;

public abstract class ChunkLoader
{
	public abstract Block[,,] LoadChunk();
}
