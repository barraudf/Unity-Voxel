using System;
using UnityEngine;

public class MeshData
{
	private Vector3[] Vertices;
	private int[][] Triangles;
	private Color32[] Colors;
	private Vector3[] Normals;

	public MeshData(Vector3[] vertices, int[][] triangles, Color32[] colors, Vector3[] normals)
	{
		Vertices = vertices;
		Triangles = triangles;
		Colors = colors;
		Normals = normals;
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
		mesh.normals = Normals;
		return mesh;
	}
}
