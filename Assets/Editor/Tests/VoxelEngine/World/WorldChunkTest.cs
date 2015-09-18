using UnityEngine;
using NUnit.Framework;
using System;

[TestFixture]
public class WorldChunkTest
{
	[Test]
	public void CalculateChunkOffsetTest()
	{
		WorldChunk chunk = new WorldChunk(null, GridPosition.Zero);
		chunk.InitBlocks(5, 6, 7);

		GridPosition p1 = chunk.CalculateChunkOffset(-11, 18, 5);
		GridPosition p2 = chunk.CalculateChunkOffset(5, 6, 7);
		GridPosition p3 = chunk.CalculateChunkOffset(24, -12, -6);

		Assert.AreEqual(-3, p1.x, "#1");
		Assert.AreEqual(3, p1.y, "#2");
		Assert.AreEqual(0, p1.z, "#3");

		Assert.AreEqual(1, p2.x, "#4");
		Assert.AreEqual(1, p2.y, "#5");
		Assert.AreEqual(1, p2.z, "#6");

		Assert.AreEqual(4, p3.x, "#7");
		Assert.AreEqual(-2, p3.y, "#8");
		Assert.AreEqual(-1, p3.z, "#9");
	}

	[Test]
	public void CalculateBlockPositionTest()
	{
		WorldChunk chunk = new WorldChunk(null, GridPosition.Zero);
		chunk.InitBlocks(5, 6, 7);

		GridPosition p1 = chunk.CalculateBlockPosition(-11, 18, 5);
		GridPosition p2 = chunk.CalculateBlockPosition(5, 6, 7);
		GridPosition p3 = chunk.CalculateBlockPosition(24, -12, -6);

		Assert.AreEqual(4, p1.x, "#1");
		Assert.AreEqual(0, p1.y, "#2");
		Assert.AreEqual(5, p1.z, "#3");

		Assert.AreEqual(0, p2.x, "#4");
		Assert.AreEqual(0, p2.y, "#5");
		Assert.AreEqual(0, p2.z, "#6");

		Assert.AreEqual(4, p3.x, "#7");
		Assert.AreEqual(0, p3.y, "#8");
		Assert.AreEqual(1, p3.z, "#9");
	}
}
