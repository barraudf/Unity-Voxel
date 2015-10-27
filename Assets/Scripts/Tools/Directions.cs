using UnityEngine;

public enum Direction
{
	/// <summary>YPos</summary>
	Up			= 0,

	/// <summary>YNeg</summary>
	Down		= 1,

	/// <summary>XPos, East</summary>
	Right		= 2,

	/// <summary>XNeg, West</summary>
	Left		= 3,

	/// <summary>ZPos, North</summary>
	Forward		= 4,

	/// <summary>ZNeg, South</summary>
	Backward	= 5
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

	public static GridPosition ToPositionOffset(this Direction dir)
	{
		GridPosition offset;

		switch (dir)
		{
			case Direction.Up:
				offset = GridPosition.Up;
				break;
			case Direction.Down:
				offset = GridPosition.Down;
				break;
			case Direction.Right:
				offset = GridPosition.Right;
				break;
			case Direction.Left:
				offset = GridPosition.Left;
				break;
			case Direction.Forward:
				offset = GridPosition.Forward;
				break;
			case Direction.Backward:
				offset = GridPosition.Backward;
				break;
			default:
				Debug.LogErrorFormat("Direction \"{0}\" has no position offset defined", dir.ToString());
				offset = GridPosition.Up;
				break;
		}

		return offset;
	}
}