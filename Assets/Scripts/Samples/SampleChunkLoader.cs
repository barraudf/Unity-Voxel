using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SampleChunkLoader : MonoBehaviour
{
	protected MeshFilter Filter;

	void Start ()
	{
		Filter = gameObject.GetComponent<MeshFilter>();

		Chunk chunk = new SampleChunk();
		chunk.MeshOrigin = Vector3.one * 1.5f;

		chunk.Blocks = new Block[16, 256, 16];
		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
					if( (x+y+z) % 2 == 0)
						chunk.Blocks[x, y, z] = new SampleBlock();

		ChunkRenderer renderer = new SimpleMeshRenderer();
		renderer.blockOrigin = Vector3.zero;
		renderer.blockScale = 1f;
		chunk.BuildMeshes(renderer);

		Debug.LogFormat("chunk modélisé : {0} meshes", chunk.Meshes.Length);

		Filter.mesh = chunk.Meshes[0];
    }
}
