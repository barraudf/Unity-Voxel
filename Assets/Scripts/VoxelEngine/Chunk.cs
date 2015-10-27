using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Chunk
{
	/// <summary>
	/// Grid of blocks constituting the chunk
	/// </summary>
	protected Block[,,] Blocks;

	/// <summary>
	/// Number of columns on the X axis
	/// </summary>
	public int SizeX;

	/// <summary>
	/// Number of rows on the Y axis
	/// </summary>
	public int SizeY;

	/// <summary>
	/// Number of columns on the Z axis
	/// </summary>
	public int SizeZ;

	/// <summary>
	/// The pivot point of the mesh
	/// </summary>
	public Vector3 ChunkOriginPoint {  get { return _Container.ChunkOriginPoint; } }

	/// <summary>
	/// coordinates of the pivot point
	/// </summary>
	public Vector3 BlockOriginPoint { get { return _Container.BlockOriginPoint; } }

	/// <summary>
	/// One or more mesh to hold the chunk model
	/// </summary>
	public MeshData[] MeshData;

	/// <summary>
	/// One or more GameObjects to which meshes are attached to
	/// </summary>
	public GameObject[] GameObjects;

	/// <summary>
	/// Scale of the blocks. Default is 1, which is equal to 1 unity unit.
	/// </summary>
	public Vector3 BlockScale { get { return _Container.BlockScale; } }

	protected ChunkContainer _Container;

	public Chunk(ChunkContainer container)
	{
		_Container = container;
		ColumnLoaded = false;
		BlocksLoaded = false;
		DeleteRequested = false;
		MeshDataLoaded = false;
		Busy = false;
	}

	public virtual void InitBlocks(int sizeX, int sizeY, int sizeZ)
	{
		Blocks = new Block[sizeX, sizeY, sizeZ];
		SizeX = sizeX;
		SizeY = sizeY;
		SizeZ = sizeZ;
		
	}

	#region States of the chunk
	/// <summary>
	/// Has the chunk's column been loaded?
	/// </summary>
	public bool ColumnLoaded;

	/// <summary>
	/// Are blocks loaded into memory?
	/// </summary>
	public bool BlocksLoaded;

	/// <summary>
	/// Did somebody requested chunk deletion? this will occur during the next update
	/// </summary>
	public bool DeleteRequested;

	/// <summary>
	/// Are mesh data (vertices, triangles, etc...) loaded?
	/// </summary>
	public bool MeshDataLoaded;

	/// <summary>
	/// Are the mesh data being loaded?
	/// </summary>
	public bool Busy;

	/// <summary>
	/// Has a mesh building already been requested since the last build?
	/// </summary>
	public bool UpdatePending;
	#endregion States of the chunk

	public bool IsLocalCoordinates(int x, int y, int z)
	{
		return x >= 0 && x < SizeX
			&& y >= 0 && y < SizeY
			&& z >= 0 && z < SizeZ;
	}

	#region get / set blocks
	public virtual Block GetBlock(int x, int y, int z)
	{
		if (IsLocalCoordinates(x, y, z))
			return Blocks[x, y, z];
		else
			return new Block { Type = Block.BlockTypes.Air };
	}

	public virtual void SetBlock(int x, int y, int z, Block block)
	{
		if (IsLocalCoordinates(x, y, z))
			Blocks[x, y, z] = block;
		else
			Debug.LogErrorFormat("Can't set block({0},{1},{2}) because it's outside of the chunk", x, y, z);
	}

	public virtual void SetBlock(RaycastHit hit, Block block, bool adjacent = false)
	{
		GridPosition pos = GetBlockPosition(hit, adjacent);
		SetBlock(pos.x, pos.y, pos.z, block);
	}
	#endregion get / set blocks

	#region chunk operations
	public void Merge(Chunk otherChunk, GridPosition position, Quaternion rotation, bool ConsiderEmptyBlocks = false)
	{
		for (int x = 0; x < otherChunk.SizeX; x++)
			for (int y = 0; y < otherChunk.SizeY; y++)
				for (int z = 0; z < otherChunk.SizeZ; z++)
				{
					Vector3 vect = new Vector3(x, y, z) - otherChunk.ChunkOriginPoint;
					vect = rotation * vect;
					GridPosition blockPosition = position + new GridPosition( Mathf.RoundToInt(vect.x), Mathf.RoundToInt(vect.y), Mathf.RoundToInt(vect.z) );

					Block block = otherChunk.GetBlock(x,y,z);
					if(block.Type != Block.BlockTypes.Air || ConsiderEmptyBlocks)
						SetBlock(blockPosition.x, blockPosition.y, blockPosition.z, block);
				}
    }
	#endregion chunk operations

	#region hitbox
	public virtual bool GetHitBox(RaycastHit hit, out GridPosition position, out Vector3 size)
	{
		size = Vector3.zero;
		position = GridPosition.Zero;
		return false;
	}

	protected static float MoveWithinBlock(float pos, float norm, float scale, bool adjacent = false)
	{
		if (pos  % scale == 0)
		{
			if ((norm < 0 && adjacent) || (norm > 0 && !adjacent))
				pos -= scale;
		}

		return (float)pos;
	}

	public GridPosition GetBlockPosition(RaycastHit hit, bool adjacent = false)
	{
		Vector3 localHitPos = GetLocalPosition(hit.point);
		Vector3 pos = new Vector3(
			MoveWithinBlock(localHitPos.x, hit.normal.x, BlockScale.x, adjacent),
			MoveWithinBlock(localHitPos.y, hit.normal.y, BlockScale.y, adjacent),
			MoveWithinBlock(localHitPos.z, hit.normal.z, BlockScale.z, adjacent)
			);

		return new GridPosition(Mathf.FloorToInt(pos.x / BlockScale.x), Mathf.FloorToInt(pos.y / BlockScale.y), Mathf.FloorToInt(pos.z / BlockScale.z));
	}

	protected virtual Vector3 GetLocalPosition(Vector3 globalPosition)
	{
		return globalPosition + ChunkOriginPoint + BlockOriginPoint;
	}
	#endregion hitbox
}
