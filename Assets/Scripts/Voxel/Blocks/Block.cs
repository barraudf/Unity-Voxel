using UnityEngine;
using System;
using System.Collections.Generic;

using Object = System.Object;

public enum Direction { North, East, South, West, Up, Down };

[Serializable]
public abstract class Block
{
    protected delegate MeshData BuildFaceMesh(Chunk chunk, Vector3i position, MeshData meshData, float scale);

    #region Fields
    [NonSerialized]
    protected static Dictionary<Type, Color32> Colors = new Dictionary<Type, Color32>();

    [NonSerialized]
    public bool changed = true;


    [NonSerialized]
    protected static Vector3 v1 = new Vector3(-1f, -1f, -1f);
    [NonSerialized]
    protected static Vector3 v2 = new Vector3(-1f, -1f, 1f);
    [NonSerialized]
    protected static Vector3 v3 = new Vector3(-1f, 1f, -1f);
    [NonSerialized]
    protected static Vector3 v4 = new Vector3(-1f, 1f, 1f);
    [NonSerialized]
    protected static Vector3 v5 = new Vector3(1f, -1f, -1f);
    [NonSerialized]
    protected static Vector3 v6 = new Vector3(1f, -1f, 1f);
    [NonSerialized]
    protected static Vector3 v7 = new Vector3(1f, 1f, -1f);
    [NonSerialized]
    protected static Vector3 v8 = new Vector3(1f, 1f, 1f);

    #endregion Fields

    public virtual MeshData BuildMesh(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.UseAlternateCollider = false;
        float scale = World.BLOCK_SIZE / 2;

        meshData = BuildFace(BuildFaceUp, chunk, position, meshData, Direction.Down, Vector3i.Up, scale);
        meshData = BuildFace(BuildFaceDown, chunk, position, meshData, Direction.Up, Vector3i.Down, scale);
        meshData = BuildFace(BuildFaceNorth, chunk, position, meshData, Direction.South, Vector3i.Backward, scale);
        meshData = BuildFace(BuildFaceSouth, chunk, position, meshData, Direction.North, Vector3i.Forward, scale);
        meshData = BuildFace(BuildFaceEast, chunk, position, meshData, Direction.West, Vector3i.Right, scale);
        meshData = BuildFace(BuildFaceWest, chunk, position, meshData, Direction.East, Vector3i.Left, scale);

        return meshData;
    }

    protected virtual MeshData BuildFace(BuildFaceMesh func, Chunk chunk, Vector3i position, MeshData meshData, Direction dir, Vector3i blockOffset, float scale)
    {
        Block otherBlock = chunk.GetBlock(position + blockOffset);
        if (otherBlock != null && !otherBlock.IsSolid(dir))
            meshData = func(chunk, position, meshData, scale);

        return meshData;
    }

    #region Mesh creation
    protected virtual MeshData BuildFaceUp(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v4 * scale);
        meshData.AddVertex(position + v8 * scale);
        meshData.AddVertex(position + v7 * scale);
        meshData.AddVertex(position + v3 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles(true);

        return meshData;
    }

    protected virtual MeshData BuildFaceDown(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v1 * scale);
        meshData.AddVertex(position + v5 * scale);
        meshData.AddVertex(position + v6 * scale);
        meshData.AddVertex(position + v2 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData BuildFaceNorth(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v6 * scale);
        meshData.AddVertex(position + v8 * scale);
        meshData.AddVertex(position + v4 * scale);
        meshData.AddVertex(position + v2 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData BuildFaceEast(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v8 * scale);
        meshData.AddVertex(position + v6 * scale);
        meshData.AddVertex(position + v5 * scale);
        meshData.AddVertex(position + v7 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData BuildFaceSouth(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v1 * scale);
        meshData.AddVertex(position + v3 * scale);
        meshData.AddVertex(position + v7 * scale);
        meshData.AddVertex(position + v5 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData BuildFaceWest(Chunk chunk, Vector3i position, MeshData meshData, float scale)
    {
        meshData.AddVertex(position + v2 * scale);
        meshData.AddVertex(position + v4 * scale);
        meshData.AddVertex(position + v3 * scale);
        meshData.AddVertex(position + v1 * scale);

        meshData.AddQuadColors(GetBlockColor());
        meshData.AddQuadTriangles();

        return meshData;
    }
    #endregion Mesh creation

    public virtual bool IsSolid(Direction direction)
    {
        return true;
    }

    protected virtual Color32 GetBlockColor()
    {
        return Colors[GetType()];
    }
}
