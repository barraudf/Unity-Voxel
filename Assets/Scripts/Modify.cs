using UnityEngine;
using System.Collections.Generic;

public class Modify : MonoBehaviour
{
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

        transform.position += transform.forward * Input.GetAxis("Vertical");
        transform.position += transform.right * Input.GetAxis("Horizontal");
    }
}
