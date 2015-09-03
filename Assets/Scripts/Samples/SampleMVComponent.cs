using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class SampleMVComponent : MonoBehaviour
{
	private ObjectPool pool;

	private MVChunk chunk;

	private ChunkManager manager;

	private void Start ()
	{
		pool = GetComponent<ObjectPool>();

		VoxLoader loader = new VoxLoader();
		loader.Layers.Add(new MVModelLayer(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\vox\Chest1.vox"));

		ChunkMeshBuilder builder = new SimpleMeshBuilder();
		builder.BlockScale = 0.1f;

		manager = new ChunkManager(loader, new SimpleUnloader(), builder);
		chunk = new MVChunk();
		manager.Load(chunk);
		manager.Build(chunk);

		List<GameObject> GOs = new List<GameObject>();

		for (int i = 0; i < chunk.Meshes.Length; i++)
		{
			GOs.Add(AttachMesh(chunk.Meshes[i]));
		}

		chunk.GameObjects = GOs.ToArray();
	}

	private GameObject AttachMesh(Mesh mesh)
	{
		GameObject go = pool.NextObject();

		go.SetActive(true);

		MeshFilter filter = go.GetComponent<MeshFilter>();
		MeshCollider col = go.GetComponent<MeshCollider>();

		filter.sharedMesh = mesh;
		col.sharedMesh = mesh;

		return go;
	}
}
