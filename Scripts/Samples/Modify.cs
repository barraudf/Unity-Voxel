using UnityEngine;
using System.Collections.Generic;


public class Modify : MonoBehaviour
{
	public float MovementInputModifier = 3f;
	public float MouseInputModifier = 5f;

	private	Vector2 _Rotation;

    private void Update()
    {
        _Rotation = new Vector2(
            _Rotation.x + Input.GetAxis("Mouse X") * MouseInputModifier * Time.deltaTime,
            _Rotation.y + Input.GetAxis("Mouse Y") * MouseInputModifier * Time.deltaTime);

        transform.localRotation = Quaternion.AngleAxis(_Rotation.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(_Rotation.y, Vector3.left);

        transform.position += transform.forward * MovementInputModifier * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += transform.right * MovementInputModifier * Input.GetAxis("Horizontal") * Time.deltaTime;

		if (Input.GetKey(KeyCode.E))
			transform.position += Vector3.up * MovementInputModifier * Time.deltaTime;
		if (Input.GetKey(KeyCode.A))
			transform.position -= Vector3.up * MovementInputModifier * Time.deltaTime;
	}
}
