using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectGun : MonoBehaviour
{
    public projectship projectship; // Reference to the projectship object
    public mpu6050 mpu6050; // Reference to the MPU6050 sensor

    private bool hasStarted = false; // Will be activated after receiving the first data
    private int previousButtonState = 1; // Previous state of the button

    void Start()
    {
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // Find the MPU6050 sensor reference
        }
        if (projectship == null)
        {
            projectship = FindObjectOfType<projectship>(); // Find the projectship reference
        }
    }

    void Update()
    {
        // Check if the first data has been received
        if (!hasStarted && mpu6050.buttonData.x != 0)
        {
            hasStarted = true; // Activate when the first data is received
            return;
        }

        if (hasStarted)
        {
            int currentButtonState = (int)mpu6050.buttonData.x;

            // If the button state changed and is 0, shoot
            if (previousButtonState == 1 && currentButtonState == 0)
            {
                projectship.Shoot(); // Call the Shoot method of projectship
            }

            // Update the previous button state
            previousButtonState = currentButtonState;
        }
    }
}
