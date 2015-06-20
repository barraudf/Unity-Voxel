using UnityEngine;
using System;

using Object = System.Object;

public enum Direction { North, East, South, West, Up, Down };

[Serializable]
public class Block
{
    protected delegate MeshData FaceData(Chunk chunk, Vector3i position, MeshData meshData);

    #region Fields
    [NonSerialized]
    public bool changed = true;


    [NonSerialized]
    private static Vector3 v1 = new Vector3(-0.5f, -0.5f, -0.5f);
    [NonSerialized]
    private static Vector3 v2 = new Vector3(-0.5f, -0.5f,  0.5f);
    [NonSerialized]
    private static Vector3 v3 = new Vector3(-0.5f,  0.5f, -0.5f);
    [NonSerialized]
    private static Vector3 v4 = new Vector3(-0.5f,  0.5f,  0.5f);
    [NonSerialized]
    private static Vector3 v5 = new Vector3( 0.5f, -0.5f, -0.5f);
    [NonSerialized]
    private static Vector3 v6 = new Vector3( 0.5f, -0.5f,  0.5f);
    [NonSerialized]
    private static Vector3 v7 = new Vector3( 0.5f,  0.5f, -0.5f);
    [NonSerialized]
    private static Vector3 v8 = new Vector3( 0.5f,  0.5f,  0.5f);

    [NonSerialized]
    protected Color32 Color;
    #endregion Fields

    public Block()
    {
        Color = new Color32() { a = 255, r = 255, g = 255, b = 255 };
    }

    public virtual MeshData BlockData (Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        meshData = ProcessFace(FaceDataUp, chunk, position, meshData, Direction.Down, Vector3i.up);
        meshData = ProcessFace(FaceDataDown, chunk, position, meshData, Direction.Up, Vector3i.down);
        meshData = ProcessFace(FaceDataNorth, chunk, position, meshData, Direction.South, Vector3i.backward);
        meshData = ProcessFace(FaceDataSouth, chunk, position, meshData, Direction.North, Vector3i.forward);
        meshData = ProcessFace(FaceDataEast, chunk, position, meshData, Direction.West, Vector3i.right);
        meshData = ProcessFace(FaceDataWest, chunk, position, meshData, Direction.East, Vector3i.left);

        return meshData;
    }

    protected virtual MeshData ProcessFace(FaceData func, Chunk c, Vector3i position, MeshData meshData, Direction dir, Vector3i blockOffset)
    {
        Block otherBlock = c.GetBlock(position + blockOffset);
        if (!Object.ReferenceEquals(otherBlock, null) && !otherBlock.IsSolid(dir))
                meshData = func(c, position, meshData);

        return meshData;
    }

    #region Mesh creation
    protected virtual MeshData FaceDataUp(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v3);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataDown(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v1);
        meshData.AddVertex(position + v5);
        meshData.AddVertex(position + v6);
        meshData.AddVertex(position + v2);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataNorth(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v6);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v2);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataEast(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v5);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v6);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataSouth(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v1);
        meshData.AddVertex(position + v3);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v5);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataWest(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v2);
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v3);
        meshData.AddVertex(position + v1);

        meshData.AddQuadColors(Color);
        meshData.AddQuadTriangles();

        return meshData;
    }
    #endregion Mesh creation

    public virtual bool IsSolid(Direction direction)
    {
        return true;
    }
}
