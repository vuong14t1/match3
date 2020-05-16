using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModeGame
{
    Timer,
    Moves
}

public class GameManager : Singleton<GameManager>
{
    public Transform containBlocks;
    public float speedMove = 5f;
    public int score = 0;
    public int highScore;
    public int countThreshold = 0;
    public bool isOverGame = false;
    public bool isEndGame = false;
    public ModeGame modeGame;
    public float elapse = 0;

    public void CreaseAndUpdateScore(int numberOf)
    {
        score += Controller.Instance.model.valueOfBlock * numberOf;
        if (score >= Controller.Instance.model.thresholdTarget)
        {
            isEndGame = true;
            Controller.Instance.mEvenManager.Fire(UIEvent.WIN_GAME);
        }
    }

    public int[] GetScoreInfo()
    {
        return new[] {score, highScore};
    }
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitMaskBlocks()
    {
        
    }

    private void Update()
    {
        if (modeGame == ModeGame.Timer && !isOverGame)
        {
            elapse += Time.deltaTime;
            if (elapse >= 1)
            {
                countThreshold -= 1;
                Controller.Instance.mEvenManager.Fire(UIEvent.UPDATE_GAME_STATE, countThreshold);
                elapse = 0;
            }

            if (countThreshold <= 0)
            {
                isOverGame = true;
                Controller.Instance.mEvenManager.Fire(UIEvent.GAME_OVER);
            }
        }
    }

    public void RefreshGame()
    {
        countThreshold = Controller.Instance.model.thresholdCondition;
        isOverGame = false;
        isEndGame = false;
        Controller.Instance.mEvenManager.Fire(UIEvent.UPDATE_GAME_STATE, countThreshold);
    }
    
    
}
