using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class GridPosition_Test
{
	[Test]
	public void ToVector3Test()
	{
		ToVector3Test(new Vector3(0f, 0f, 0f), new GridPosition(0, 0, 0));
		ToVector3Test(new Vector3(1f, 1f, 1f), new GridPosition(1, 1, 1));
		ToVector3Test(new Vector3(-1f, -1f, -1f), new GridPosition(-1, -1, -1));
	}

	public void ToVector3Test(Vector3 expected, GridPosition actual)
	{
		Assert.AreEqual(actual.ToVector3(), expected);
	}

	[Test]
	public void EqualsTest()
	{
		EqualsTest(new GridPosition(0, 0, 0), new GridPosition(0, 0, 0));
		EqualsTest(new GridPosition(1, 1, 1), new GridPosition(1, 1, 1));
		EqualsTest(new GridPosition(-1, -1, -1), new GridPosition(-1, -1, -1));
	}

	public void EqualsTest(GridPosition v1, GridPosition v2)
	{
		Assert.True(v1.Equals(v2),			"Vector3i.Equals(Vector3i) failed");
		Assert.True(v1.Equals((object)v2),	"Vector3i.Equals(object) failed");
	}

	[Test]
	public void GetHashCodeTest()
	{
		Assert.AreEqual(new GridPosition(0, 0, 0).GetHashCode(),	0);
		Assert.AreEqual(new GridPosition(1, 1, 1).GetHashCode(),	3);
		Assert.AreEqual(new GridPosition(-1, -1, -1).GetHashCode(), -3);
	}

	[Test]
	public void OperatorPlusIntTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);

		Assert.AreEqual(v1, v2 + v3, "#1");
		Assert.AreEqual(v2, v1 + v2, "#2");
		Assert.AreEqual(v3, v1 + v3, "#3");
	}

	[Test]
	public void OperatorPlusFloatTest()
	{
		Vector3 v1 = new Vector3(0.5f, 0.5f, 0.5f);
		Vector3 v2 = new Vector3(1.5f, 1.5f, 1.5f);
		GridPosition v3 = new GridPosition(-1, -1, -1);
		GridPosition v4 = new GridPosition(1, 1, 1);
		Vector3 v5 = new Vector3(2.5f, 2.5f, 2.5f);

		Assert.AreEqual(v1, v3 + v2, "#1");
		Assert.AreEqual(v5, v4 + v2, "#2");
	}

	[Test]
	public void OperatorMinusTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);
		GridPosition v4 = new GridPosition(2, 2, 2);

		Assert.AreEqual(v2, v1 - v3, "#1");
		Assert.AreEqual(v3, v1 - v2, "#2");
		Assert.AreEqual(v4, v3 - v2, "#3");
	}

	[Test]
	public void OperatorMultiplicationIntTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);
		GridPosition v4 = new GridPosition(2, 2, 2);
		GridPosition v5 = new GridPosition(-2, -2, -2);

		Assert.AreEqual(v1, v1 * 2, "#1");
		Assert.AreEqual(v4, v3 * 2, "#2");
		Assert.AreEqual(v5, v2 * 2, "#3");
	}

	[Test]
	public void OperatorMultiplicationFloatTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);
		Vector3 v4 = new Vector3(1.5f, 1.5f, 1.5f);
		Vector3 v5 = new Vector3(-1.5f, -1.5f, -1.5f);

		Assert.AreEqual(v1.ToVector3(),	v1 * 1.5f, "#1");
		Assert.AreEqual(v4,				v3 * 1.5f, "#2");
		Assert.AreEqual(v5,				v2 * 1.5f, "#3");
	}

	[Test]
	public void OperatorMultiplicationVector3iTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);
		GridPosition v4 = new GridPosition(2, 2, 2);
		GridPosition v5 = new GridPosition(-2, -2, -2);
		GridPosition v6 = new GridPosition(4, 4, 4);
		GridPosition v7 = new GridPosition(-4, -4, -4);

		Assert.AreEqual(v1, v1 * v2, "#1");
		Assert.AreEqual(v2, v2 * v3, "#1");
		Assert.AreEqual(v3, v3 * v3, "#3");
		Assert.AreEqual(v6, v4 * v4, "#4");
		Assert.AreEqual(v7, v4 * v5, "#5");
	}

	[Test]
	public void OperatorDivisionTest()
	{
		GridPosition v1 = new GridPosition(0, 0, 0);
		GridPosition v2 = new GridPosition(-1, -1, -1);
		GridPosition v3 = new GridPosition(1, 1, 1);
		GridPosition v4 = new GridPosition(2, 2, 2);
		GridPosition v5 = new GridPosition(-2, -2, -2);
		GridPosition v6 = new GridPosition(4, 4, 4);
		GridPosition v7 = new GridPosition(-4, -4, -4);

		Assert.AreEqual(v1, v1 / v2, "#1");
		Assert.AreEqual(v2, v2 / v3, "#2");
		Assert.AreEqual(v3, v3 / v3, "#3");
		Assert.AreEqual(v4, v6 / v4, "#4");
		Assert.AreEqual(v5, v6 / v5, "#5");
		Assert.AreEqual(v4, v7 / v5, "#6");
	}

	[Test]
	public void DistanceSquaredTest()
	{
		GridPosition origin = new GridPosition(0, 0, 0);
		GridPosition v1 = new GridPosition(2, 2, 2);
		GridPosition v2 = new GridPosition(-2, -2, -2);
		GridPosition v3 = new GridPosition(4, -3, 5);

		Assert.AreEqual(12, GridPosition.DistanceSquared(v1, origin), "#1");
		Assert.AreEqual(12, GridPosition.DistanceSquared(v2, origin), "#2");
		Assert.AreEqual(50, GridPosition.DistanceSquared(v3, origin), "#3");

		Assert.AreEqual(GridPosition.DistanceSquared(v1, origin),
			GridPosition.DistanceSquared(v2, origin), "#4");
		Assert.AreEqual(GridPosition.DistanceSquared(v3, origin),
			GridPosition.DistanceSquared(origin, v3), "#5");

		Assert.AreEqual(12, GridPosition.DistanceSquared(origin, v1), "#6");
		Assert.AreEqual(12, GridPosition.DistanceSquared(origin, v2), "#7");
		Assert.AreEqual(50, GridPosition.DistanceSquared(origin, v3), "#8");

		Assert.AreEqual(3, GridPosition.DistanceSquared(origin, GridPosition.One), "#9");
		Assert.AreEqual(0, GridPosition.DistanceSquared(v3, v3), "#10");
	}

	[Test]
	public void OperatorImpliciteTest()
	{
		Assert.AreEqual((Vector3)new GridPosition(-1, 2, 3), new Vector3(-1f, 2f, 3f), "#1");
		Assert.AreEqual((GridPosition)new Vector3(-1.4f, 2.2f, 3.6f), new GridPosition(-2, 2, 3), "#2");
	}
}
