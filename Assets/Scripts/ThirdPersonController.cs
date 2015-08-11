using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
	public float MoveSpeed = 10f;
	public float Gravity = 21f;
	public float TerminalVelocity = 20f;
	public float VerticalVelocity;
	public Vector3 MoveVector;
	public float JumpSpeed = 6f;
	public CharacterController CharacterController { get { return _CharacterController; } }

	private CharacterController _CharacterController;

	private void Awake()
	{
		_CharacterController = GetComponent<CharacterController>();
	}

	public void Move()
	{
		ProcessMotion();
		SnapAlignCharacterWithCamera();
	}

	private void ProcessMotion()
	{
		MoveVector = Camera.main.transform.TransformDirection(MoveVector);

		if (MoveVector.magnitude > 1)
			MoveVector = Vector3.Normalize(MoveVector);

		MoveVector *= MoveSpeed;

		MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
		ApplyGravity();

		_CharacterController.Move(MoveVector * Time.deltaTime);
	}

	private void SnapAlignCharacterWithCamera()
	{
		if (MoveVector.x != 0 || MoveVector.z != 0)
		{
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Rad2Deg * Mathf.Atan2(MoveVector.x, MoveVector.z), transform.eulerAngles.z);
		}
	}

	private void ApplyGravity()
	{
		if (MoveVector.y > -TerminalVelocity)
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);

		if (_CharacterController.isGrounded && MoveVector.y < -1)
			MoveVector = new Vector3(MoveVector.x, -1, MoveVector.z);
	}

	public void Jump()
	{
		if(_CharacterController.isGrounded)
		{
			VerticalVelocity = JumpSpeed;
		}
	}

}
