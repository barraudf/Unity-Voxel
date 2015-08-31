using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class VoxLoader : ChunkLoader
{
	public List<MVLayer> Layers;

	public VoxLoader()
	{
		Layers = new List<MVLayer>();
	}

	public override void LoadChunk(Chunk chunk)
	{
		MVChunk mvChunk = chunk as MVChunk;
		if(mvChunk == null)
		{
			Debug.LogError("chunk is not a MVChunk, cannot load from a vox file");
			return;
		}

		for(int i = 0; i < Layers.Count; i++)
		{
			LoadVOXFromFile(Layers[i].FilePath, mvChunk, Layers[i]);
		}
	}

	public static void LoadVOXFromFile(string path, MVChunk chunk, MVLayer layer)
	{
		byte[] bytes = File.ReadAllBytes(path);
		if (bytes[0] != 'V' ||
			bytes[1] != 'O' ||
			bytes[2] != 'X' ||
			bytes[3] != ' ')
		{
			Debug.LogError("Invalid VOX file, magic number mismatch");
			return;
		}

		LoadVOXFromData(chunk, layer, bytes);
	}

	private static void LoadVOXFromData(MVChunk chunk, MVLayer layer, byte[] data)
	{
		using (MemoryStream ms = new MemoryStream(data))
		{
			using (BinaryReader br = new BinaryReader(ms))
			{
				// "VOX "
				br.ReadInt32();
				// "VERSION "
				layer.ReadVersion(chunk, br.ReadBytes(4));

				byte[] chunkId = br.ReadBytes(4);
				if (chunkId[0] != 'M' ||
					chunkId[1] != 'A' ||
					chunkId[2] != 'I' ||
					chunkId[3] != 'N')
				{
					Debug.LogError("[VoxLoader] Invalid MainChunk ID, main chunk expected");
					return;
				}

				int chunkSize = br.ReadInt32();
				int childrenSize = br.ReadInt32();

				// main chunk should have nothing... skip
				br.ReadBytes(chunkSize);

				int readSize = 0;
				while (readSize < childrenSize)
				{
					chunkId = br.ReadBytes(4);
					if (chunkId[0] == 'S' &&
						chunkId[1] == 'I' &&
						chunkId[2] == 'Z' &&
						chunkId[3] == 'E')
					{

						readSize += ReadSizeChunk(br, chunk, layer);

					}
					else if (chunkId[0] == 'X' &&
					  chunkId[1] == 'Y' &&
					  chunkId[2] == 'Z' &&
					  chunkId[3] == 'I')
					{

						readSize += ReadVoxelChunk(br, chunk, layer);

					}
					else if (chunkId[0] == 'R' &&
					  chunkId[1] == 'G' &&
					  chunkId[2] == 'B' &&
					  chunkId[3] == 'A')
					{

						layer.InitPalette(chunk);
						readSize += ReadPalattee(br, chunk, layer);

					}
					else
					{
						Debug.LogError("[VoxLoader] Chunk ID not recognized, got " + System.Text.Encoding.ASCII.GetString(chunkId));
						return;
					}
				}

				if (chunk.Palette == null)
					layer.InitDefaultPalette(chunk);
			}
		}
	}

	private static int ReadSizeChunk(BinaryReader br, MVChunk chunk, MVLayer layer)
	{
		int chunkSize = br.ReadInt32();
		int childrenSize = br.ReadInt32();

		int sizeX = br.ReadInt32();
		int sizeZ = br.ReadInt32();
		int sizeY = br.ReadInt32();

		layer.ReadSize(chunk, sizeX, sizeY, sizeZ);

		if (childrenSize > 0)
		{
			br.ReadBytes(childrenSize);
			Debug.LogWarning("[VoxLoader] Nested chunk not supported");
		}

		return chunkSize + childrenSize + 4 * 3;
	}

	private static int ReadVoxelChunk(BinaryReader br, MVChunk chunk, MVLayer layer)
	{
		int chunkSize = br.ReadInt32();
		int childrenSize = br.ReadInt32();
		int numVoxels = br.ReadInt32();

		for (int i = 0; i < numVoxels; ++i)
		{
			int x = (int)br.ReadByte();
			int z = (int)br.ReadByte();
			int y = (int)br.ReadByte();

			layer.ReadVolxel(chunk, x, y, z, br.ReadByte());
		}

		if (childrenSize > 0)
		{
			br.ReadBytes(childrenSize);
			Debug.LogWarning("[VoxLoader] Nested chunk not supported");
		}

		return chunkSize + childrenSize + 4 * 3;
	}

	private static int ReadPalattee(BinaryReader br, MVChunk chunk, MVLayer layer)
	{
		int chunkSize = br.ReadInt32();
		int childrenSize = br.ReadInt32();

		for (int i = 0; i < 256; ++i)
		{
			layer.ReadPalette(chunk, i, (float)br.ReadByte() / 255.0f, (float)br.ReadByte() / 255.0f, (float)br.ReadByte() / 255.0f, (float)br.ReadByte() / 255.0f);
		}

		if (childrenSize > 0)
		{
			br.ReadBytes(childrenSize);
			Debug.LogWarning("[VoxLoader] Nested chunk not supported");
		}

		return chunkSize + childrenSize + 4 * 3;
	}
}
