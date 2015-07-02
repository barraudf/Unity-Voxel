using UnityEngine;
using System.Collections.Generic;

public class MeshData
{
    private static Color32 COLOR_BLACK = new Color32(0, 0, 0, 255);

    public List<Vector3> Vertices;
    public List<int> Triangles;
    public List<Color32> Colors;

    public List<Vector3> ColVertices;
    public List<int> ColTriangles;

    public bool UseAlternateCollider;

    public MeshData()
    {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        Colors = new List<Color32>();
        ColVertices = new List<Vector3>();
        ColTriangles = new List<int>();
    }

    public void AddQuadTriangles(bool UseAlternateTriangleOrientation = false)
    {
        int vertCount = Vertices.Count;
        if(UseAlternateTriangleOrientation)
        {
            AddTriangle(vertCount - 4);
            AddTriangle(vertCount - 3);
            AddTriangle(vertCount - 1);

            AddTriangle(vertCount - 1);
            AddTriangle(vertCount - 3);
            AddTriangle(vertCount - 2);
        }
        else
        {
            AddTriangle(vertCount - 4);
            AddTriangle(vertCount - 3);
            AddTriangle(vertCount - 2);

            AddTriangle(vertCount - 4);
            AddTriangle(vertCount - 2);
            AddTriangle(vertCount - 1);
        }
    }

    public void AddQuadColors(Color32 color)
    {
        for (int i = 0; i < 4; i++)
            Colors.Add(Color32.Lerp(COLOR_BLACK, color, 0.5f + ((float)i /4)/2));
    }

    public void AddVertex(Vector3 vertex)
    {
        Vertices.Add(vertex);

        if (!UseAlternateCollider)
            ColVertices.Add(vertex);
    }

    public void AddTriangle(int tri)
    {
        Triangles.Add(tri);

        if (!UseAlternateCollider)
            ColTriangles.Add(tri - (Vertices.Count - ColVertices.Count));
    }

    public void ToMeshFilter(Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.colors32 = Colors.ToArray();
        mesh.RecalculateNormals();
    }

    public Mesh ToMeshCollider()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = ColVertices.ToArray();
        mesh.triangles = ColTriangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    public void Clear()
    {
        Vertices.Clear();
        Triangles.Clear();
        Colors.Clear();
        ColVertices.Clear();
        ColTriangles.Clear();
    }
}