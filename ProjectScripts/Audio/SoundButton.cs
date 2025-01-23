// Button controller for the UI
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public int musicIndex; // Will be assigned in the Inspector

    void Start()
    {
        Button button = GetComponent<Button>(); // Get the Button component attached to this GameObject
    }

    // Function called when a music is selected
    public void OnMusicSelected()
    {
        AudioManager.Instance.SelectMusic(musicIndex); // Calls the SelectMusic function in the AudioManager with the music index
    }
}
