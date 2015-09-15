using UnityEngine;
using System.Collections;

public class ChunkManager
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

		chunk.DeleteRequested = true;
	}

	public virtual void Build(Chunk chunk)
	{
		chunk.Busy = true;
		chunk.MeshData = MeshBuilder.BuildMeshes(chunk);
		chunk.MeshDataLoaded = true;
	}
}
