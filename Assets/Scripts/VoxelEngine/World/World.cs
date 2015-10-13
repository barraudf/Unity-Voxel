using UnityEngine;
using System.Collections.Generic;
using System.Threading;

[RequireComponent(typeof(ObjectPool))]
public class World : ChunkContainer
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

	public int MaxUnloadPerFrame = 1;
	public int MaxRenderPerFrame = 1;

	private List<WorldNavigator> _Navigators;
	private ObjectPool _Pool;

	private int _LastIndexUnloaded = 0;
	private WorldChunk[] _UnloadingList;

	protected int _MaskX, _MaskY, _MaskZ;
	protected int _LogSizeX, _LogSizeY, _LogSizeZ;
	#endregion fields

	protected override void Awake()
	{
		base.Awake();

		_Loader = new SampleChunkLoader();
		_Unloader = new SimpleUnloader();
		_MeshBuilder = new GreedyMeshBuilder();

		_Navigators = new List<WorldNavigator>();
		_Pool = GetComponent<ObjectPool>();
		

		// Ensure the prefab has required components
		if (_Pool == null)
			_Pool.Prefab.AddComponent<ObjectPool>();

		_LogSizeX = SetLogSize(ChunkSizeX);
		_LogSizeY = SetLogSize(ChunkSizeY);
		_LogSizeZ = SetLogSize(ChunkSizeZ);
		_MaskX = ChunkSizeX - 1;
		_MaskY = ChunkSizeY - 1;
		_MaskZ = ChunkSizeZ - 1;
	}

	protected void FixedUpdate()
	{
		UnloadChunksOutOfRange();

		int cpt = 0;
        for (int i = ChunkList.Count - 1; i >= 0; i--)
		{
			WorldChunk chunk = ChunkList[i];

			if(chunk.DeleteRequested && !chunk.Busy)
			{
				ChunkList.RemoveAt(i);
				Chunks.Remove(chunk.Position);
				continue;
			}

			if (chunk.MeshDataLoaded && cpt++ < MaxRenderPerFrame)
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
		WorldChunk chunk = null;

		Chunks.TryGetValue(position, out chunk);

		return chunk;
	}

	/// <summary>
	/// Create a chunk and load its block data
	/// </summary>
	/// <param name="position">The position of the chunk</param>
	/// <returns>The created chunk</returns>
	public WorldChunk LoadChunk(GridPosition position)
	{
		WorldChunk chunk = new WorldChunk(this, position);
		Chunks.Add(position, chunk);
		ChunkList.Add(chunk);

		if (!chunk.BlocksLoaded)
		{
			if (MultiThreading)
				ThreadPool.QueueUserWorkItem(c => { Load(chunk); });
			else
				Load(chunk);
		}

		return chunk;
	}

	/// <summary>
	/// Build mesh data (vertices, triangles, etc.) of the chunk
	/// </summary>
	/// <param name="chunk"></param>
	public void BuildChunk(WorldChunk chunk)
	{
		if (chunk == null)
			return;

		if (MultiThreading)
		{
			ThreadPool.QueueUserWorkItem(c =>
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

					Build(chunk);
				}
			});
		}
		else
		{
			Build(chunk);
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

		Vector3 goPosition = chunk.GetGlobalPosition();

        for (int i = 0; i < chunk.MeshData.Length; i++)
		{
			GameObject go = _Pool.NextObject();

			if (go == null)
			{
				Debug.LogError("Pool has no GameObject available");
				break;
			}
			AttachMesh(go, chunk, chunk.MeshData[i]);

			go.transform.position = goPosition;
			GOs.Add(go);
		}

		chunk.GameObjects = GOs.ToArray();
	}

	public void LoadChunkColumn(GridPosition columnPosition)
	{
		for (int y = 0; y < MaxChunkY; y++)
			for (int x = -1; x <= 1; x++)
				for (int z = -1; z <= 1; z++)
				{
					if (!ColumnNeeded(columnPosition, x, z))
						continue;

					GridPosition position = new GridPosition(x, y, z) + columnPosition;

					WorldChunk chunk = GetChunk(position);
					if (chunk == null)
						chunk = LoadChunk(position);

					if (x == 0 && z == 0)
						chunk.ColumnLoaded = true;
				}

		if (MultiThreading)
			ThreadPool.QueueUserWorkItem(c => { BuildChunkColumn(columnPosition); });
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
						if (!ColumnNeeded(columnPosition, x, z))
							continue;

						GridPosition position = new GridPosition(x, y, z) + columnPosition;

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
			ThreadPool.QueueUserWorkItem(c => { Unload(chunk); });
		else
			Unload(chunk);
	}
	
	private void UnloadChunksOutOfRange()
	{
		if (_LastIndexUnloaded == 0)
			_UnloadingList = ChunkList.ToArray();

		int cpt = 0;
		for (int i = _LastIndexUnloaded; i < _UnloadingList.Length; i++)
		{
			_LastIndexUnloaded = i;

			if (++cpt > MaxUnloadPerFrame)
			{
				return;
			}

			bool isInRange = false;
			for (int j = 0; j < _Navigators.Count; j++)
			{
				GridPosition offset = _UnloadingList[i].Position - _Navigators[j].Position;
				if (Mathf.Abs(offset.x) + Mathf.Abs(offset.z) < _Navigators[j].RenderDistance * 2f)
				{
					isInRange = true;
					break;
				}
			}

			if (!isInRange)
				UnloadChunk(_UnloadingList[i]);
		}
		_LastIndexUnloaded = 0;
		_UnloadingList = null;
    }
	#endregion Chunk management

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

	protected static int SetLogSize(int size)
	{
		int i = 0;
		while (1 << i != size)
			i++;
		return i;
	}

	/// <summary>
	/// Calculate the chunk position (relative to the current chunk) of a block of the given local coordinates
	/// </summary>
	/// <param name="x">x coordinate of the block, relative to the current chunk</param>
	/// <param name="y">y coordinate of the block, relative to the current chunk</param>
	/// <param name="z">z coordinate of the block, relative to the current chunk</param>
	/// <returns>chunk position</returns>
	public GridPosition CalculateChunkOffset(int x, int y, int z)
	{
		return new GridPosition(
			x >> _LogSizeX,
			y >> _LogSizeY,
			z >> _LogSizeZ
			);
	}

	/// <summary>
	/// Calculate the local coordinates of a block in another chunk
	/// </summary>
	/// <param name="x">remote local x coordinate of the block</param>
	/// <param name="y">remote local y coordinate of the block</param>
	/// <param name="z">remote local z coordinate of the block</param>
	/// <returns>Block position in a remote chunk</returns>
	public GridPosition CalculateBlockPosition(int x, int y, int z)
	{
		return new GridPosition(
			x & _MaskX,
			y & _MaskY,
			z & _MaskZ
			);
	}
}
