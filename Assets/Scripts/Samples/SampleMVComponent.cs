using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class SampleMVComponent : MonoBehaviour
{
	private ObjectPool pool;

	private MVChunk chunk;

	private ChunkLoader _Loader;
	private ChunkMeshBuilder _MeshBuilder;
	private ChunkUnloader _Unloader;

	private void Start ()
	{
		pool = GetComponent<ObjectPool>();

		MVLoader loader = new MVLoader();
		loader.Layers.Add(new MVModelLayer(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\vox\Head3.vox"));
		loader.Layers.Add(new MVAlphaLayer(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\vox\Head3_AM.vox"));

		//Texture2D tex = new Texture2D(256, 1);
		//tex.LoadImage(System.IO.File.ReadAllBytes(@"Z:\Fab\Programmation\Voxel\MagicaVoxel\export\AlternatePalette.png"));

		ChunkMeshBuilder builder = new GreedyMeshBuilder();

		_Loader = loader;
		_Unloader = new SimpleUnloader();
		_MeshBuilder = builder;

		chunk = new MVChunk();
		Load(chunk);
		//chunk.LoadPalette(tex);
		Build(chunk);

		List<GameObject> GOs = new List<GameObject>();

		for (int i = 0; i < chunk.MeshData.Length; i++)
		{
			GOs.Add(AttachMesh(chunk.MeshData[i]));
		}

		chunk.GameObjects = GOs.ToArray();
	}

	protected virtual void Load(Chunk chunk)
	{
		_Loader.LoadChunk(chunk);
		chunk.BlocksLoaded = true;
	}

	protected virtual void Build(Chunk chunk)
	{
		chunk.Busy = true;
		chunk.MeshData = _MeshBuilder.BuildMeshes(chunk);
		chunk.MeshDataLoaded = true;
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
