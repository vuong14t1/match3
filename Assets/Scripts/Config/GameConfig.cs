using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : Singleton<GameConfig>
{
    public TextAsset levelConfigs;
    public LevelConfigs LevelConfigs;

    private void Awake()
    {
        base.Awake();    
        loadLevelConfig();
    }

    public void loadLevelConfig()
    {
        LevelConfigs = JsonUtility.FromJson<LevelConfigs>(levelConfigs.text);
    }

    public LevelConfig GetLevelConfig(int level)
    {
        foreach (LevelConfig levelConfig in LevelConfigs.levelConfigs)
        {
            if (levelConfig.level == level)
            {
                return levelConfig;
            }
        }

        return null;
    }
}
