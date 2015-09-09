﻿using UnityEngine;
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

		manager = new ChunkManager(loader, new SimpleUnloader(), builder);
		chunk = new MVChunk();
		manager.Load(chunk);
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