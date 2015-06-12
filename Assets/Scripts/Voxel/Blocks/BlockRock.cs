using UnityEngine;
using System;

[Serializable]
public class BlockRock : Block
{
    public BlockRock(Chunk parent)
        : base(parent)
    {

    }

    protected override Color32 GetBlockColor()
    {
        return new Color32() { a = 255, r = 127, g = 127, b = 127 };
    }
}
