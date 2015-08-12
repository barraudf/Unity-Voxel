using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(ThirdPersonController))]
public class ThirdPersonAnimator : MonoBehaviour
{
	//public enum Direction
	//{
	//	Stationary,
	//	Forward,
	//	Backward,
	//	Left,
	//	Right,
	//	LeftForward,
	//	RightForward,
	//	LeftBackward,
	//	RightBackward
	//}

	//public Direction MoveDirection;


	//public void DetermineCurrentMoveDirection(Vector3 moveVector)
	//{
	//	bool forward = false;
	//	bool backward = false;
	//	bool left = false;
	//	bool right = false;

	//	if (moveVector.x < 0)
	//		left = true;
	//	else if (moveVector.x > 0)
	//		right = true;

	//	if (moveVector.z < 0)
	//		backward = true;
	//	else if (moveVector.z > 0)
	//		forward = true;

	//	if (forward)
	//	{
	//		if (right)
	//			MoveDirection = Direction.RightForward;
	//		else if (left)
	//			MoveDirection = Direction.LeftForward;
	//		else
	//			MoveDirection = Direction.Forward;
	//	}
	//	else if (backward)
	//	{
	//		if (right)
	//			MoveDirection = Direction.RightBackward;
	//		else if (left)
	//			MoveDirection = Direction.LeftBackward;
	//		else
	//			MoveDirection = Direction.Backward;
	//	}
	//	else if (left)
	//		MoveDirection = Direction.Left;
	//	else if (right)
	//		MoveDirection = Direction.Right;
	//	else
	//		MoveDirection = Direction.Stationary;
	//}

	public enum CharacterStates
	{
		Idle,
		Running,
		Jumping,
		Falling,
		Landing,
		Using,
		Dead,
		ActionLocked
	}

	public CharacterStates State;
	public bool IsDead; 

	private ThirdPersonController _Controller;
	private Animation _Animation;
	private CharacterStates _LastState;
	//private Animator _Animator;

	private void Start()
	{
		_Controller = GetComponent<ThirdPersonController>();
		_Animation = GetComponent<Animation>();
		//_Animator = GetComponent<Animator>();
	}
	private void Update()
	{
		//_Animator.SetBool("MovingH", _Controller.MoveVector.x != 0 || _Controller.MoveVector.z != 0);
		//_Animator.SetBool("MovingV", _Controller.MoveVector.y != 0);
		//_Animator.SetBool("isGrounded", _Controller.CharacterController.isGrounded);
		DetermineCurrentState();
		ProcessCurrentState();
		//Debug.Log("Current Character state: " + State.ToString());
	}

	private void DetermineCurrentState()
	{
		if (State == CharacterStates.Dead)
			return;

		if(!_Controller.CharacterController.isGrounded)
		{
			if (State != CharacterStates.Falling && State != CharacterStates.Jumping && State != CharacterStates.Landing)
			{
				Fall();
			}
		}

		if(State != CharacterStates.Falling &&
			State != CharacterStates.Jumping &&
			State != CharacterStates.Landing &&
			State != CharacterStates.Using)
		{
			if (_Controller.MoveVector.x == 0 && _Controller.MoveVector.z == 0)
				State = CharacterStates.Idle;
			else
				State = CharacterStates.Running;
		}
	}

	private void ProcessCurrentState()
	{
		switch(State)
		{
			case CharacterStates.Idle:
				Idling();
				break;
			case CharacterStates.Running:
				Running();
				break;
			case CharacterStates.Jumping:
				Jumping();
				break;
			case CharacterStates.Falling:
				Falling();
				break;
			case CharacterStates.Landing:
				break;
			case CharacterStates.Using:
				break;
			case CharacterStates.Dead:
				break;
			case CharacterStates.ActionLocked:
				break;
		}
	}

	#region Character states methods
	private void Idling()
	{
		_Animation.CrossFade("Idle");
	}

	private void Running()
	{
		_Animation.CrossFade("Run");
	}

	private void Jumping()
	{
		if( (!_Animation.isPlaying && _Controller.CharacterController.isGrounded) ||
				_Controller.CharacterController.isGrounded)
		{
			if (_Controller.MoveVector.x == 0 && _Controller.MoveVector.z == 0)
				State = CharacterStates.Idle;
			else
				State = CharacterStates.Running;
		}
		else if(!_Animation.IsPlaying("Jump"))
		{
			State = CharacterStates.Falling;
			_Animation.CrossFade("Falling");
		}
		else
		{
			State = CharacterStates.Jumping;
		}
	}

	private void Falling()
	{
		if(_Controller.CharacterController.isGrounded)
		{
			if (_Controller.MoveVector.x == 0 && _Controller.MoveVector.z == 0)
				State = CharacterStates.Idle;
			else
				State = CharacterStates.Running;
		}
	}
	#endregion Character states methods

	#region Start Action Method
	public void Jump()
	{
		if (!_Controller.CharacterController.isGrounded || IsDead || State == CharacterStates.Jumping)
			return;

		_LastState = State;
		State = CharacterStates.Jumping;
		_Animation.CrossFade("Jump");
	}

	public void Fall()
	{
		if (IsDead)
			return;

		_LastState = State;
		State = CharacterStates.Falling;
		_Animation.CrossFade("Falling");
	}
	#endregion Start Action Method
}
