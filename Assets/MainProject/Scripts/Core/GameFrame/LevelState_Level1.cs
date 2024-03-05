using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelState_Level1 : MonoBehaviour
{
    public static Action OnGameStart;
    public static Action OnGameEnd;

    [SerializeField] private static string bestScoreKey = "Null";
    public static string bestScoreValue { get; private set; }

    private void Awake()
    {
        Time.timeScale = 0f;
        GetBestScoreRecord();
    }

    private void Start()
    {
        GameStandby();
    }

    private static void ClearGameEvent()
    {
        OnGameStart = null;
        OnGameEnd = null;
    }

    public static void GameStandby()
    {
        OnGameStart?.Invoke();
        Time.timeScale = 1f;
    }

    public static void GameEnd()
    {
        OnGameEnd?.Invoke();
        SetBestScoreRecord();
        ClearGameEvent();
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
    }

    private void GetBestScoreRecord()
    {
        if (PlayerPrefs.HasKey(bestScoreKey))
        {
            bestScoreValue = PlayerPrefs.GetString(bestScoreKey);
        }
        else
        {
            bestScoreValue = "None";
        }
    }

    private static void SetBestScoreRecord()
    {
        float currentTime = Time.timeSinceLevelLoad;

        if (!PlayerPrefs.HasKey(bestScoreKey))
        {
            string bestScore = $"{Time.timeSinceLevelLoad:F2}";
            PlayerPrefs.SetString(bestScoreKey, bestScore);
        }
        else
        {
            float bestTime = float.Parse(PlayerPrefs.GetString(bestScoreKey));
            if (currentTime < bestTime)
            {
                string bestScore = $"{Time.timeSinceLevelLoad:F2}";
                PlayerPrefs.SetString(bestScoreKey, bestScore);
            }
        }
    }
}
