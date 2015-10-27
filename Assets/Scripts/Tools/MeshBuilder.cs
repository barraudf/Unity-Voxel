using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshBuilder
{
	private List<Vector3> _Vertices;
	private List<int>[] _Triangles;
	private List<Color32> _Colors;
	private List<Vector3> _Normals;
	private List<MeshData> _MeshData;

	public MeshBuilder()
	{
		int submeshesCount = Enum.GetValues(typeof(SubMeshes)).Length;

		_Vertices = new List<Vector3>();
		_Normals = new List<Vector3>();
		_Triangles = new List<int>[submeshesCount];
		for(int i = 0; i < submeshesCount; i++)
			_Triangles[i] = new List<int>();
		_Colors = new List<Color32>();
		_MeshData = new List<MeshData>(1);
	}

	/// <summary>
	/// Add a quad to the mesh
	/// </summary>
	/// <param name="vertices">List of vertices of the quad (order is important!).</param>
	/// <param name="color">Color to apply to all 4 vertices</param>
	/// <param name="subMesh">Which submesh the quad belongs to</param>
	/// <param name="normal">normal of  each vertex</param>
	public void AddQuad(Vector3[] vertices, Color32 color, SubMeshes subMesh, Vector3 normal)
	{
		if (!CheckVerticeCount(4, vertices.Length))
			return;

		for (int i = 0; i < 4; i++)
		{
			_Vertices.Add(vertices[i]);
			_Colors.Add(color);
			_Normals.Add(normal);
		}

		int vertCount = _Vertices.Count;
		_Triangles[(int)subMesh].Add(vertCount - 4);
		_Triangles[(int)subMesh].Add(vertCount - 3);
		_Triangles[(int)subMesh].Add(vertCount - 1);

		_Triangles[(int)subMesh].Add(vertCount - 1);
		_Triangles[(int)subMesh].Add(vertCount - 3);
		_Triangles[(int)subMesh].Add(vertCount - 2);
	}

	/// <summary>
	/// Use all pending data to create a new mesh
	/// </summary>
	private void FlushMeshData()
	{
		int[][] triangles;
		triangles = new int[_Triangles.Length][];
		for (int i = 0; i < _Triangles.Length; i++)
			triangles[i] = _Triangles[i].ToArray();
		MeshData meshData = new MeshData(_Vertices.ToArray(), triangles, _Colors.ToArray(), _Normals.ToArray());
		_MeshData.Add(meshData);
		Clear();
	}

	/// <summary>
	/// Clear all data not already used to build a mesh
	/// </summary>
	public void Clear()
	{
		_Vertices.Clear();
		for (int i = 0; i < _Triangles.Length; i++)
			_Triangles[i].Clear();
		_Colors.Clear();
		_Normals.Clear();
	}

	/// <summary>
	/// Return one or more meshes build from the data send to the builder
	/// </summary>
	/// <returns></returns>
	public MeshData[] BuildMesh()
	{
		FlushMeshData();
		return _MeshData.ToArray();
    }

	/// <summary>
	/// Check that the function has received the right number of vertices, and create a new mesh if it exceed the limit per mesh
	/// </summary>
	/// <param name="expectedVertexCount">How many vertices are required to build the requested shape</param>
	/// <param name="actualVertexCount">How many vertices are actually provided to build the shape</param>
	/// <returns></returns>
	private bool CheckVerticeCount(int expectedVertexCount, int actualVertexCount)
	{
		if (actualVertexCount != expectedVertexCount)
		{
			Debug.LogErrorFormat("A Quad must have {0} vertices", expectedVertexCount);
			return false;
		}

		if (_Vertices.Count + actualVertexCount > System.UInt16.MaxValue)
			FlushMeshData();

		return true;
	}
}
