using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonUserControl : MonoBehaviour
{
	private ThirdPersonController ThirdPersonController;

	private void Awake()
	{
		ThirdPersonController = GetComponent<ThirdPersonController>();
	}

	private void FixedUpdate()
	{
		if (!Camera.main)
			return;

		Vector3 moveVector = GetLocomotionInput();

		ThirdPersonController.Move(moveVector);
	}

	private Vector3 GetLocomotionInput()
	{
		float deadZone = 0.1f;

		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		if(v <= deadZone && v >= -deadZone)
			v = 0f;
		if (h <= deadZone && h >= -deadZone)
			h = 0f;

		Vector3 moveVector = new Vector3(h, 0, v);

		return moveVector;
	}
}
