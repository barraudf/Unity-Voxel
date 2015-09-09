//using UnityEngine;
//using NUnit.Framework;

//[TestFixture]
//public class DirectionsTest
//{
//	[Test]
//	public void OppositeTest()
//	{
//		Assert.AreEqual(Direction.Down, Direction.Up.Opposite(), "#1");
//		Assert.AreEqual(Direction.Up, Direction.Down.Opposite(), "#2");
//		Assert.AreEqual(Direction.Left, Direction.Right.Opposite(), "#3");
//		Assert.AreEqual(Direction.Right, Direction.Left.Opposite(), "#4");
//		Assert.AreEqual(Direction.Backward, Direction.Forward.Opposite(), "#5");
//		Assert.AreEqual(Direction.Forward, Direction.Backward.Opposite(), "#6");
//	}

//	[Test]
//	public void NormalTest()
//	{
//		Assert.AreEqual(Vector3.down, Direction.Down.ToUnitVector(), "#1");
//		Assert.AreEqual(Vector3.up, Direction.Up.ToUnitVector(), "#2");
//		Assert.AreEqual(Vector3.left, Direction.Left.ToUnitVector(), "#3");
//		Assert.AreEqual(Vector3.right, Direction.Right.ToUnitVector(), "#4");
//		Assert.AreEqual(Vector3.back, Direction.Backward.ToUnitVector(), "#5");
//		Assert.AreEqual(Vector3.forward, Direction.Forward.ToUnitVector(), "#6");
//	}
//}
