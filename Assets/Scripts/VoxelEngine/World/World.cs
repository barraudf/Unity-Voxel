using UnityEngine;
using System.Collections.Generic;
using System.Threading;

[RequireComponent(typeof(ObjectPool))]
public class World : MonoBehaviour
{
	#region fields
	/// <summary>
	/// Chunks loaded into memory, optimized for direct access by position
	/// </summary>
	public Dictionary<GridPosition, WorldChunk> Chunks = new Dictionary<GridPosition, WorldChunk>();

	/// <summary>
	/// Chunks loaded into memory, optimized for enumeration
	/// </summary>
	private List<WorldChunk> ChunkList = new List<WorldChunk>();

	/// <summary>
	/// Number of blocks in each chunks on the X axis
	/// </summary>
	public int ChunkSizeX = 16;

	/// <summary>
	/// Number of blocks in each chunks on the Y axis
	/// </summary>
	public int ChunkSizeY = 64;

	/// <summary>
	/// Number of blocks in each chunks on the Z axis
	/// </summary>
	public int ChunkSizeZ = 16;

	/// <summary>
	/// Size of each block (in Unity unit)
	/// </summary>
	public float BlockScale = 1f;

	/// <summary>
	/// Maximum number of chunk on the X axis (0 means unlimited)
	/// </summary>
	public int MaxChunkX = 0;

	/// <summary>
	/// Maximum number of chunk on the Y axis (0 means unlimited)
	/// </summary>
	public int MaxChunkY = 1;

	/// <summary>
	/// Maximum number of chunk on the Z axis (0 means unlimited)
	/// </summary>
	public int MaxChunkZ = 0;

	/// <summary>
	/// Origin point of the world
	/// </summary>
	public Vector3 WorldOrigin = Vector3.zero;

	/// <summary>
	/// coordinates of the pivot point (expressed in grid unit, so new Vector3(0.5f,0.5f,0.5f) would be the center of the block)
	/// </summary>
	public Vector3 BlockOrigin = Vector3.zero;

	/// <summary>
	/// The pivot point of the chunk (expressed in grid unit)
	/// </summary>
	public Vector3 ChunkOrigin = Vector3.zero;

	/// <summary>
	/// Should we use multithreading for chunk operations?
	/// </summary>
	public bool MultiThreading = true;

	public int MaxUnloadPerFrame = 1;

	private List<WorldNavigator> _Navigators;
	private ObjectPool _Pool;
	private ChunkManager _Manager;

	private int _LastIndexUnloaded = 0;
	#endregion fields

	private void Awake()
	{
		_Navigators = new List<WorldNavigator>();
		_Pool = GetComponent<ObjectPool>();
		_Manager = new ChunkManager(new SampleChunkLoader(), new SimpleUnloader(), new GreedyMeshBuilder());

		// Ensure the prefab has required components
		if (_Pool.Prefab.GetComponent<MeshFilter>() == null)
			_Pool.Prefab.AddComponent<MeshFilter>();
		if (_Pool.Prefab.GetComponent<MeshCollider>() == null)
			_Pool.Prefab.AddComponent<MeshCollider>();
	}

	private void Update()
	{
		UnloadChunksOutOfRange();

        for (int i = ChunkList.Count - 1; i >= 0; i--)
		{
			WorldChunk chunk = ChunkList[i];

			if(chunk.DeleteRequested && !chunk.Busy)
			{
				ChunkList.RemoveAt(i);
				Chunks.Remove(chunk.Position);
				continue;
			}

			if (chunk.MeshDataLoaded)
			{
				chunk.MeshDataLoaded = false;
				RenderChunk(chunk);
				chunk.MeshData = null;
				chunk.Busy = false;
			}
		}
	}

	#region Chunk management
	/// <summary>
	/// Get the chunk at the given position if it has already been loaded
	/// </summary>
	/// <param name="position">The position of the chunk</param>
	/// <returns>The chunk if it is loaded or null otherwise</returns>
	public WorldChunk GetChunk(GridPosition position)
	{
		if (Chunks.ContainsKey(position))
			return Chunks[position];
		else
			return null;
	}

	/// <summary>
	/// Create a chunk and load its block data
	/// </summary>
	/// <param name="position">The position of the chunk</param>
	/// <returns>The created chunk</returns>
	public WorldChunk LoadChunk(GridPosition position)
	{
		WorldChunk chunk = new WorldChunk(this, position);
		chunk.BlockOrigin = BlockOrigin;
		chunk.ChunkOrigin = ChunkOrigin;
		chunk.BlockScale = BlockScale;
		Chunks.Add(position, chunk);
		ChunkList.Add(chunk);

		if (!chunk.BlocksLoaded)
		{
			if (MultiThreading)
				new Thread(() => { _Manager.Load(chunk); }).Start();
			else
				_Manager.Load(chunk);
		}

		return chunk;
	}

	/// <summary>
	/// Build mesh data (vertices, triangles, etc.) of the chunk
	/// </summary>
	/// <param name="chunk"></param>
	public void BuildChunk(WorldChunk chunk)
	{
		if (MultiThreading)
		{
			new Thread(() =>
			{
				if (!chunk.UpdatePending)
				{
					if (chunk.Busy)
					{
						chunk.UpdatePending = true;
						while (chunk.Busy)
						{
							Thread.Sleep(0);
						}
						chunk.UpdatePending = false;
					}

					_Manager.Build(chunk);
				}
			}).Start();
		}
		else
		{
			_Manager.Build(chunk);
		}
	}

	/// <summary>
	/// Attach each mesh of the chunk to a GameObject
	/// </summary>
	/// <param name="chunk">the chunk</param>
	public void RenderChunk(WorldChunk chunk)
	{
		if(chunk.GameObjects != null && chunk.GameObjects.Length != 0)
		{
			for (int i = 0; i < chunk.GameObjects.Length; i++)
				chunk.GameObjects[i].SetActive(false);
		}


		List<GameObject> GOs = new List<GameObject>();

		for (int i = 0; i < chunk.MeshData.Length; i++)
		{
			GameObject go = AttachMesh(chunk.MeshData[i]);
			if (go == null)
				break;
			go.transform.position = chunk.GetGlobalPosition();
			GOs.Add(go);
		}

		chunk.GameObjects = GOs.ToArray();
	}

	public void LoadChunkColumn(int colX, int colZ)
	{
		GridPosition columnPosition = new GridPosition(colX, 0, colZ);
		for (int y = 0; y < MaxChunkY; y++)
			for (int x = -1; x <= 1; x++)
				for (int z = -1; z <= 1; z++)
				{
					// Don't process diagonals
					if (x != 0 && z != 0)
						continue;

					GridPosition position = new GridPosition(x, y, z) + columnPosition;
					// Check if position is inside the world
					if (MaxChunkX != 0 && (position.x < 0 || position.x >= MaxChunkX) ||
						MaxChunkZ != 0 && (position.z < 0 || position.z >= MaxChunkZ))
						continue;

					WorldChunk chunk = GetChunk(position);
					if (chunk == null)
						chunk = LoadChunk(position);

					if (x == 0 && z == 0)
						chunk.ColumnLoaded = true;
				}

		if (MultiThreading)
			new Thread(() => { BuildChunkColumn(columnPosition); }).Start();
		else
			BuildChunkColumn(columnPosition);
	}

	private void BuildChunkColumn(GridPosition columnPosition)
	{
		WorldChunk chunk;

		if (MultiThreading)
		{
			for (int y = 0; y < MaxChunkY; y++)
				for (int x = -1; x <= 1; x++)
					for (int z = -1; z <= 1; z++)
					{
						GridPosition position = new GridPosition(x, y, z) + columnPosition;

						if (!ColumnNeeded(columnPosition, x, z))
							continue;						

						chunk = GetChunk(position);
						while (!chunk.BlocksLoaded)
							Thread.Sleep(0);
					}
		}

		for (int y = 0; y < MaxChunkY; y++)
		{
			chunk = GetChunk(new GridPosition(columnPosition.x, y, columnPosition.z));

			if (chunk != null)
				BuildChunk(chunk);
		}
	}

	private bool ColumnNeeded(GridPosition columnPosition, int x, int z)
	{
		// Don't process diagonals
		if (x != 0 && z != 0)
			return false;

		// Check if position is inside the world
		if (MaxChunkX != 0 && (columnPosition.x + x < 0 || columnPosition.x + x >= MaxChunkX) ||
			MaxChunkZ != 0 && (columnPosition.z + z < 0 || columnPosition.z + z >= MaxChunkZ))
			return false;

		return true;
	}

	private void UnloadChunk(WorldChunk chunk)
	{
		if (chunk.GameObjects != null)
		{
			for (int i = 0; i < chunk.GameObjects.Length; i++)
				chunk.GameObjects[i].SetActive(false);
			chunk.GameObjects = null;
		}

		if (MultiThreading)
			new Thread(() => { _Manager.Unload(chunk); }).Start();
		else
			_Manager.Unload(chunk);
	}

	private void UnloadChunksOutOfRange()
	{
		int cpt = 0;
		for (int i = _LastIndexUnloaded; i < ChunkList.Count; i++)
		{
			_LastIndexUnloaded = i;

			if (++cpt > MaxUnloadPerFrame)
			{
				return;
			}

			bool isInRange = false;
			for (int j = 0; j < _Navigators.Count; j++)
			{
				GridPosition offset = ChunkList[i].Position - _Navigators[j].Position;
				if (Mathf.Abs(offset.x) + Mathf.Abs(offset.z) < _Navigators[j].RenderDistance * 2f)
				{
					isInRange = true;
					break;
				}
			}

			if (!isInRange)
				UnloadChunk(ChunkList[i]);
		}
		_LastIndexUnloaded = 0;
	}
	#endregion Chunk management

	/// <summary>
	/// Get a GameObject from the pool and attach a mesh to it
	/// </summary>
	/// <param name="mesh">Mesh to attach</param>
	/// <returns>The "instantiated" GameObject</returns>
	private GameObject AttachMesh(MeshData meshData)
	{
		GameObject go = _Pool.NextObject();

		if (go != null)
		{
			go.SetActive(true);

			MeshFilter filter = go.GetComponent<MeshFilter>();
			MeshCollider col = go.GetComponent<MeshCollider>();

			Mesh mesh = meshData.ToMesh();
			filter.sharedMesh = mesh;
			col.sharedMesh = mesh;
		}
		else
		{
			Debug.LogError("Pool has no GameObject available");
		}

		return go;
	}

	#region navigators
	public void RegisterNavigator(WorldNavigator navigator)
	{
		if (!_Navigators.Contains(navigator))
			_Navigators.Add(navigator);
	}

	public void UnregisterNavigator(WorldNavigator navigator)
	{
		if (_Navigators.Contains(navigator))
			_Navigators.Remove(navigator);
	}
	#endregion navigators
}
