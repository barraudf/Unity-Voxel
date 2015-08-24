using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SampleChunkComponent : MonoBehaviour
{
	protected MeshFilter Filter;
	protected MeshCollider Collider;

	Chunk chunk;

	void Start()
	{
		Filter = GetComponent<MeshFilter>();
		Collider = GetComponent<MeshCollider>();

		ChunkManager manager = new SampleChunkManager(new SampleChunkLoader(), new SimpleUnloader(), new SimpleMeshBuilder());
		chunk = new SampleChunk();
		manager.Load(chunk);
		manager.Build(chunk);

		Filter.sharedMesh = chunk.Meshes[0];
		Collider.sharedMesh = chunk.Meshes[0];
	}
}
