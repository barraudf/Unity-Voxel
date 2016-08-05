using UnityEngine;
using System.Collections;

public abstract class ChunkMeshBuilder
{
	/// <summary>
	/// Build one or more mesh from a chunk
	/// </summary>
	/// <param name="chunk">chunk containing the blocks to render</param>
	/// <param name="blockOrigin">coordinates of the pivot point (expressed in grid unit, so new Vector3(0.5f,0.5f,0.5f) would be the center of the block)</param>
	/// <param name="blockScale">Scale of the blocks. Default is 1, which is equal to 1 unity unit.</param>
	/// <returns></returns>
	public abstract MeshData[] BuildMeshes(Chunk chunk);
}
