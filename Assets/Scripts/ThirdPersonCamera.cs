using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonCamera : MonoBehaviour
{
	public Transform TargetLookAt;
	public float Distance = 5f;
	public float DistanceMin = 3f;
	public float DistanceMax = 10f;
	public float DistanceSmooth = 0.05f;
	public float X_MouseSensitivity = 5f;
	public float Y_MouseSensitivity = 5f;
	public float MouseWheelSensitivity = 5f;
	public float Y_MinLimit = -40f;
	public float Y_MaxLimit = 90f;
	public float X_Smooth = 0.05f;
	public float Y_Smooth = 0.1f;


	private float MouseX = 0f;
	private float MouseY = 0f;
	private float velDistance = 0f;
	private float StartDistance = 0f;
	private float DesiredDistance = 0f;
	private Vector3 DesiredPosition = Vector3.zero;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private Vector3 Position = Vector3.zero;

	private void Start()
	{
		Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
		StartDistance = Distance;
		Reset();
	}

	private void LateUpdate()
	{
		if (!TargetLookAt)
			return;

		HandlePlayerInput();
		CalculateDesiredPosition();
		UpdatePosition();
	}

	public void Reset()
	{
		MouseX = 0;
		MouseY = 10;
		Distance = StartDistance;
		DesiredDistance = Distance;
	}

	private void HandlePlayerInput()
	{
		float deadZone = 0.001f;

		if (CrossPlatformInputManager.GetButton("Fire2"))
		{
			MouseX += CrossPlatformInputManager.GetAxis("Mouse X") * X_MouseSensitivity;
			MouseY -= CrossPlatformInputManager.GetAxis("Mouse Y") * Y_MouseSensitivity;

			MouseY = ClampAngle(MouseY, Y_MinLimit, Y_MaxLimit);
			MouseX = ClampAngle(MouseX);
		}

		if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < -deadZone || CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > deadZone)
			DesiredDistance = Mathf.Clamp(Distance - CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, DistanceMin, DistanceMax);
	}

	private void CalculateDesiredPosition()
	{
		Distance = Mathf.SmoothDamp(Distance, DesiredDistance, ref velDistance, DistanceSmooth);

		DesiredPosition = CalculatePosition(MouseY, MouseX, Distance);
	}

	private Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

		return TargetLookAt.position + rotation * direction;
	}



	private void UpdatePosition()
	{
		float posX = Mathf.SmoothDamp(Position.x, DesiredPosition.x, ref velX, X_Smooth);
		float posY = Mathf.SmoothDamp(Position.y, DesiredPosition.y, ref velY, Y_Smooth);
		float posZ = Mathf.SmoothDamp(Position.z, DesiredPosition.z, ref velZ, X_Smooth);

		Position = new Vector3(posX, posY, posZ);
		
		transform.position = Position;
		transform.LookAt(TargetLookAt.transform.position);
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		angle = ClampAngle(angle);

		return Mathf.Clamp(angle, min, max);
	}

	private static float ClampAngle(float angle)
	{
		do
		{
			if (angle < -180)
				angle += 360;
			if (angle > 180)
				angle -= 360;
		}
		while (angle < -180 || angle > 180);

		return angle;
	}
}
