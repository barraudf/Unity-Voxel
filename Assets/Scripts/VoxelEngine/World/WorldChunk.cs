using UnityEngine;
using System.Collections;
using System;

public class WorldChunk : Chunk
{
	public World World;

	public GridPosition Position;

	public WorldChunk(World world, GridPosition position)
		:base()
	{
		World = world;
		Position = position;
	}

	#region external block position calculation
	protected override Block GetExternalBlock(int x, int y, int z)
	{
		GridPosition chunkOffset = CalculateChunkOffset(x, y, z);
		GridPosition blockRemotePosition = CalculateBlockPosition(chunkOffset, x, y, z);
		WorldChunk chunk = World.GetChunk(Position + chunkOffset);

		if (chunk != null)
			return chunk.GetBlock(blockRemotePosition);
		else
			return null;
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
			Mathf.FloorToInt(x / (float)SizeX),
			Mathf.FloorToInt(y / (float)SizeY),
			Mathf.FloorToInt(z / (float)SizeZ)
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
		return CalculateBlockPosition(CalculateChunkOffset(x, y, z), x, y, z);
	}

	/// <summary>
	/// Calculate the local coordinates of a block in another chunk
	/// </summary>
	/// <param name="chunkOffset">the other chunk coordinates, relative to the current chunk</param>
	/// <param name="x">remote local x coordinate of the block</param>
	/// <param name="y">remote local y coordinate of the block</param>
	/// <param name="z">remote local z coordinate of the block</param>
	/// <returns>Block position in a remote chunk</returns>
	public GridPosition CalculateBlockPosition(GridPosition chunkOffset, int x, int y, int z)
	{
		return new GridPosition(
			CalcultateAxisPosition(x, SizeX, chunkOffset.x),
			CalcultateAxisPosition(y, SizeY, chunkOffset.y),
			CalcultateAxisPosition(z, SizeZ, chunkOffset.z)
			);
	}

	protected int CalcultateAxisPosition(int position, int size, int offset)
	{
		int remoteLocalPosition = position - (size * offset);

		return remoteLocalPosition;
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

	public override string ToString()
	{
		return "Chunk(" + Position.x + "," + Position.y + "," + Position.z + ")";
    }
}
