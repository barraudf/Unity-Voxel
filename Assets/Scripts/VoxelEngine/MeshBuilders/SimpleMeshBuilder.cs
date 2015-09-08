using UnityEngine;
using System.Collections;
using System;

public class SimpleMeshBuilder : ChunkMeshBuilder
{
	/// <summary>
	/// Build one or more mesh from a chunk
	/// </summary>
	/// <param name="chunk">chunk containing the blocks to render</param>
	/// <returns></returns>
	public override MeshData[] BuildMeshes(Chunk chunk)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		
		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					Block block = chunk.Blocks[x, y, z];
					if (block != null)
					{
						GridPosition blockPosition = new GridPosition(x, y, z);
                        ProcessFace(block, blockPosition, Direction.Up,			chunk, meshBuilder);
						ProcessFace(block, blockPosition, Direction.Down,		chunk, meshBuilder);
						ProcessFace(block, blockPosition, Direction.Right,		chunk, meshBuilder);
						ProcessFace(block, blockPosition, Direction.Left,		chunk, meshBuilder);
						ProcessFace(block, blockPosition, Direction.Forward,	chunk, meshBuilder);
						ProcessFace(block, blockPosition, Direction.Backward,	chunk, meshBuilder);
					}
				}

		return meshBuilder.BuildMesh();
	}

	private	void ProcessFace(Block block, GridPosition blockPosition, Direction direction, Chunk chunk, MeshBuilder meshBuilder)
	{
		GridPosition otherBlockPosition = blockPosition + direction.ToUnitVector();
		Block otherBlock = chunk.GetBlock(otherBlockPosition.x, otherBlockPosition.y, otherBlockPosition.z);

		if (block.IsFaceVisible(direction.Opposite(), otherBlock))
			BuildFace(blockPosition, block.GetBlockColor(), direction, chunk.MeshOrigin, meshBuilder);
	}

	private void BuildFace(GridPosition blockPosition, Color32 color, Direction direction, Vector3 chunkOrigin, MeshBuilder meshBuilder)
	{
		Vector3[] vertices = new Vector3[4];

		Vector3 finalOrigin = ((Vector3)blockPosition - chunkOrigin - BlockOrigin) * BlockScale;
		float px = finalOrigin.x;
		float py = finalOrigin.y;
		float pz = finalOrigin.z;


		/* Vertex list :
					4      8
					+------+
				  .'|    .'|		y
			  3	+---+--+' 7|		|
				|   |  |   |		|   .z
				| 2 +--+---+ 6		| .'
				| .'   | .'			+------x
			  1	+------+' 5
		*/

		switch (direction)
		{
			case Direction.Up:
				vertices[0] = new Vector3(px,				py + BlockScale,	pz);				// 3
				vertices[1] = new Vector3(px,				py + BlockScale,	pz + BlockScale);	// 4	
				vertices[2] = new Vector3(px + BlockScale,	py + BlockScale,	pz + BlockScale);	// 8
				vertices[3] = new Vector3(px + BlockScale,	py + BlockScale,	pz);				// 7
				break;
			case Direction.Down:
				vertices[0] = new Vector3(px,				py,					pz + BlockScale);	// 2
				vertices[1] = new Vector3(px,				py,					pz);				// 1
				vertices[2] = new Vector3(px + BlockScale,	py,					pz);				// 5
				vertices[3] = new Vector3(px + BlockScale,	py,					pz + BlockScale);	// 6
				break;
			case Direction.Right:
				vertices[0] = new Vector3(px + BlockScale,	py + BlockScale,	pz);				// 7
				vertices[1] = new Vector3(px + BlockScale,	py + BlockScale,	pz + BlockScale);	// 8
				vertices[2] = new Vector3(px + BlockScale,	py,					pz + BlockScale);	// 6
				vertices[3] = new Vector3(px + BlockScale,	py,					pz);				// 5
				break;
			case Direction.Left:
				vertices[0] = new Vector3(px,				py + BlockScale,	pz + BlockScale);	// 4
				vertices[1] = new Vector3(px,				py + BlockScale,	pz);				// 3
				vertices[2] = new Vector3(px,				py,					pz);				// 1
				vertices[3] = new Vector3(px,				py,					pz + BlockScale);	// 2
				break;
			case Direction.Forward:
				vertices[0] = new Vector3(px + BlockScale,	py + BlockScale,	pz + BlockScale);	// 8
				vertices[1] = new Vector3(px,				py + BlockScale,	pz + BlockScale);	// 4
				vertices[2] = new Vector3(px,				py,					pz + BlockScale);	// 2
				vertices[3] = new Vector3(px + BlockScale,	py,					pz + BlockScale);	// 6
				break;
			case Direction.Backward:
				vertices[0] = new Vector3(px,				py + BlockScale,	pz);				// 3
				vertices[1] = new Vector3(px + BlockScale,	py + BlockScale,	pz);				// 7
				vertices[2] = new Vector3(px + BlockScale,	py,					pz);				// 5
				vertices[3] = new Vector3(px,				py,					pz);				// 1
				break;
		}

		meshBuilder.AddQuad(vertices, color);
	}
}
