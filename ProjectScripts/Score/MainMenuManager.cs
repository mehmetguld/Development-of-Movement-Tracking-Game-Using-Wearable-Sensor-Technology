using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] levelObjects; // Array of objects for each level (3 objects in this case)
    private List<TextMeshProUGUI[]> levelTextFields = new List<TextMeshProUGUI[]>(); // List of TextMeshPro arrays for each object

    public TMP_InputField nameInput; // Input field for entering the player's name
    public Button[] buttons; // Array of buttons
    public string level1 = "Level1"; // Level 1 identifier
    public string level2 = "Level2"; // Level 2 identifier
    public string level3 = "Level3"; // Level 3 identifier

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the script is enabled
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event when the script is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the "main deneeme" scene is loaded and the nameInput field is null, find the input field again
        if (scene.name == "main deneeme")
        {
            // Find the InputField in the UI
            nameInput = FindObjectOfType<TMP_InputField>();

            // Add a listener to save the name when the user finishes editing
            nameInput.onEndEdit.AddListener((string text) => Score.Instance.SaveName());

            // Add listeners to each button for specific actions
            buttons[0].onClick.AddListener(() => Score.Instance.SaveName()); // Button 1: SaveName
            buttons[1].onClick.AddListener(() => Score.Instance.SaveLevel(level1)); // Button 2: SaveLevel(Level1)
            buttons[2].onClick.AddListener(() => Score.Instance.SaveLevel(level2)); // Button 3: SaveLevel(Level2)
            buttons[3].onClick.AddListener(() => Score.Instance.SaveLevel(level3)); // Button 4: SaveLevel(Level3)
        }
    }

    public void UpdateData()
    {
        // Clear the list of TextMeshPro fields
        levelTextFields.Clear();

        // Collect TextMeshPro components for each object
        foreach (var obj in levelObjects)
        {
            // Get TextMeshPro components inside the object
            TextMeshProUGUI[] textFields = obj.GetComponentsInChildren<TextMeshProUGUI>();
            levelTextFields.Add(textFields);
        }

        // Load scores and update the UI
        Score.Instance.LoadScores();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        // Iterate through levels and display sorted scores
        for (int levelIndex = 0; levelIndex < levelObjects.Length; levelIndex++)
        {
            string currentLevel = $"Level{levelIndex + 1}"; // Example: Level1, Level2
            List<Score.PlayerScore> levelScores = Score.Instance.GetScoresForLevel(currentLevel);

            // Sort the scores in descending order and take the top 30
            var sortedScores = levelScores
                .OrderByDescending(s => s.score)
                .Take(30)
                .ToList();

            // Update the TextMeshPro components with the scores
            for (int i = 0; i < levelTextFields[levelIndex].Length; i++)
            {
                if (i < sortedScores.Count)
                {
                    var playerScore = sortedScores[i];
                    levelTextFields[levelIndex][i].text = $"     {i + 1}. {playerScore.playerName} - {playerScore.score}";
                }
                else
                {
                    levelTextFields[levelIndex][i].text = $"     {i + 1}."; // Leave blank if there is no score
                }
            }
        }
    }
}
