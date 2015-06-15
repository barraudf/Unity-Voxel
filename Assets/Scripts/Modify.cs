using UnityEngine;
using System.Collections.Generic;

public class Modify : MonoBehaviour
{
    Vector2 rot;
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk != null && Input.GetMouseButtonDown(0))
                    World.SetBlock(hit, new BlockAir());
            else if (chunk != null && Input.GetMouseButtonDown(1))
                    World.SetBlock(hit, new BlockGrass(), true);
        }

        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 3,
            rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
    }
}
