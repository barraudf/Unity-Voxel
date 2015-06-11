using UnityEngine;
using System.Collections.Generic;

public class Modify : MonoBehaviour
{
    Vector2 rot;

    private Block lastBlockHit = null;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk != null && Input.GetMouseButtonDown(0))
                    World.SetBlock(hit, new BlockAir(chunk));
            else if (chunk != null && Input.GetMouseButtonDown(1))
                    World.SetBlock(hit, new BlockGrass(chunk), true);
            else
            {
                Block b = World.GetBlock(hit);
                if(b != null && b != lastBlockHit)
                {
                    b.SetHighlight(true);

                    if(lastBlockHit != null)
                        lastBlockHit.SetHighlight(false);

                    lastBlockHit = b;
                }
            }
        }
        else if(lastBlockHit != null)
        {
            lastBlockHit.SetHighlight(false);
            lastBlockHit = null;
        }

        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 7,
            rot.y + Input.GetAxis("Mouse Y") * 7);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * Input.GetAxis("Vertical");
        transform.position += transform.right * Input.GetAxis("Horizontal");
    }
}
