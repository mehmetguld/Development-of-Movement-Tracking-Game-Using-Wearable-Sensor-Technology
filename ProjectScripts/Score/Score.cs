using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static Score Instance; // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string playerName;
        public string level;
        public int score;

        public PlayerScore(string name, string level, int score)
        {
            this.playerName = name;
            this.level = level;
            this.score = score;
        }
    }

    public List<PlayerScore> playerlist = new List<PlayerScore>(); // List of player scores
    public TMP_InputField nameInput; // Input field for player's name
    private string playerNameKey = "CurrentPlayerName"; // Key for saving the player's name
    private string playerName;
    private string playerLevel;
    private int score;

    private void Start()
    {
        nameInput = FindObjectOfType<TMP_InputField>(); // Find the InputField in the scene
        LoadScores(); // Load existing scores
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the "main deneeme" scene is loaded and nameInput is null, find the InputField again
        if (scene.name == "main deneeme" && nameInput == null)
        {
            nameInput = FindObjectOfType<TMP_InputField>();
        }
    }

    public void SaveName()
    {
        PlayerPrefs.SetString(playerNameKey, nameInput.text); // Save player's name
        Debug.Log($"Name saved: {nameInput.text}");
        PlayerPrefs.Save();
        playerName = nameInput.text;
    }

    public void SaveLevel(string level)
    {
        PlayerPrefs.SetString(GetName(), level); // Save player's current level
        PlayerPrefs.Save();
        playerLevel = level;
        Debug.Log($"Level saved: {level}");
    }

    public void SavePoint(int point)
    {
        PlayerPrefs.SetInt($"{GetLevel()}_{GetName()}", point); // Save player's score
        PlayerPrefs.Save();
        score = point;
        Debug.Log($"Score saved: {point} | Player: {GetName()} | Level: {GetLevel()}");
        AddScore(GetName(), GetLevel(), score);
        SaveScores();
    }

    public void AddScore(string name, string level, int score)
    {
        // Find an existing player with the same name
        var existingPlayer = playerlist.FirstOrDefault(p => p.playerName == name);

        if (existingPlayer != null)
        {
            // Update score if the new score is higher
            if (existingPlayer.score < score)
            {
                existingPlayer.score = score;
                existingPlayer.level = level;
                Debug.Log($"Score updated: {name} | Level: {level} | New Score: {score}");
            }
        }
        else
        {
            // Add a new player
            playerlist.Add(new PlayerScore(name, level, score));
        }
    }

    public void SaveScores()
    {
        for (int i = 0; i < playerlist.Count; i++)
        {
            PlayerPrefs.SetString($"Player_{i}_Name", playerlist[i].playerName);
            PlayerPrefs.SetString($"Player_{i}_Level", playerlist[i].level);
            PlayerPrefs.SetInt($"Player_{i}_Score", playerlist[i].score);
            Debug.Log($"Player saved: {playerlist[i].playerName} | {playerlist[i].level} | {playerlist[i].score}");
        }
        PlayerPrefs.SetInt("PlayerCount", playerlist.Count);
        PlayerPrefs.Save();
        Debug.Log("All scores saved.");
    }

    public void LoadScores()
    {
        playerlist.Clear();
        int count = PlayerPrefs.GetInt("PlayerCount", 0);
        Debug.Log($"Number of players to load: {count}");

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"Player_{i}_Name");
            string level = PlayerPrefs.GetString($"Player_{i}_Level");
            int score = PlayerPrefs.GetInt($"Player_{i}_Score");
            playerlist.Add(new PlayerScore(name, level, score));
            Debug.Log($"Player loaded: {name} | {level} | {score}");
        }
    }

    public List<PlayerScore> GetScoresForLevel(string level)
    {
        return playerlist.Where(p => p.level == level).ToList(); // Get scores for a specific level
    }

    public string GetName()
    {
        string name = PlayerPrefs.GetString(playerNameKey, nameInput.text);
        Debug.Log($"Player name retrieved: {name}");
        return name;
    }

    public string GetLevel()
    {
        string level = PlayerPrefs.GetString(playerName, playerLevel);
        Debug.Log($"Level retrieved: {level}");
        return level;
    }

    public int GetPoint()
    {
        int point = PlayerPrefs.GetInt($"{playerLevel}_{playerName}", 0);
        Debug.Log($"Score retrieved: {point}");
        return point;
    }

    public void ClearAllScores()
    {
        PlayerPrefs.DeleteAll(); // Clear all saved data
        playerlist.Clear(); // Clear the player list
        Debug.Log("All scores cleared.");
    }
}
