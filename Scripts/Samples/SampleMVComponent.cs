using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class SampleMVComponent : MVModel
{
	private ObjectPool pool;

	private MVChunk chunk;

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

		chunk = new MVChunk(this);
		Load(chunk);
		//chunk.LoadPalette(tex);
		Build(chunk);

		List<GameObject> GOs = new List<GameObject>();

		for (int i = 0; i < chunk.MeshData.Length; i++)
		{
			GameObject go = pool.NextObject();

			if (go == null)
			{
				Debug.LogError("Pool has no GameObject available");
				break;
			}
			AttachMesh(go, chunk, chunk.MeshData[i]);

			go.transform.localPosition = Vector3.zero;
			GOs.Add(go);
		}

		chunk.GameObjects = GOs.ToArray();
	}
}
