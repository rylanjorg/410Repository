using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float speed = 2f; // Adjust speed
    public float radius = 2f; // Adjust radius
    public float minSpeed = 1f; // Minimum speed
    public float maxSpeed = 3f; // Maximum speed
    public float accelerationChangeInterval = 1f; // Interval to change acceleration

    private float angle = 0f;
    private float acceleration = 0f;
    private float timeSinceLastAccelerationChange = 0f;

    void Update()
    {
        // Update the time since the last acceleration change
        timeSinceLastAccelerationChange += Time.deltaTime;

        // If it's time to change the acceleration
        if (timeSinceLastAccelerationChange >= accelerationChangeInterval)
        {
            // Reset the timer
            timeSinceLastAccelerationChange = 0f;

            // Randomly change the acceleration
            acceleration = Random.Range(-1f, 1f);
        }

        // Update the speed based on the acceleration
        speed += acceleration * Time.deltaTime;

        // Clamp the speed between the minimum and maximum speeds
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        // Update the angle based on time and speed
        angle += speed * Time.deltaTime;

        // Calculate the new position on the circle
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Set the object's position
        transform.position = new Vector3(x, 0f, y);
    }
}