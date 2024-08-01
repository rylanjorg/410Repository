using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public bool rotateOnZ = false;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
