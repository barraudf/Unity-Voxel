using UnityEngine;

public enum Direction { North, East, South, West, Up, Down };

public class Block
{
    #region Fields
    private static Vector3 v1 = new Vector3(-0.5f, -0.5f, -0.5f);
    private static Vector3 v2 = new Vector3(-0.5f, -0.5f,  0.5f);
    private static Vector3 v3 = new Vector3(-0.5f,  0.5f, -0.5f);
    private static Vector3 v4 = new Vector3(-0.5f,  0.5f,  0.5f);
    private static Vector3 v5 = new Vector3( 0.5f, -0.5f, -0.5f);
    private static Vector3 v6 = new Vector3( 0.5f, -0.5f,  0.5f);
    private static Vector3 v7 = new Vector3( 0.5f,  0.5f, -0.5f);
    private static Vector3 v8 = new Vector3( 0.5f,  0.5f,  0.5f);

    protected Color32 BlockColor;
    protected Color32 HighlightColor;

    protected bool Highlight;
    private Chunk Chunk;
    #endregion Fields

    public Block(Chunk parent)
    {
        BlockColor = new Color32() { a = 255, r = 255, g = 255, b = 255 };
        HighlightColor = new Color32() { a = 255, r = 255, g = 0, b = 0 };
        Highlight = false;
        Chunk = parent;
    }

    public virtual MeshData BlockData (Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(position + Vector3i.up).IsSolid(Direction.Down))
            meshData = FaceDataUp(chunk, position, meshData);

        if (!chunk.GetBlock(position + Vector3i.down).IsSolid(Direction.Up))
            meshData = FaceDataDown(chunk, position, meshData);

        if (!chunk.GetBlock(position + Vector3i.backward).IsSolid(Direction.South))
            meshData = FaceDataNorth(chunk, position, meshData);

        if (!chunk.GetBlock(position + Vector3i.forward).IsSolid(Direction.North))
            meshData = FaceDataSouth(chunk, position, meshData);

        if (!chunk.GetBlock(position + Vector3i.right).IsSolid(Direction.West))
            meshData = FaceDataEast(chunk, position, meshData);

        if (!chunk.GetBlock(position + Vector3i.left).IsSolid(Direction.East))
            meshData = FaceDataWest(chunk, position, meshData);

        return meshData;
    }

    #region Mesh creation
    protected virtual MeshData FaceDataUp(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v3);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataDown(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v1);
        meshData.AddVertex(position + v5);
        meshData.AddVertex(position + v6);
        meshData.AddVertex(position + v2);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataNorth(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v6);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v2);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataEast(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v5);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v8);
        meshData.AddVertex(position + v6);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataSouth(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v1);
        meshData.AddVertex(position + v3);
        meshData.AddVertex(position + v7);
        meshData.AddVertex(position + v5);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }

    protected virtual MeshData FaceDataWest(Chunk chunk, Vector3i position, MeshData meshData)
    {
        meshData.AddVertex(position + v2);
        meshData.AddVertex(position + v4);
        meshData.AddVertex(position + v3);
        meshData.AddVertex(position + v1);

        meshData.AddQuadColors(Highlight ? HighlightColor : BlockColor);
        meshData.AddQuadTriangles();

        return meshData;
    }
    #endregion Mesh creation

    public virtual bool IsSolid(Direction direction)
    {
        return true;
    }

    public void SetHighlight(bool value)
    {
        Highlight = value;
        if(Chunk != null)
            Chunk.Invalidate();
    }
}
