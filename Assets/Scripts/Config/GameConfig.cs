using System;
using System.Collections;
using System.Collections.Generic;
using Config.Maps;
using UnityEngine;

public class TagBlock
{
    public static string Blank = "Blank";
}

public class GameConfig : Singleton<GameConfig>
{
    public TextAsset levelConfigs;
    public LevelConfigs LevelConfigs;
    //
    public TextAsset assetMapConfigs;
    public MapConfigs mapConfigs;

    private void Awake()
    {
        base.Awake();    
        loadLevelConfig();
        LoadMapConfigs();
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

    public void LoadMapConfigs()
    {
        mapConfigs = JsonUtility.FromJson<MapConfigs>(assetMapConfigs.text);
    }
}
