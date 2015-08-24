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
		chunk.Load(Loader);
	}

	public virtual void Unload(Chunk chunk)
	{
		chunk.Unload(Unloader);
	}

	public virtual void Build(Chunk chunk)
	{
		chunk.BuildMeshes(MeshBuilder);
	}
}
