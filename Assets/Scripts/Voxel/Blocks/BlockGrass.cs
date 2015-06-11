using UnityEngine;

public class BlockGrass : Block
{
    public BlockGrass(Chunk parent)
        : base(parent)
    {
        BlockColor = new Color32() { a = 255, r = 0, g = 255, b = 0 };
    }
}
