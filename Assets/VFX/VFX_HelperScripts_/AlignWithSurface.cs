using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithSurface : MonoBehaviour
{
    public float raycastDistance = 10f;
    public Vector3 offset = new Vector3(0, 2.0f, 0);
    public Vector3 vfxoffset = new Vector3(0, 0.1f, 0);

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, -Vector3.up, out hit, raycastDistance))
        {
            // Align the GameObject with the surface normal
            transform.up = hit.normal;
            transform.position = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z) + vfxoffset;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}