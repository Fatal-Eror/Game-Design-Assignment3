using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIManager : MonoBehaviour
{
    private VisualElement _root;
    private bool isLevelRuning = false;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        BindLevelState();
    }

    private void GameStart()
    {
        isLevelRuning = true;
        StartCoroutine(UpdateTimer());
        UpdateBestRecord();
    }

    private void GameEnd()
    {
        isLevelRuning= false;
        StopCoroutine(UpdateTimer());
        UnbindLevelState();
    }

    private void BindLevelState()
    {
        LevelState_Level1.OnGameStart += GameStart;
        LevelState_Level1.OnGameEnd += GameEnd;
    }

    private void UnbindLevelState()
    {
        LevelState_Level1.OnGameStart -= GameStart;
        LevelState_Level1.OnGameEnd -= GameEnd;
    }

    private IEnumerator UpdateTimer()
    {
        while (isLevelRuning)
        {
            _root.Q<Label>("Timer").text = $"Current: {Time.timeSinceLevelLoad:F2}";
            yield return null;
        }
    }

    private void UpdateBestRecord()
    {
        _root.Q<Label>("BestTime").text = $"Best: {LevelState_Level1.bestScoreValue}";
    }
}
