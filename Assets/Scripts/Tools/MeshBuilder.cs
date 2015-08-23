using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBuilder
{
	/// <summary>
	/// Soft maximum number of vertices per mesh. Can not be more than Unity limit (which is equal to UInt16.MaxValue)
	/// </summary>
	public int SoftVerticesCountLimit;

	private List<Vector3> _Vertices;
	private List<int> _Triangles;
	private List<Color32> _Colors;
	private List<Mesh> _Meshes;

	public MeshBuilder()
	{
		SoftVerticesCountLimit = System.UInt16.MaxValue;
		_Vertices = new List<Vector3>();
		_Triangles = new List<int>();
		_Colors = new List<Color32>();
		_Meshes = new List<Mesh>(1);
	}

	/// <summary>
	/// Add a quad to the mesh
	/// </summary>
	/// <param name="vertices">List of vertices of the quad (order is important!).</param>
	/// <param name="color">Color to apply to all 4 vertices</param>
	public void AddQuad(Vector3[] vertices, Color32 color)
	{
		if (!CheckVerticeCount(4, vertices.Length))
			return;

		for (int i = 0; i < 4; i++)
		{
			_Vertices.Add(vertices[i]);
			_Colors.Add(color);
		}

		int vertCount = _Vertices.Count;
		_Triangles.Add(vertCount - 4);
		_Triangles.Add(vertCount - 3);
		_Triangles.Add(vertCount - 1);

		_Triangles.Add(vertCount - 1);
		_Triangles.Add(vertCount - 3);
		_Triangles.Add(vertCount - 2);
	}

	/// <summary>
	/// Use all pending data to create a new mesh
	/// </summary>
	private void FlushMeshData()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = _Vertices.ToArray();
		mesh.triangles = _Triangles.ToArray();
		mesh.colors32 = _Colors.ToArray();
		mesh.RecalculateNormals();
		mesh.Optimize();
		_Meshes.Add(mesh);
		Clear();
	}

	/// <summary>
	/// Clear all data not already used to build a mesh
	/// </summary>
	public void Clear()
	{
		_Vertices.Clear();
		_Triangles.Clear();
		_Colors.Clear();
	}

	/// <summary>
	/// Return one or more meshes build from the data send to the builder
	/// </summary>
	/// <returns></returns>
	public Mesh[] BuildMesh()
	{
		FlushMeshData();
		return _Meshes.ToArray();
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

		if (_Vertices.Count + actualVertexCount > System.UInt16.MaxValue || _Vertices.Count + actualVertexCount > SoftVerticesCountLimit)
			FlushMeshData();

		return true;
	}
}
