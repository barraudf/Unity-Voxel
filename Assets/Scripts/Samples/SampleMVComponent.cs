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

		MVLoader loader = new MVLoader();
		loader.Layers.Add(new MVModelLayer(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\vox\Chest1.vox"));

		Texture2D tex = new Texture2D(256, 1);
		tex.LoadImage(System.IO.File.ReadAllBytes(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\export\AlternatePalette.png"));

		ChunkMeshBuilder builder = new GreedyMeshBuilder();

		manager = new ChunkManager(loader, new SimpleUnloader(), builder);
		chunk = new MVChunk();
		manager.Load(chunk);
		chunk.LoadPalette(tex);
		manager.Build(chunk);

		List<GameObject> GOs = new List<GameObject>();

		for (int i = 0; i < chunk.MeshData.Length; i++)
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
