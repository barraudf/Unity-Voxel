using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class MVBlockTest
{
	[Test]
	public void GetBlockColorTest()
	{
		MVChunk chunk = new MVChunk(null);
		chunk.Palette = new Color[] { Color.white, Color.blue };
		chunk.Name = "TestChunk";
		chunk.InitBlocks(3, 1, 1);
		for (int x = 0; x < chunk.SizeX; x++)
		{
			MVBlock block = new MVBlock(chunk);
			switch (x)
			{
				case 0:
					block.ColorIndex = 3;
					break;
				case 1:
					block.ColorIndex = 1;
					break;
				case 2:
					block.ColorIndex = 2;
					block.Alpha = 127;
					break;
			}

			chunk.SetBlock(x, 0, 0, block);
		}

		Assert.AreEqual((Color32)Color.magenta, chunk.GetBlock(0, 0, 0).GetBlockColor(), "#1");
		Assert.AreEqual((Color32)Color.white, chunk.GetBlock(1, 0, 0).GetBlockColor(), "#2");
		Assert.AreEqual(new Color32(0, 0, 255, 127), chunk.GetBlock(2, 0, 0).GetBlockColor(), "#3");
	}
}
