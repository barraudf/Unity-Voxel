using UnityEngine;
using System.Collections;

public class ThirdPersonAnimator : MonoBehaviour
{
	public enum Direction
	{
		Stationary,
		Forward,
		Backward,
		Left,
		Right,
		LeftForward,
		RightForward,
		LeftBackward,
		RightBackward
	}

	public Direction MoveDirection;


	public void DetermineCurrentMoveDirection(Vector3 moveVector)
	{
		bool forward = false;
		bool backward = false;
		bool left = false;
		bool right = false;

		if (moveVector.x < 0)
			left = true;
		else if (moveVector.x > 0)
			right = true;

		if (moveVector.z < 0)
			backward = true;
		else if (moveVector.z > 0)
			forward = true;

		if (forward)
		{
			if (right)
				MoveDirection = Direction.RightForward;
			else if (left)
				MoveDirection = Direction.LeftForward;
			else
				MoveDirection = Direction.Forward;
		}
		else if (backward)
		{
			if (right)
				MoveDirection = Direction.RightBackward;
			else if (left)
				MoveDirection = Direction.LeftBackward;
			else
				MoveDirection = Direction.Backward;
		}
		else if (left)
			MoveDirection = Direction.Left;
		else if (right)
			MoveDirection = Direction.Right;
		else
			MoveDirection = Direction.Stationary;
	}
}
