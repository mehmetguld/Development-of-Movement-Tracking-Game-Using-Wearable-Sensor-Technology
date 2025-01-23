using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardController : MonoBehaviour
{
    public GameObject keyboard; // Keyboard object
    public GameObject menu; // Menu object
    private bool isKeyboardVisible = false; // Flag to track if the keyboard is visible

    void Update()
    {
        // Check the currently selected UI element
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null)
        {
            // Check for a normal InputField
            InputField inputField = currentSelected.GetComponent<InputField>();
            // Check for a TextMeshPro InputField
            TMPro.TMP_InputField tmpInputField = currentSelected.GetComponent<TMPro.TMP_InputField>();

            // If the selected element is an input field and it is interactable
            if ((inputField != null && inputField.interactable) ||
                (tmpInputField != null && tmpInputField.interactable))
            {
                // Additional logic can be added here
            }
        }
    }

    // Function to show the keyboard and hide the menu
    public void ShowKeyboard()
    {
        keyboard.SetActive(true); // Activates the keyboard
        menu.SetActive(false); // Deactivates the menu
    }

    // Function to hide the keyboard and show the menu
    public void HideKeyboard()
    {
        keyboard.SetActive(false); // Deactivates the keyboard
        menu.SetActive(true); // Activates the menu
    }
}
