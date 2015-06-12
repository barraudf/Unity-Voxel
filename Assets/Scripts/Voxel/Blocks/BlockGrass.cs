using UnityEngine;
using System;

[Serializable]
public class BlockGrass : Block
{
    public BlockGrass(Chunk parent)
        : base(parent)
    {
    }
    
    protected override Color32 GetBlockColor()
    {
        return new Color32() { a = 255, r = 0, g = 255, b = 0 };
    }
}
