using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string sceneName;  // Define the name of the scene to be loaded

    // Function to change the scene
    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);  // Load the specified scene
    }
}
