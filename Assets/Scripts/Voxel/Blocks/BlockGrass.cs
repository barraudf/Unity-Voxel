using UnityEngine;
using System;

[Serializable]
public class BlockGrass : Block
{
    public BlockGrass()
        : base()
    {
        Color = new Color32() { a = 255, r = 0, g = 255, b = 0 };
    }
}
