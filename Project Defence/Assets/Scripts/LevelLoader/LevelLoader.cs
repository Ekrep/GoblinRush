using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader instance;
    public static LevelLoader Instance => instance;
    [SerializeField] private LevelsData levelsData;

    private int currentLevelIndex = 0;
    public int CurrentLevelIndex => currentLevelIndex;

    public static event Action OnLevelLoaded;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        LoadLevel(2);
    }
    public void LoadLevel(int index)
    {

        currentLevelIndex = index;
        GridMap.Instance.LoadMap(levelsData.levels[currentLevelIndex]);

    }
    public void LoadNextLevel()
    {
        currentLevelIndex = (currentLevelIndex + 1) % levelsData.levels.Count;
        GridMap.Instance.LoadMap(levelsData.levels[currentLevelIndex]);
    }
    public void RestartLevel()
    {
        GridMap.Instance.LoadMap(levelsData.levels[currentLevelIndex]);
    }

    public void LevelLoaded()
    {
        if (OnLevelLoaded != null)
        {
            OnLevelLoaded();
        }

    }
}
