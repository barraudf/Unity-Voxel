using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Chunk
{
	/// <summary>
	/// Grid of blocks constituting the chunk
	/// </summary>
	public Block[,,] Blocks;

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
	public Mesh[] Meshes;

	public Chunk()
	{
		MeshOrigin = Vector3.zero;
		LoadingInProgress = false;
		BlocksLoaded = false;
		DeleteRequested = false;
		MeshDataLoaded = false;
	}

	#region States of the chunk
	/// <summary>
	/// Is the chunk currently being loaded?
	/// </summary>
	public bool LoadingInProgress;

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
	/// Is the mesh sent to a MeshFilter?
	/// </summary>
	public bool MeshRendered = false;
	#endregion States of the chunk

	private bool IsLocalCoordinate(int index, Direction direction)
	{
		bool isLocal = true;
		if (index < 0)
			isLocal = false;
		else
			switch(direction)
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
		return IsLocalCoordinateX(pos.x) && IsLocalCoordinateY(pos.y) && IsLocalCoordinateZ(pos.z);
	}
	public bool IsLocalCoordinates(int x, int y, int z)
	{
		return IsLocalCoordinateX(x) && IsLocalCoordinateY(y) && IsLocalCoordinateZ(z);
	}

	public virtual void BuildMeshes(ChunkRenderer builder)
	{
		Meshes = builder.BuildMeshes(this);
		MeshDataLoaded = true;
	}

	public virtual void LoadChunk(ChunkLoader loader)
	{
		Blocks = loader.LoadChunk();
		BlocksLoaded = true;
	}

	public Block GetBlock(int x, int y, int z)
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

	protected abstract Block GetExternalBlock(int x, int y, int z);
}
