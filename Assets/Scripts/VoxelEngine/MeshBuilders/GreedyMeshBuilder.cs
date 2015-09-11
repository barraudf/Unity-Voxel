﻿using UnityEngine;
using System.Collections;
using System;

public class GreedyMeshBuilder : ChunkMeshBuilder
{
    public override MeshData[] BuildMeshes(Chunk chunk)
	{
		Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
		Block[,,,] _Faces = new Block[directions.Length, chunk.SizeX, chunk.SizeY, chunk.SizeZ];
		MeshBuilder meshBuilder = new MeshBuilder();
		float blockScale = chunk.BlockScale;

		for (int x = 0; x < chunk.SizeX; x++)
			for (int y = 0; y < chunk.SizeY; y++)
				for (int z = 0; z < chunk.SizeZ; z++)
				{
					Block block = chunk.GetBlock(x, y, z);
					if (block != null)
					{
						GridPosition blockPosition = new GridPosition(x, y, z);

						for (int d = 0; d < directions.Length; d++)
						{
							if (FaceVisible(block, blockPosition, directions[d], chunk))
								_Faces[d, x, y, z] = block;
						}
					}
				}

		for (int d = 0; d < directions.Length; d++)
		{
			Direction dir = directions[d];
			for (int x = 0; x < chunk.SizeX; x++)
				for (int y = 0; y < chunk.SizeY; y++)
					for (int z = 0; z < chunk.SizeZ; z++)
					{

						Block block = _Faces[d, x, y, z];

						if (block != null)
						{
							GridPosition blockPosition = new GridPosition(x, y, z);
							Vector3 finalOrigin = ((Vector3)blockPosition - chunk.MeshOrigin - chunk.BlockOrigin) * blockScale;
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
										while (ry < chunk.SizeY && SameColor(block, _Faces[d, x, ry, z]))
											ry++;
										ry--;

										rz = z + 1;
										while (rz < chunk.SizeZ)
										{
											bool inc = true;
											for (int k = y; k <= ry; ++k)
											{
												inc = inc & (SameColor(block, _Faces[d, x, k, rz]));
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
										while (rx < chunk.SizeX && SameColor(block, _Faces[d, rx, y, z]))
											rx++;
										rx--;

										rz = z + 1;
										while (rz < chunk.SizeZ)
										{
											bool inc = true;
											for (int k = x; k <= rx; ++k)
											{
												inc = inc & (SameColor(block, _Faces[d, k, y, rz]));
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
										while (rx < chunk.SizeX && SameColor(block, _Faces[d, rx, y, z]))
											rx++;
										rx--;

										ry = y + 1;
										while (ry < chunk.SizeY)
										{
											bool inc = true;
											for (int k = x; k <= rx; ++k)
											{
												inc = inc & (SameColor(block, _Faces[d, k, ry, z]));
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
											_Faces[d, kx, ky, kz] = null;
									}
								}
							}

							int dx = rx - x;
							int dy = ry - y;
							int dz = rz - z;
							Vector3[] vertices = new Vector3[4];

							switch (dir)
							{
								case Direction.Left:
									vertices[0] = new Vector3(px,									py,									pz);
									vertices[1] = new Vector3(px,									py,									pz + blockScale + blockScale * dz);
									vertices[2] = new Vector3(px,									py + blockScale + blockScale * dy,	pz + blockScale + blockScale * dz);
									vertices[3] = new Vector3(px,									py + blockScale + blockScale * dy,	pz);
									break;

								case Direction.Right:
									vertices[0] = new Vector3(px + blockScale,						py,									pz);
									vertices[1] = new Vector3(px + blockScale,						py + blockScale + blockScale * dy,	pz);
									vertices[2] = new Vector3(px + blockScale,						py + blockScale + blockScale * dy,	pz + blockScale + blockScale * dz);
									vertices[3] = new Vector3(px + blockScale,						py,									pz + blockScale + blockScale * dz);
									break;

								case Direction.Down:
									vertices[0] = new Vector3(px + blockScale + blockScale * dx,	py,									pz);
									vertices[1] = new Vector3(px + blockScale + blockScale * dx,	py,									pz + blockScale + blockScale * dz);
									vertices[2] = new Vector3(px,									py,									pz + blockScale + blockScale * dz);
									vertices[3] = new Vector3(px,									py,									pz);
									break;

								case Direction.Up:
									vertices[0] = new Vector3(px + blockScale + blockScale * dx,	py + blockScale,					pz);
									vertices[1] = new Vector3(px,									py + blockScale,					pz);
									vertices[2] = new Vector3(px,									py + blockScale,					pz + blockScale + blockScale * dz);
									vertices[3] = new Vector3(px + blockScale + blockScale * dx,	py + blockScale,					pz + blockScale + blockScale * dz);
									break;

								case Direction.Backward:
									vertices[0] = new Vector3(px,									py + blockScale + blockScale * dy,	pz);
									vertices[1] = new Vector3(px + blockScale + blockScale * dx,	py + blockScale + blockScale * dy,	pz);
									vertices[2] = new Vector3(px + blockScale + blockScale * dx,	py,									pz);
									vertices[3] = new Vector3(px,									py,									pz);
									break;

								case Direction.Forward:
									vertices[0] = new Vector3(px,									py + blockScale + blockScale * dy,	pz + blockScale);
									vertices[1] = new Vector3(px,									py,									pz + blockScale);
									vertices[2] = new Vector3(px + blockScale + blockScale * dx,	py,									pz + blockScale);
									vertices[3] = new Vector3(px + blockScale + blockScale * dx,	py + blockScale + blockScale * dy,	pz + blockScale);
									break;
							}

							meshBuilder.AddQuad(vertices, block.GetBlockColor());
						}
					}
		}

		return meshBuilder.BuildMesh();
	}

	private bool FaceVisible(Block block, GridPosition blockPosition, Direction direction, Chunk chunk)
	{
		GridPosition otherBlockPosition = blockPosition + direction.ToUnitVector();
		Block otherBlock = chunk.GetBlock(otherBlockPosition.x, otherBlockPosition.y, otherBlockPosition.z);

		bool visible = block.IsFaceVisible(direction.Opposite(), otherBlock);
		return visible;
	}

	private bool SameColor(Block b1, Block b2)
	{
		if (b2 == null)
			return false;

		Color32 c1 = b1.GetBlockColor();
		Color32 c2 = b2.GetBlockColor();

		return c1.a == c2.a && c1.b == c2.b && c1.g == c2.g && c1.r == c2.r;
	}
}
