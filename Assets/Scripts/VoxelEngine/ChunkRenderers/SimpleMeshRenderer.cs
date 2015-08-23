using UnityEngine;
using System.Collections;
using System;

public class SimpleMeshRenderer : ChunkRenderer
{
	private MeshBuilder meshBuilder;

	/// <summary>
	/// Build one or more mesh from a chunk
	/// </summary>
	/// <param name="chunk">chunk containing the blocks to render</param>
	/// <returns></returns>
	public override Mesh[] BuildMeshes(Chunk chunk)
	{
		meshBuilder = new MeshBuilder();

		for(int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					Block block = chunk.Blocks[x, y, z];
					if (block != null)
					{
						GridPosition blockPosition = new GridPosition(x, y, z);
                        ProcessFace(block, blockPosition, Direction.Up, chunk);
						ProcessFace(block, blockPosition, Direction.Down, chunk);
						ProcessFace(block, blockPosition, Direction.Right, chunk);
						ProcessFace(block, blockPosition, Direction.Left, chunk);
						ProcessFace(block, blockPosition, Direction.Forward, chunk);
						ProcessFace(block, blockPosition, Direction.Backward, chunk);
					}
				}

		return meshBuilder.BuildMesh();
	}

	private	void ProcessFace(Block block, GridPosition blockPosition, Direction direction, Chunk chunk)
	{
		GridPosition otherBlockPosition = blockPosition + direction.ToUnitVector();
		Block otherBlock = chunk.GetBlock(otherBlockPosition.x, otherBlockPosition.y, otherBlockPosition.z);

		if (block.IsFaceVisible(direction.Opposite(), otherBlock))
			BuildFace(blockPosition, block.GetBlockColor(), direction, chunk.MeshOrigin, blockOrigin, blockScale);
	}

	private void BuildFace(GridPosition blockPosition, Color32 color, Direction direction, Vector3 chunkOrigin, Vector3 blockOrigin, float blockScale)
	{
		Vector3[] vertices = new Vector3[4];

		Vector3 finalOrigin = ((Vector3)blockPosition - chunkOrigin - blockOrigin) * blockScale;
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
				vertices[0] = new Vector3(px,				py + blockScale,	pz);				// 3
				vertices[1] = new Vector3(px,				py + blockScale,	pz + blockScale);	// 4	
				vertices[2] = new Vector3(px + blockScale,	py + blockScale,	pz + blockScale);	// 8
				vertices[3] = new Vector3(px + blockScale,	py + blockScale,	pz);				// 7
				break;
			case Direction.Down:
				vertices[0] = new Vector3(px,				py,					pz + blockScale);	// 2
				vertices[1] = new Vector3(px,				py,					pz);				// 1
				vertices[2] = new Vector3(px + blockScale,	py,					pz);				// 5
				vertices[3] = new Vector3(px + blockScale,	py,					pz + blockScale);	// 6
				break;
			case Direction.Right:
				vertices[0] = new Vector3(px + blockScale,	py + blockScale,	pz);				// 7
				vertices[1] = new Vector3(px + blockScale,	py + blockScale,	pz + blockScale);	// 8
				vertices[2] = new Vector3(px + blockScale,	py,					pz + blockScale);	// 6
				vertices[3] = new Vector3(px + blockScale,	py,					pz);				// 5
				break;
			case Direction.Left:
				vertices[0] = new Vector3(px,				py + blockScale,	pz + blockScale);	// 4
				vertices[1] = new Vector3(px,				py + blockScale,	pz);				// 3
				vertices[2] = new Vector3(px,				py,					pz);				// 1
				vertices[3] = new Vector3(px,				py,					pz + blockScale);	// 2
				break;
			case Direction.Forward:
				vertices[0] = new Vector3(px + blockScale,	py + blockScale,	pz + blockScale);	// 8
				vertices[1] = new Vector3(px,				py + blockScale,	pz + blockScale);	// 4
				vertices[2] = new Vector3(px,				py,					pz + blockScale);	// 2
				vertices[3] = new Vector3(px + blockScale,	py,					pz + blockScale);	// 6
				break;
			case Direction.Backward:
				vertices[0] = new Vector3(px,				py + blockScale,	pz); // 3
				vertices[1] = new Vector3(px + blockScale,	py + blockScale,	pz); // 7
				vertices[2] = new Vector3(px + blockScale,	py,					pz); // 5
				vertices[3] = new Vector3(px,				py,					pz); // 1
				break;
		}

		meshBuilder.AddQuad(vertices, color);
	}
}
