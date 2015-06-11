using UnityEngine;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> Vertices;
    public List<int> Triangles;
    public List<Color32> Colors;

    public List<Vector3> ColVertices;
    public List<int> ColTriangles;

    public bool useRenderDataForCol;

    public MeshData()
    {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        Colors = new List<Color32>();
        ColVertices = new List<Vector3>();
        ColTriangles = new List<int>();
    }

    public void AddQuadTriangles()
    {
        int vertCount = Vertices.Count;
        Triangles.Add(vertCount - 4);
        Triangles.Add(vertCount - 3);
        Triangles.Add(vertCount - 2);

        Triangles.Add(vertCount - 4);
        Triangles.Add(vertCount - 2);
        Triangles.Add(vertCount - 1);

        if (useRenderDataForCol)
        {
            int colVertCount = ColVertices.Count;
            ColTriangles.Add(colVertCount - 4);
            ColTriangles.Add(colVertCount - 3);
            ColTriangles.Add(colVertCount - 2);
            ColTriangles.Add(colVertCount - 4);
            ColTriangles.Add(colVertCount - 2);
            ColTriangles.Add(colVertCount - 1);
        }
    }

    public void AddQuadColors(Color32 color)
    {
        for (int i = 0; i < 4; i++)
            Colors.Add(color);
    }

    public void AddVertex(Vector3 vertex)
    {
        Vertices.Add(vertex);

        if (useRenderDataForCol)
            ColVertices.Add(vertex);
    }

    public void AddTriangle(int tri)
    {
        Triangles.Add(tri);

        if (useRenderDataForCol)
            ColTriangles.Add(tri - (Vertices.Count - ColVertices.Count));
    }
}