using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;

    private Vector3 moveDirection;
    private float rotateX, rotateY;

    void Update()
    {
        // Move camera forward and backward
        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        else if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;

        // Move camera left and right
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        else if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;

        // Rotate camera using mouse
        if (Input.GetMouseButton(1))
        {
            rotateX += Input.GetAxis("Mouse X") * rotateSpeed;
            rotateY += Input.GetAxis("Mouse Y") * rotateSpeed;
            rotateY = Mathf.Clamp(rotateY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(-rotateY, rotateX, 0f);
        }
    }

    void FixedUpdate()
    {
        // Move camera
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        moveDirection = Vector3.zero;
    }
}
