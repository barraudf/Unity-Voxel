using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(ThirdPersonAnimator))]
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonUserControl : MonoBehaviour
{
	private ThirdPersonController ThirdPersonController;
	private ThirdPersonAnimator ThirdPersonAnimator;

	private void Awake()
	{
		ThirdPersonController = GetComponent<ThirdPersonController>();
		ThirdPersonAnimator = GetComponent<ThirdPersonAnimator>();
    }

	private void FixedUpdate()
	{
		if (!Camera.main)
			return;

		GetLocomotionInput();
		HandleActionInput();

		ThirdPersonController.Move();
	}

	private void GetLocomotionInput()
	{
		float deadZone = 0.1f;

		ThirdPersonController.VerticalVelocity = ThirdPersonController.MoveVector.y;

		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		if(v <= deadZone && v >= -deadZone)
			v = 0f;
		if (h <= deadZone && h >= -deadZone)
			h = 0f;

		ThirdPersonController.MoveVector = new Vector3(h, 0, v);
	}

	private void HandleActionInput()
	{
		if (CrossPlatformInputManager.GetButton("Jump"))
			Jump();
	}

	private void Jump()
	{
		ThirdPersonController.Jump();
		ThirdPersonAnimator.Jump();
    }
}
