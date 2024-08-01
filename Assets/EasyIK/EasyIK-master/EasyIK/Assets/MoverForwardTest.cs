using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoverForwardTest : MonoBehaviour
{
    public float speed = 0.1f;
    public Vector3 direction = Vector3.forward;

    // Update is called once per frame
    void Update()
    {
        // Move the object forward along its z axis 1 unit/second.
        transform.Translate(direction * speed * Time.deltaTime);
    }
}