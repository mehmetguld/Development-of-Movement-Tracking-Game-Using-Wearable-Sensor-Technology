using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasInputClick : MonoBehaviour
{
    public mpu6050 mpu6050; // Reference to the MPU6050 sensor
    private GraphicRaycaster raycaster; // To cast ray for UI interaction
    private EventSystem eventSystem; // Event system for handling events
    private PointerEventData pointerEventData; // Pointer event data for raycasting
    private Canvas canvas; // Reference to the canvas

    void Start()
    {
        // Get the reference to the MPU6050 sensor
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // Find the MPU6050 object
        }

        // Get the Canvas components
        canvas = FindObjectOfType<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>(); // Get the GraphicRaycaster
        eventSystem = FindObjectOfType<EventSystem>(); // Get the EventSystem
        pointerEventData = new PointerEventData(eventSystem); // Initialize pointer event data
    }

    void Update()
    {
        // Convert the mouse object's position to screen coordinates
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        pointerEventData.position = screenPosition;

        // Check UI elements under the pointer
        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        // If the pointer is on a UI element and the Arduino button is pressed
        if (results.Count > 0 && mpu6050.buttonData.x == 0)
        {
            foreach (RaycastResult result in results)
            {
                // Button control
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null && button.interactable)
                {
                    button.onClick.Invoke(); // Trigger the button click event
                    StartCoroutine(ButtonClickDelay()); // Wait before triggering again
                    return;
                }

                // Input Field control
                InputField inputField = result.gameObject.GetComponent<InputField>();
                if (inputField != null && inputField.interactable)
                {
                    // Select and activate the input field
                    inputField.Select();
                    inputField.ActivateInputField();

                    // Move the cursor to the end of the input field
                    inputField.caretPosition = inputField.text.Length;

                    StartCoroutine(ButtonClickDelay());
                    return;
                }

                // TMP_InputField control (for TextMeshPro)
                TMPro.TMP_InputField tmpInputField = result.gameObject.GetComponent<TMPro.TMP_InputField>();
                if (tmpInputField != null && tmpInputField.interactable)
                {
                    // Select and activate the TMP input field
                    tmpInputField.Select();
                    tmpInputField.ActivateInputField();

                    // Move the cursor to the end of the TMP input field
                    tmpInputField.caretPosition = tmpInputField.text.Length;

                    StartCoroutine(ButtonClickDelay());
                    return;
                }
            }
        }
    }

    // Coroutine to delay the button click action
    System.Collections.IEnumerator ButtonClickDelay()
    {
        yield return new WaitForSeconds(0.2f); // Wait for 200ms before the next interaction
    }
}
