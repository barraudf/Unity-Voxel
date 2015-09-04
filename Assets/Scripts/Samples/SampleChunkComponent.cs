using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class SampleChunkComponent : MonoBehaviour
{
	private ObjectPool pool;
	
	private Chunk chunk;

	private ChunkManager manager;

	void Start()
	{
		pool = GetComponent<ObjectPool>();

		manager = new ChunkManager(new SampleChunkLoader(), new SimpleUnloader(), new SimpleMeshBuilder());
		chunk = new SampleChunk();
		manager.Load(chunk);
		manager.Build(chunk);

		List<GameObject> GOs = new List<GameObject>();

		for(int i = 0; i < chunk.MeshData.Length; i++)
		{
			GOs.Add(AttachMesh(chunk.MeshData[i]));
		}

		chunk.GameObjects = GOs.ToArray();
	}

	private GameObject AttachMesh(MeshData meshData)
	{
		GameObject go = pool.NextObject();

		go.SetActive(true);

		MeshFilter filter = go.GetComponent<MeshFilter>();
		MeshCollider col = go.GetComponent<MeshCollider>();

		Mesh mesh = meshData.ToMesh();
		filter.sharedMesh = mesh;
		col.sharedMesh = mesh;

		return go;
	}
}
