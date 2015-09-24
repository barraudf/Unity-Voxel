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
}
