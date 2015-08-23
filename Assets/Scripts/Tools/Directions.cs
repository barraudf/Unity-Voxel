using UnityEngine;

public enum Direction
{
	Up			= 0,
	Down		= 1,
	Right		= 2,
	Left		= 3,	
	Forward		= 4,
	Backward	= 5,

	YPos		= Up,
	YNeg		= Down,
	XPos		= Right,
	XNeg		= Left,
	ZPos		= Forward,
	ZNeg		= Backward
}

public static class DirectionExtensions
{
	public static Direction Opposite(this Direction dir)
	{
		Direction opposite;

		switch(dir)
		{
			case Direction.Up:
				opposite = Direction.Down;
				break;
			case Direction.Down:
				opposite = Direction.Up;
				break;
			case Direction.Right:
				opposite = Direction.Left;
				break;
			case Direction.Left:
				opposite = Direction.Right;
				break;
			case Direction.Forward:
				opposite = Direction.Backward;
				break;
			case Direction.Backward:
				opposite = Direction.Forward;
				break;
			default:
				Debug.LogErrorFormat("Direction \"{0}\" has no opposite defined", dir.ToString());
				opposite = Direction.Up;
				break;
		}

		return opposite;
	}

	public static Vector3 ToUnitVector(this Direction dir)
	{
		Vector3 normal;

		switch(dir)
		{
			case Direction.Up:
				normal = Vector3.up;
				break;
			case Direction.Down:
				normal = Vector3.down;
				break;
			case Direction.Right:
				normal = Vector3.right;
				break;
			case Direction.Left:
				normal = Vector3.left;
				break;
			case Direction.Forward:
				normal = Vector3.forward;
				break;
			case Direction.Backward:
				normal = Vector3.back;
				break;
			default:
				Debug.LogErrorFormat("Direction \"{0}\" has no unit vector defined", dir.ToString());
				normal = Vector3.up;
				break;
		}

		return normal;
	}
}