using UnityEngine;
using System.Collections;
using System;

public class WorldChunk : Chunk
{
	public World World;

	public GridPosition Position;

	protected int _MaskX, _MaskY, _MaskZ;
	protected int _LogSizeX, _LogSizeY, _LogSizeZ;

	public WorldChunk(World world, GridPosition position)
		:base()
	{
		World = world;
		Position = position;
	}

	public override void InitBlocks(int sizeX, int sizeY, int sizeZ)
	{
		if ((sizeX & (sizeX - 1)) != 0 || (sizeY & (sizeY - 1)) != 0 || (sizeZ & (sizeZ - 1)) != 0)
		{
			Debug.LogErrorFormat("Invalid chunk size ({0},{1},{2}). Size have to be a power of 2", sizeX, sizeY, sizeZ);
			return;
		}

		_LogSizeX = SetLogSize(sizeX);
		_LogSizeY = SetLogSize(sizeY);
		_LogSizeZ = SetLogSize(sizeZ);
		_MaskX = sizeX - 1;
		_MaskY = sizeY - 1;
		_MaskZ = sizeZ - 1;

		base.InitBlocks(sizeX, sizeY, sizeZ);
	}

	#region external block position calculation
	public override void SetBlock(RaycastHit hit, Block block, bool adjacent = false)
	{
		GridPosition pos = GetBlockPosition(hit, adjacent);
		pos -= Position * new GridPosition(SizeX, SizeY, SizeZ);
		SetBlock(pos, block);
	}

	protected override Block GetExternalBlock(int x, int y, int z)
	{
		GridPosition chunkOffset = CalculateChunkOffset(x, y, z);
		GridPosition blockRemotePosition = CalculateBlockPosition(x, y, z);
		WorldChunk chunk = World.GetChunk(Position + chunkOffset);

		if (chunk != null)
			return chunk.GetBlock(blockRemotePosition);
		else
			return null;
	}

	protected override void SetExternalBlock(int x, int y, int z, Block block)
	{
		GridPosition chunkOffset = CalculateChunkOffset(x, y, z);
		GridPosition blockRemotePosition = CalculateBlockPosition(x, y, z);
		WorldChunk chunk = World.GetChunk(Position + chunkOffset);

		if (chunk != null)
			chunk.SetBlock(blockRemotePosition, block);
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
	public GridPosition CalculateBlockPosition( int x, int y, int z)
	{
		return new GridPosition(
			x & _MaskX,
			y & _MaskY,
			z & _MaskZ
			);
	}
	#endregion external block position calculation

	/// <summary>
	/// Get the global position of the chunk 
	/// </summary>
	/// <returns></returns>
	public Vector3 GetGlobalPosition()
	{
		return new Vector3(
			(Position.x * World.ChunkSizeX * World.BlockScale) - World.WorldOrigin.x,
			(Position.y * World.ChunkSizeY * World.BlockScale) - World.WorldOrigin.y,
			(Position.z * World.ChunkSizeZ * World.BlockScale) - World.WorldOrigin.z
			);
	}

	protected override Vector3 GetLocalPosition(Vector3 globalPosition)
	{
		return base.GetLocalPosition(globalPosition) + World.WorldOrigin;
	}

	#region hitbox
	public override bool GetHitBox(RaycastHit hit, out GridPosition position, out Vector3 size)
	{
		GridPosition blockPosition = GetBlockPosition(hit);
		blockPosition -= Position * new GridPosition(SizeX, SizeY, SizeZ);

		if (GetBlock(blockPosition) != null)
		{
			size = new Vector3(BlockScale, BlockScale, BlockScale);
			position = blockPosition;
			return true;
		}
		else
			return base.GetHitBox(hit, out position, out size);
	}
	#endregion hitbox

	public override void SetBlock(int x, int y, int z, Block block)
	{
		SetBlock(x, y, z, block, true);
	}

	public void SetBlock(int x, int y, int z, Block block, bool rebuildMesh)
	{
		base.SetBlock(x, y, z, block);

		if (IsLocalCoordinates(x, y, z) && rebuildMesh)
			World.BuildChunk(this);
	}

	public override string ToString()
	{
		return string.Format("Chunk({0},{1},{2})", Position.x, Position.y, Position.z);
	}

	protected static int SetLogSize(int size)
	{
		int i = 0;
		while (1 << i != size)
			i++;
		return i;
	}
}
