using UnityEngine;
using NUnit.Framework;
using System;

[TestFixture]
public class ChunkTest
{
	[Test]
	public void ChunkSizeTest()
	{
		TestChunk chunk = new TestChunk();
		chunk.InitBlocks(5, 6, 7);

		Assert.AreEqual(5, chunk.SizeX, "#1");
		Assert.AreEqual(6, chunk.SizeY, "#2");
		Assert.AreEqual(7, chunk.SizeZ, "#3");
	}

	[Test]
	public void IsLocalCoordinateTest()
	{
		TestChunk chunk = new TestChunk();
		chunk.InitBlocks(5, 6, 7);

		Assert.IsTrue(chunk.IsLocalCoordinates(new GridPosition(4, 5, 6)), "#1");
		Assert.IsFalse(chunk.IsLocalCoordinates(new GridPosition(5, 6, 7)), "#2");

		Assert.IsTrue(chunk.IsLocalCoordinates(4, 5, 6), "#3");
		Assert.IsFalse(chunk.IsLocalCoordinates(5, 6, 7), "#4");
	}

	private class TestChunk : Chunk
	{
		public TestChunk()
			: base(null)
		{

		}

		protected override Block GetExternalBlock(int x, int y, int z)
		{
			throw new NotImplementedException();
		}

		protected override void SetExternalBlock(int x, int y, int z, Block block)
		{
			throw new NotImplementedException();
		}
	}
}


