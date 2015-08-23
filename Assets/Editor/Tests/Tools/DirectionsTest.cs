using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class DirectionsTest
{
	[Test]
	public void OppositeTest()
	{
		Assert.AreEqual(Direction.Down, Direction.Up.Opposite(), "#1");
		Assert.AreEqual(Direction.Up, Direction.Down.Opposite(), "#2");
		Assert.AreEqual(Direction.Left, Direction.Right.Opposite(), "#3");
		Assert.AreEqual(Direction.Right, Direction.Left.Opposite(), "#4");
		Assert.AreEqual(Direction.Backward, Direction.Forward.Opposite(), "#5");
		Assert.AreEqual(Direction.Forward, Direction.Backward.Opposite(), "#6");

		Assert.AreEqual(Direction.Down, Direction.YPos.Opposite(), "#7");
		Assert.AreEqual(Direction.Up, Direction.YNeg.Opposite(), "#8");
		Assert.AreEqual(Direction.Left, Direction.XPos.Opposite(), "#9");
		Assert.AreEqual(Direction.Right, Direction.XNeg.Opposite(), "#10");
		Assert.AreEqual(Direction.Backward, Direction.ZPos.Opposite(), "#11");
		Assert.AreEqual(Direction.Forward, Direction.ZNeg.Opposite(), "#12");
	}

	[Test]
	public void NormalTest()
	{
		Assert.AreEqual(Vector3.down, Direction.Down.ToUnitVector(), "#1");
		Assert.AreEqual(Vector3.up, Direction.Up.ToUnitVector(), "#2");
		Assert.AreEqual(Vector3.left, Direction.Left.ToUnitVector(), "#3");
		Assert.AreEqual(Vector3.right, Direction.Right.ToUnitVector(), "#4");
		Assert.AreEqual(Vector3.back, Direction.Backward.ToUnitVector(), "#5");
		Assert.AreEqual(Vector3.forward, Direction.Forward.ToUnitVector(), "#6");

		Assert.AreEqual(Vector3.down, Direction.YNeg.ToUnitVector(), "#7");
		Assert.AreEqual(Vector3.up, Direction.YPos.ToUnitVector(), "#8");
		Assert.AreEqual(Vector3.left, Direction.XNeg.ToUnitVector(), "#9");
		Assert.AreEqual(Vector3.right, Direction.XPos.ToUnitVector(), "#10");
		Assert.AreEqual(Vector3.back, Direction.ZNeg.ToUnitVector(), "#11");
		Assert.AreEqual(Vector3.forward, Direction.ZPos.ToUnitVector(), "#12");
	}
}
