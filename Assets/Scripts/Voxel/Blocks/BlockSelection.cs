using UnityEngine;
using System.Collections.Generic;

public abstract class BlockSelection : Block
{
    public static Vector3[] BuildBox()
    {
        List<Vector3> vertices = new List<Vector3>(24);

        vertices.Add(v1 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v5 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v5 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v6 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v6 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v2 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v2 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v1 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v3 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v7 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v7 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v8 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v8 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v4 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v4 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v3 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v3 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v1 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v2 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v4 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v8 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v6 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v5 * World.BLOCK_SIZE * 0.6f);
        vertices.Add(v7 * World.BLOCK_SIZE * 0.6f);

        return vertices.ToArray();
    }
}
