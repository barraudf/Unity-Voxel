using UnityEngine;
using System.Collections;
using System;

public class GreedyMeshBuilder : ChunkMeshBuilder
{
    public override MeshData[] BuildMeshes(Chunk chunk)
	{
		Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
		bool[,,,] _Faces = new bool[directions.Length, chunk.SizeX, chunk.SizeY, chunk.SizeZ];
		MeshBuilder meshBuilder = new MeshBuilder();
		Vector3 blockScale = chunk.BlockScale;

		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					Block block = chunk.GetBlock(x, y, z);
					GridPosition blockPosition = new GridPosition(x, y, z);

					for (int d = 0; d < directions.Length; d++)
						_Faces[d, x, y, z] = FaceVisible(block, blockPosition, directions[d], chunk);
				}

		for (int d = 0; d < directions.Length; d++)
		{
			Direction dir = directions[d];
			for (int x = 0; x < chunk.SizeX; x++)
				for (int y = 0; y < chunk.SizeY; y++)
					for (int z = 0; z < chunk.SizeZ; z++)
					{

						if(_Faces[d, x, y, z])
						{
							Block block = chunk.GetBlock(x, y, z);
                            Vector3 blockPosition = new Vector3(x, y, z);
							Vector3 finalOrigin = Vector3.Scale(blockPosition, blockScale) - chunk.ChunkOriginPoint - chunk.BlockOriginPoint;
							float px = finalOrigin.x;
							float py = finalOrigin.y;
							float pz = finalOrigin.z;

							int rx = x, ry = y, rz = z;
							switch (dir)
							{
								case Direction.Right:
								case Direction.Left:
									{
										ry = y + 1;
										while (ry < chunk.SizeY && _Faces[d, x, ry, z] && SameColor(block, chunk.GetBlock(x, ry, z)))
											ry++;
										ry--;

										rz = z + 1;
										while (rz < chunk.SizeZ)
										{
											bool inc = true;
											for (int k = y; k <= ry; ++k)
											{
												inc = inc & (_Faces[d, x, k, rz] && SameColor(block, chunk.GetBlock(x, k, rz)));
											}

											if (inc)
												rz++;
											else
												break;
										}
										rz--;
										break;
									}

								case Direction.Up:
								case Direction.Down:
									{
										rx = x + 1;
										while (rx < chunk.SizeX && _Faces[d, rx, y, z] && SameColor(block, chunk.GetBlock(rx, y, z)))
											rx++;
										rx--;

										rz = z + 1;
										while (rz < chunk.SizeZ)
										{
											bool inc = true;
											for (int k = x; k <= rx; ++k)
											{
												inc = inc & (_Faces[d, k, y, rz] && SameColor(block, chunk.GetBlock(k, y, rz)));
											}

											if (inc)
												rz++;
											else
												break;
										}
										rz--;
										break;
									}

								case Direction.Forward:
								case Direction.Backward:
									{
										rx = x + 1;
										while (rx < chunk.SizeX && _Faces[d, rx, y, z] && SameColor(block, chunk.GetBlock(rx, y, z)))
											rx++;
										rx--;

										ry = y + 1;
										while (ry < chunk.SizeY)
										{
											bool inc = true;
											for (int k = x; k <= rx; ++k)
											{
												inc = inc & (_Faces[d, k, ry, z] && SameColor(block, chunk.GetBlock(k, ry, z)));
											}

											if (inc)
												ry++;
											else
												break;
										}
										ry--;
										break;
									}
							}


							for (int kx = x; kx <= rx; kx++)
							{
								for (int ky = y; ky <= ry; ky++)
								{
									for (int kz = z; kz <= rz; kz++)
									{
										if (kx != x || ky != y || kz != z)
											_Faces[d, kx, ky, kz] = false;
									}
								}
							}

							float dx = (rx - x + 1) * blockScale.x;
							float dy = (ry - y + 1) * blockScale.y;
							float dz = (rz - z + 1) * blockScale.z;
							Vector3[] vertices = new Vector3[4];

							switch (dir)
							{
								case Direction.Left:
									vertices[0] = new Vector3(px,					py,					pz);
									vertices[1] = new Vector3(px,					py,					pz + dz);
									vertices[2] = new Vector3(px,					py + dy,			pz + dz);
									vertices[3] = new Vector3(px,					py + dy,			pz);
									break;

								case Direction.Right:
									vertices[0] = new Vector3(px + blockScale.x,	py,					pz);
									vertices[1] = new Vector3(px + blockScale.x,	py + dy,			pz);
									vertices[2] = new Vector3(px + blockScale.x,	py + dy,			pz + dz);
									vertices[3] = new Vector3(px + blockScale.x,	py,					pz + dz);
									break;

								case Direction.Down:
									vertices[0] = new Vector3(px + dx,				py,					pz);
									vertices[1] = new Vector3(px + dx,				py,					pz + dz);
									vertices[2] = new Vector3(px,					py,					pz + dz);
									vertices[3] = new Vector3(px,					py,					pz);
									break;

								case Direction.Up:
									vertices[0] = new Vector3(px + dx,				py + blockScale.y,	pz);
									vertices[1] = new Vector3(px,					py + blockScale.y,	pz);
									vertices[2] = new Vector3(px,					py + blockScale.y,	pz + dz);
									vertices[3] = new Vector3(px + dx,				py + blockScale.y,	pz + dz);
									break;

								case Direction.Backward:
									vertices[0] = new Vector3(px,					py + dy,			pz);
									vertices[1] = new Vector3(px + dx,				py + dy,			pz);
									vertices[2] = new Vector3(px + dx,				py,					pz);
									vertices[3] = new Vector3(px,					py,					pz);
									break;

								case Direction.Forward:
									vertices[0] = new Vector3(px,					py + dy,			pz + blockScale.z);
									vertices[1] = new Vector3(px,					py,					pz + blockScale.z);
									vertices[2] = new Vector3(px + dx,				py,					pz + blockScale.z);
									vertices[3] = new Vector3(px + dx,				py + dy,			pz + blockScale.z);
									break;
							}

							meshBuilder.AddQuad(vertices, block.Color, block.GetSubMesh(), dir.ToUnitVector());
						}
					}
		}

		return meshBuilder.BuildMesh();
	}

	private bool FaceVisible(Block block, GridPosition blockPosition, Direction direction, Chunk chunk)
	{
		GridPosition otherBlockPosition = blockPosition + direction.ToPositionOffset();
		Block otherBlock = chunk.GetBlock(otherBlockPosition.x, otherBlockPosition.y, otherBlockPosition.z);

		bool visible = block.IsFaceVisible(direction.Opposite(), otherBlock);
		return visible;
	}

	private bool SameColor(Block b1, Block b2)
	{
		Color32 c1 = b1.Color;
		Color32 c2 = b2.Color;

		return c1.a == c2.a && c1.b == c2.b && c1.g == c2.g && c1.r == c2.r;
	}
}
