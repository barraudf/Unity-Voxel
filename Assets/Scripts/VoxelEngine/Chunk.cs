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
	public int SizeX { get { return Blocks.GetLength(0); } }

	/// <summary>
	/// Number of rows on the Y axis
	/// </summary>
	public int SizeY { get { return Blocks.GetLength(1); } }

	/// <summary>
	/// Number of columns on the Z axis
	/// </summary>
	public int SizeZ { get { return Blocks.GetLength(2); } }

	/// <summary>
	/// The pivot point of the mesh
	/// </summary>
	public Vector3 MeshOrigin;

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
	public float BlockScale;

	public Chunk()
	{
		MeshOrigin = Vector3.zero;
		ColumnLoaded = false;
		BlocksLoaded = false;
		DeleteRequested = false;
		MeshDataLoaded = false;
		Busy = false;
		BlockScale = 1f;
	}

	public void InitBlocks(int sizeX, int sizeY, int sizeZ)
	{
		Blocks = new Block[sizeX, sizeY, sizeZ];
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

	private bool IsLocalCoordinate(int index, Direction direction)
	{
		bool isLocal = true;
		if (index < 0)
			isLocal = false;
		else
			switch (direction)
			{
				case Direction.Right:
				case Direction.Left:
					isLocal = index < SizeX;
					break;
				case Direction.Up:
				case Direction.Down:
					isLocal = index < SizeY;
					break;
				case Direction.Forward:
				case Direction.Backward:
					isLocal = index < SizeZ;
					break;
			}

		return isLocal;
	}

	private bool IsLocalCoordinateX(int index) { return IsLocalCoordinate(index, Direction.Left); }
	private bool IsLocalCoordinateY(int index) { return IsLocalCoordinate(index, Direction.Up); }
	private bool IsLocalCoordinateZ(int index) { return IsLocalCoordinate(index, Direction.Forward); }
	public bool IsLocalCoordinates(GridPosition pos)
	{
		return IsLocalCoordinates(pos.x, pos.y, pos.z);
	}
	public bool IsLocalCoordinates(int x, int y, int z)
	{
		return IsLocalCoordinateX(x) && IsLocalCoordinateY(y) && IsLocalCoordinateZ(z);
	}

	#region get / set blocks
	public virtual Block GetBlock(int x, int y, int z)
	{
		if (IsLocalCoordinates(x, y, z))
			return Blocks[x, y, z];
		else
			return GetExternalBlock(x, y, z);
	}

	public Block GetBlock(GridPosition position)
	{
		return GetBlock(position.x, position.y, position.z);
	}

	public virtual void SetBlock(int x, int y, int z, Block block)
	{
		if (IsLocalCoordinates(x, y, z))
			Blocks[x, y, z] = block;
		else
			SetExternalBlock(x, y, z, block);
	}

	public void SetBlock(GridPosition position, Block block)
	{
		SetBlock(position.x, position.y, position.z, block);
	}

	protected abstract Block GetExternalBlock(int x, int y, int z);
	protected abstract void SetExternalBlock(int x, int y, int z, Block block);
	#endregion get / set blocks
}
