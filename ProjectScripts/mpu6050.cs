using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class mpu6050 : MonoBehaviour
{
    private static mpu6050 instance; // Ensures a single instance

    SerialPort stream = new SerialPort("COM5", 9600); // Serial port
    public string strReceived;

    public float qw, qx, qy, qz;
    public Vector3 sensorData;
    public Vector2 buttonData;

    private bool isReading = false; // Ensures the coroutine runs only once

    void Awake()
    {
        // Singleton structure to ensure only one instance of this script
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction across scenes
        }
        else
        {
            Destroy(gameObject); // Destroys new instance if one already exists
        }
    }

    void Start()
    {
        if (!stream.IsOpen)
        {
            stream.Open(); // Open serial stream
        }

        if (!isReading)
        {
            StartCoroutine(ReadSerialData());
            isReading = true;
        }
    }

    IEnumerator ReadSerialData()
    {
        while (stream.IsOpen)
        {
            try
            {
                strReceived = stream.ReadLine(); // Read incoming data
                string[] strData = strReceived.Split(','); // Split data by commas
                if (strData.Length >= 5)
                {
                    // Parse quaternion data
                    qw = float.Parse(strData[0]);
                    qx = float.Parse(strData[1]);
                    qy = float.Parse(strData[2]);
                    qz = float.Parse(strData[3]);

                    // Parse button states
                    int button1State = int.Parse(strData[4]);

                    qx = ProcessValue(qx);
                    qy = ProcessValue(qy);
                    qz = ProcessValue(qz);

                    sensorData = new Vector3(qx, qy, qz);
                    buttonData = new Vector2(button1State, 0);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading data: " + e.Message);
            }

            yield return null; // Runs once per frame
        }
    }

    private float ProcessValue(float value)
    {
        if (value > 0.2f) return 0.6f;
        else if (value < -0.2f) return -0.6f;
        else return 0f;
    }

    void OnApplicationQuit()
    {
        if (stream.IsOpen)
        {
            stream.Close(); // Close serial port on application quit
        }
    }
}
