using UnityEngine;
using System.Collections;

public abstract class ChunkManager
{
	public ChunkLoader Loader;
	public ChunkMeshBuilder MeshBuilder;
	public ChunkUnloader Unloader;

	public ChunkManager(ChunkLoader loader, ChunkUnloader unloader, ChunkMeshBuilder meshBuilder)
	{
		Loader = loader;
		Unloader = unloader;
		MeshBuilder = meshBuilder;
	}

	public virtual void Load(Chunk chunk)
	{
		Loader.LoadChunk(chunk);
		chunk.BlocksLoaded = true;
	}

	public virtual void Unload(Chunk chunk)
	{
		Unloader.UnloadChunk(chunk);

		if (chunk.GameObjects == null)
			return;

		for (int i = 0; i < chunk.GameObjects.Length; i++)
			chunk.GameObjects[i].SetActive(false);
		chunk.GameObjects = null;
	}

	public virtual void Build(Chunk chunk)
	{
		chunk.Meshes = MeshBuilder.BuildMeshes(chunk);
		chunk.MeshRendered = true;
	}
}
