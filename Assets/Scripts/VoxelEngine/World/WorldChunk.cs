using UnityEngine;
using System.Collections;
using System;

public class WorldChunk : Chunk
{
	public World World {  get { return (World)_Container; } }

	public GridPosition Position;

	public WorldChunk(World world, GridPosition position)
		:base(world)
	{
		Position = position;
	}

	public override void InitBlocks(int sizeX, int sizeY, int sizeZ)
	{
		if ((sizeX & (sizeX - 1)) != 0 || (sizeY & (sizeY - 1)) != 0 || (sizeZ & (sizeZ - 1)) != 0)
		{
			Debug.LogErrorFormat("Invalid chunk size ({0},{1},{2}). Size have to be a power of 2", sizeX, sizeY, sizeZ);
			return;
		}

		base.InitBlocks(sizeX, sizeY, sizeZ);
	}

	#region external block position calculation
	public override void SetBlock(RaycastHit hit, Block block, bool adjacent = false)
	{
		GridPosition pos = GetBlockPosition(hit, adjacent);
		pos -= Position * new GridPosition(SizeX, SizeY, SizeZ);
		SetBlock(pos.x, pos.y, pos.z, block);
	}

	protected override Block GetExternalBlock(int x, int y, int z)
	{
		GridPosition chunkOffset = World.CalculateChunkOffset(x, y, z);
		GridPosition blockRemotePosition = World.CalculateBlockPosition(x, y, z);
		WorldChunk chunk = World.GetChunk(Position + chunkOffset);

		if (chunk != null)
			return chunk.GetBlock(blockRemotePosition.x, blockRemotePosition.y, blockRemotePosition.z);
		else
			return null;
	}

	protected override void SetExternalBlock(int x, int y, int z, Block block)
	{
		GridPosition chunkOffset = World.CalculateChunkOffset(x, y, z);
		GridPosition blockRemotePosition = World.CalculateBlockPosition(x, y, z);
		WorldChunk chunk = World.GetChunk(Position + chunkOffset);

		if (chunk != null)
			chunk.SetBlock(blockRemotePosition.x, blockRemotePosition.y, blockRemotePosition.z, block);
	}
	#endregion external block position calculation

	/// <summary>
	/// Get the global position of the chunk 
	/// </summary>
	/// <returns></returns>
	public Vector3 GetGlobalPosition()
	{
		return Vector3.Scale(new Vector3(
			Position.x * World.ChunkSizeX,
			Position.y * World.ChunkSizeY,
			Position.z * World.ChunkSizeZ
			), World.BlockScale) - World.WorldOriginPoint;
	}

	protected override Vector3 GetLocalPosition(Vector3 globalPosition)
	{
		return base.GetLocalPosition(globalPosition) + World.WorldOriginPoint;
	}

	#region hitbox
	public override bool GetHitBox(RaycastHit hit, out GridPosition position, out Vector3 size)
	{
		GridPosition blockPosition = GetBlockPosition(hit);
		blockPosition -= Position * new GridPosition(SizeX, SizeY, SizeZ);

		if (GetBlock(blockPosition.x, blockPosition.y, blockPosition.z) != null)
		{
			size = BlockScale;
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
		{
			World.BuildChunk(this);

			if (x == 0)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Left));
			if (x == SizeX - 1)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Right));
			if (y == 0)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Down));
			if (y == SizeY - 1)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Up));
			if (z == 0)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Backward));
			if (z == SizeZ - 1)
				World.BuildChunk(World.GetChunk(Position + GridPosition.Forward));
		}
	}

	public override string ToString()
	{
		return string.Format("Chunk({0},{1},{2})", Position.x, Position.y, Position.z);
	}
}
