using System;
using UnityEngine;

public class MeshData
{
	private Vector3[] Vertices;
	private int[][] Triangles;
	private Color32[] Colors;

	public MeshData(Vector3[] vertices, int[][] triangles, Color32[] colors)
	{
		Vertices = vertices;
		Triangles = triangles;
		Colors = colors;
	}

	public Mesh ToMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = Vertices;
		mesh.subMeshCount = Triangles.Length;
		for(int i = 0; i < Triangles.Length; i++)
			if(Triangles[i].Length > 0)
				mesh.SetTriangles(Triangles[i], i);
			else
				mesh.SetTriangles(new int[3] { 0, 0, 0 }, i); // Required because MeshCollider don't work with empty submeshes
		mesh.colors32 = Colors;
		mesh.RecalculateNormals();
		return mesh;
	}
}
