using System;
using UnityEngine;

public class MeshData
{
	private Vector3[] Vertices;
	private int[] Triangles;
	private Color32[] Colors;

	public MeshData(Vector3[] vertices, int[] triangles, Color32[] colors)
	{
		Vertices = vertices;
		Triangles = triangles;
		Colors = colors;
	}

	public Mesh ToMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = Vertices;
		mesh.triangles = Triangles;
		mesh.colors32 = Colors;
		mesh.RecalculateNormals();
		mesh.Optimize();
		return mesh;
	}
}
