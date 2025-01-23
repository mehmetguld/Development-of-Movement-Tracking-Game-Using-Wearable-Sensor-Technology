using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance for global access
    public AudioSource audioSource; // The audio source component for playing music
    public List<AudioClip> levelMusics = new List<AudioClip>(); // List of level-specific background music
    public AudioClip mainMenuMusic; // Background music for the main menu

    private int selectedMusicIndex = 0; // The index of the currently selected music
    private string currentScene; // The name of the currently loaded scene

    private void Awake()
    {
        // Initialize the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the AudioManager from being destroyed when changing scenes
            currentScene = SceneManager.GetActiveScene().name; // Get the name of the currently active scene
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManager instances
            return;
        }

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load the selected music index from player preferences
        selectedMusicIndex = PlayerPrefs.GetInt("SelectedMusicIndex", 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the current scene name
        currentScene = scene.name;

        // Play main menu music if the current scene is the main menu
        if (currentScene == "main deneeme")
        {
            if (audioSource.clip != mainMenuMusic)
            {
                PlayMainMenuMusic();
            }
        }
    }

    public void SelectMusic(int index)
    {
        // Change the selected music and save the choice
        if (index >= 0 && index < levelMusics.Count)
        {
            selectedMusicIndex = index;
            PlayerPrefs.SetInt("SelectedMusicIndex", index);

            // Play the level music if the current scene is not the main menu
            if (currentScene != "main deneeme")
            {
                PlayLevelMusic();
            }
        }
    }

    public void PlayMainMenuMusic()
    {
        // Play the main menu background music
        if (mainMenuMusic != null)
        {
            audioSource.clip = mainMenuMusic;
            audioSource.Play();
        }
    }

    public void PlayLevelMusic()
    {
        // Play the selected level background music
        if (levelMusics.Count > selectedMusicIndex)
        {
            audioSource.clip = levelMusics[selectedMusicIndex];
            audioSource.Play();
        }
    }
}
