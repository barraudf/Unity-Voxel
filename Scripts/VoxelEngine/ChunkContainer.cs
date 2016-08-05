using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public abstract class ChunkContainer : MonoBehaviour
{
	#region Fields
	/// <summary>
	/// Number of blocks in each chunks on the X axis
	/// </summary>
	public int ChunkSizeX = 1;

	/// <summary>
	/// Number of blocks in each chunks on the Y axis
	/// </summary>
	public int ChunkSizeY = 1;

	/// <summary>
	/// Number of blocks in each chunks on the Z axis
	/// </summary>
	public int ChunkSizeZ = 1;

	/// <summary>
	/// Size of each block (in Unity unit)
	/// </summary>
	public Vector3 BlockScale { get { return _BlockScale; } }

	/// <summary>
	/// Origin point of the world
	/// </summary>
	public Vector3 WorldOriginPoint = Vector3.zero;

	/// <summary>
	/// The pivot point of blocks
	/// </summary>
	public Vector3 BlockOriginPoint = Vector3.zero;

	/// <summary>
	/// The pivot point of chunks
	/// </summary>
	public Vector3 ChunkOriginPoint = Vector3.zero;

	/// <summary>
	/// Should we use multithreading for chunk operations?
	/// </summary>
	public bool MultiThreading = true;

	protected static IThreadPool ThreadPool { get { return _ThreadPool; } }

	protected ChunkLoader _Loader;
	protected ChunkMeshBuilder _MeshBuilder;
	protected ChunkUnloader _Unloader;

	private Vector3 _BlockScale;

	private static IThreadPool _ThreadPool;
	#endregion Fields

	static ChunkContainer()
	{
		_ThreadPool = new CustomThreadPool(false);
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

	protected virtual void Unload(Chunk chunk)
	{
		_Unloader.UnloadChunk(chunk);

		chunk.DeleteRequested = true;
	}

	/// <summary>
	/// Get a GameObject from the pool and attach a mesh to it
	/// </summary>
	/// <param name="mesh">Mesh to attach</param>
	protected virtual void AttachMesh(GameObject go, Chunk chunk, MeshData meshData)
	{
		if (go != null)
		{
			go.name = chunk.ToString();
			MeshFilter filter = go.GetComponent<MeshFilter>();
			if (filter == null)
				filter = go.AddComponent<MeshFilter>();
			MeshCollider col = go.GetComponent<MeshCollider>();
			if (col == null)
				col = go.AddComponent<MeshCollider>();
			ChunkMesh chunkMesh = go.GetComponent<ChunkMesh>();
			if (chunkMesh == null)
				chunkMesh = go.AddComponent<ChunkMesh>();

			Mesh mesh = meshData.ToMesh();
			filter.sharedMesh = mesh;
			col.sharedMesh = mesh;
			chunkMesh.Chunk = chunk;
		}
	}

	protected virtual void Awake()
	{
		_BlockScale = transform.localScale;
    }
}
