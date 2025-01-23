using UnityEngine;
using TMPro;

public class keyboard : MonoBehaviour
{
    public TMP_InputField inputField; // Reference to the TMP_InputField
    public string letter; // Letter to be written for each button


    // Function called when the button is clicked
    public void OnButtonClick()
    {
        inputField.text += letter; // Adds the letter to the TMP_InputField
    }

    // Function to clear the InputField
    public void ClearInputField()
    {
        inputField.text = ""; // Clears the content of the InputField
    }

    // Function to delete the last entered letter
    public void DeleteLastLetter()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1); // Deletes the last letter
        }
    }
}
