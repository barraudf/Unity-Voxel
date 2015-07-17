using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
	public float MoveSpeed = 10f;

	private CharacterController CharacterController;

	private void Awake()
	{
		CharacterController = GetComponent<CharacterController>();
	}

	public void Move(Vector3 moveVector)
	{
		SnapAlignCharacterWithCamera(moveVector);
		ProcessMotion(moveVector);
	}

	private void ProcessMotion(Vector3 moveVector)
	{
		moveVector = transform.TransformDirection(moveVector);

		if(moveVector.magnitude > 1)
			moveVector = Vector3.Normalize(moveVector);

		moveVector *= MoveSpeed;

		moveVector *= Time.deltaTime;

		CharacterController.Move(moveVector);
	}

	private void SnapAlignCharacterWithCamera(Vector3 moveVector)
	{
		if(moveVector.x != 0 || moveVector.z != 0)
		{
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}
}
