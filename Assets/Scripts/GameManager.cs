using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModeGame
{
    Timer,
    Moves
}

public enum StateGame
{
    Idle,
    Moving,
    EndGame
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
    public StateGame stateGame = StateGame.Idle;
    public int level = 1;
    public void CreaseAndUpdateScore(int numberOf)
    {
        score += Controller.Instance.model.valueOfBlock * numberOf;
        if (score >= Controller.Instance.model.thresholdTarget)
        {
            isEndGame = true;
            EventManager.Instance.Fire(UIEvent.WIN_GAME);
            SetStateGame(StateGame.EndGame);
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

    public void DecreaseMoves(int moves)
    {
        if (modeGame != ModeGame.Moves)
        {
            Debug.Log("wrong mode game "+ modeGame);
        } 
        countThreshold -= moves;
        EventManager.Instance.Fire(UIEvent.UPDATE_GAME_STATE, countThreshold);
        if (countThreshold <= 0)
        {
            isOverGame = true;
            EventManager.Instance.Fire(UIEvent.GAME_OVER);
            SetStateGame(StateGame.EndGame);
        }
    }
        

    private void Update()
    {
        if (modeGame == ModeGame.Timer && !isOverGame)
        {
            elapse += Time.deltaTime;
            if (elapse >= 1)
            {
                countThreshold -= 1;
                EventManager.Instance.Fire(UIEvent.UPDATE_GAME_STATE, countThreshold);
                elapse = 0;
            }

            if (countThreshold <= 0)
            {
                isOverGame = true;
                EventManager.Instance.Fire(UIEvent.GAME_OVER);
                SetStateGame(StateGame.EndGame);
            }
        }
    }

    public void RefreshGame()
    {
        countThreshold = Controller.Instance.model.thresholdCondition;
        isOverGame = false;
        isEndGame = false;
        EventManager.Instance.Fire(UIEvent.UPDATE_GAME_STATE, countThreshold);
        SetStateGame(StateGame.Idle);
    }

    public int GetHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }

        return highScore;
    }

    public void SetStateGame(StateGame stateGame)
    {
        this.stateGame = stateGame;
    }

    public StateGame GetStateGame()
    {
        return this.stateGame;
    }

    public void NextLevel()
    {
        level++;
    }
    
    
}
