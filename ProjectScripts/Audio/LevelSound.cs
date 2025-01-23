using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSound : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "main deneeme")
        {
            AudioManager.Instance.PlayLevelMusic();
        }
    }
}