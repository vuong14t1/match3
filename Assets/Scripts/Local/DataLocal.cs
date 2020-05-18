using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class DataLocal
{
    public ModeGame modeGame;

    public StateGame stateGame;

    public int level;

    public int score;

    public bool isMute;

    public string[,] allBlock;

    public int highScore;

    public int countThreshold;

    public static DataLocal CreateData()
    {
        DataLocal dataLocal = new DataLocal();
        dataLocal.modeGame = GameManager.Instance.modeGame;
        dataLocal.stateGame = GameManager.Instance.stateGame;
        dataLocal.level = GameManager.Instance.level;
        dataLocal.score = GameManager.Instance.score;
        dataLocal.isMute = false;
        int c = Controller.Instance.model.columnMaxMatrix;
        int r = Controller.Instance.model.rowMaxMatrix;
        dataLocal.allBlock = new string[c, r];
        for (int i = 0; i < c; i++)
        {
            for (int j = 0; j < r; j++)
            {
                dataLocal.allBlock[i, j] = Controller.Instance.model.allBlocks[i, j].tag;
            }
        }

        dataLocal.countThreshold = GameManager.Instance.countThreshold;

        dataLocal.highScore = GameManager.Instance.GetHighScore();
        return dataLocal;
    }

    
}
