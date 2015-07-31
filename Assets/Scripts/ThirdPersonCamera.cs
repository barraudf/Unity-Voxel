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
	public float OcclusionDistanceStep = 0.5f;
	public int MaxOcclusionChecks = 10;
	public float AbsoluteMaxDistance = 0.25f;
	public float DistanceResumeSmooth = 1f;

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
	private Camera Camera;
	private float distanceSmooth = 0f;
	private float preOccludedDistance = 0f;

	private void Start()
	{
		Camera = GetComponent<Camera>();
		Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
		StartDistance = Distance;
		Reset();
	}

	private void LateUpdate()
	{
		if (!TargetLookAt)
			return;

		HandlePlayerInput();

		int count = 0;
		do
		{
			CalculateDesiredPosition();
			count++;
		}
		while (CheckIfOccluded(count));

		UpdatePosition();
	}

	public void Reset()
	{
		MouseX = 0;
		MouseY = 10;
		Distance = StartDistance;
		DesiredDistance = Distance;
		preOccludedDistance = Distance;
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
		{
			DesiredDistance = Mathf.Clamp(Distance - CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, DistanceMin, DistanceMax);
			preOccludedDistance = DesiredDistance;
			distanceSmooth = DistanceSmooth;
		}
	}

	private void CalculateDesiredPosition()
	{
		ResetDesiredDistance();
		Distance = Mathf.SmoothDamp(Distance, DesiredDistance, ref velDistance, distanceSmooth);

		DesiredPosition = CalculatePosition(MouseY, MouseX, Distance);
	}

	private Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

		return TargetLookAt.position + rotation * direction;
	}

	private bool CheckIfOccluded(int count)
	{
		bool isOccluded = false;

		float nearestDistance = CheckCameraPoints(TargetLookAt.position, DesiredPosition);

		if (nearestDistance != -1f)
		{
			if(count < MaxOcclusionChecks)
			{
				isOccluded = true;
				Distance -= OcclusionDistanceStep;

				if (Distance < AbsoluteMaxDistance)
					Distance = AbsoluteMaxDistance;
			}
			else
			{
				Distance = nearestDistance - Camera.nearClipPlane;
			}

			DesiredDistance = Distance;
			distanceSmooth = DistanceResumeSmooth;
		}

		return isOccluded;
	}

	private float CheckCameraPoints(Vector3 from, Vector3 to)
	{
		float nearestDistance = -1f;

		RaycastHit hitInfo;

		ClipPlanePoints clipPlanePoints = ClipPlaneAtNear(to);

		Debug.DrawLine(from, to + transform.forward * -Camera.nearClipPlane, Color.red);
		Debug.DrawLine(from, clipPlanePoints.LowerLeft, Color.blue);
		Debug.DrawLine(from, clipPlanePoints.LowerRight, Color.blue);
		Debug.DrawLine(from, clipPlanePoints.UpperLeft, Color.blue);
		Debug.DrawLine(from, clipPlanePoints.UpperRight, Color.blue);

		Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight, Color.blue);
		Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.LowerRight, Color.blue);
		Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight, Color.blue);
		Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.LowerLeft, Color.blue);

		if (Physics.Linecast(from, clipPlanePoints.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
			nearestDistance = hitInfo.distance;

		if (Physics.Linecast(from, clipPlanePoints.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player")
			if (hitInfo.distance < nearestDistance || nearestDistance == -1f)
				nearestDistance = hitInfo.distance;

		if (Physics.Linecast(from, clipPlanePoints.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player")
			if (hitInfo.distance < nearestDistance || nearestDistance == -1f)
				nearestDistance = hitInfo.distance;

		if (Physics.Linecast(from, clipPlanePoints.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player")
			if (hitInfo.distance < nearestDistance || nearestDistance == -1f)
				nearestDistance = hitInfo.distance;

		if (Physics.Linecast(from, to + transform.forward * -Camera.nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player")
			if (hitInfo.distance < nearestDistance || nearestDistance == -1f)
				nearestDistance = hitInfo.distance;


		return nearestDistance;
	}

	private void ResetDesiredDistance()
	{
		if(DesiredDistance < preOccludedDistance)
		{
			Vector3 pos = CalculatePosition(MouseY, MouseX, preOccludedDistance);

			float nearestDistance = CheckCameraPoints(TargetLookAt.position, pos);

			if(nearestDistance == -1f || nearestDistance > preOccludedDistance)
			{
				DesiredDistance = preOccludedDistance;
			}
		}
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

	private static ClipPlanePoints ClipPlaneAtNear(Vector3 position)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints();

		if (!Camera.main)
			return clipPlanePoints;

		Transform transform = Camera.main.transform;
		float halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		float distance = Camera.main.nearClipPlane;
		float height = Mathf.Tan(halfFOV) * distance;
		float width = height * aspect;

		clipPlanePoints.LowerRight = position + transform.right * width;
		clipPlanePoints.LowerRight -= transform.up * height;
		clipPlanePoints.LowerRight += transform.forward * distance;

		clipPlanePoints.LowerLeft = position - transform.right * width;
		clipPlanePoints.LowerLeft -= transform.up * height;
		clipPlanePoints.LowerLeft += transform.forward * distance;

		clipPlanePoints.UpperRight = position + transform.right * width;
		clipPlanePoints.UpperRight += transform.up * height;
		clipPlanePoints.UpperRight += transform.forward * distance;

		clipPlanePoints.UpperLeft = position - transform.right * width;
		clipPlanePoints.UpperLeft += transform.up * height;
		clipPlanePoints.UpperLeft += transform.forward * distance;
		
		return clipPlanePoints;
	}

	private struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}
}
