using UnityEngine;

public class Mouse : MonoBehaviour
{
    public mpu6050 mpu6050; // To receive data from the MPU6050 sensor
    public float moveSpeed = 0.8f; // Movement speed
    private Vector3 sensorData; // Sensor data
    private Vector2 moveDirection; // Movement direction

    void Start()
    {
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // Find the MPU6050 object
        }
    }

    void Update()
    {
        sensorData = mpu6050.sensorData; // Get the sensor data

        // Determine the movement direction on the X and Y axes
        moveDirection = new Vector2(sensorData.y, sensorData.x); // Swap Y with X for the movement

        // If there's movement, move the mouse object
        if (moveDirection != Vector2.zero)
        {
            MoveMouse(moveDirection);
        }
    }

    // Move the mouse object
    private void MoveMouse(Vector2 direction)
    {
        Vector3 moveVector = new Vector3(direction.x, direction.y, 0).normalized * moveSpeed * Time.deltaTime;
        transform.position += moveVector; // Apply the movement
    }
}
