﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Singleton<Controller>
{
    public Model model;

    public View view;

    public EventManager mEvenManager;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        view = FindObjectOfType<View>();
        model = FindObjectOfType<Model>();
        RegisterListener();
        EventManager.Instance.Fire(UIEvent.RELOAD_GAME);
    }

    public void RegisterListener()
    {
        EventManager.Instance.Listen(UIEvent.ENTER_PLAY_STATE, StartGame);
        EventManager.Instance.Listen(UIEvent.GET_SCORE_INFO, UpdateScore); 
        EventManager.Instance.Listen(UIEvent.ACHIEVE_BLOCK, OnAchieveMatchBlock);
        EventManager.Instance.Listen(UIEvent.GAME_OVER, GameOver);
        EventManager.Instance.Listen(UIEvent.UPDATE_GAME_STATE, UpdateGameState);
        EventManager.Instance.Listen(UIEvent.WIN_GAME, WinGame);
        EventManager.Instance.Listen(UIEvent.SWAP_BLOCK, SwapBlock);
        EventManager.Instance.Listen(UIEvent.NEXT_LEVEL, NextLevel);
        EventManager.Instance.Listen(UIEvent.RELOAD_GAME, ReloadGame);
    }

    private void SwapBlock(object obj)
    {
        GameManager.Instance.DecreaseMoves((int)obj);
    }

    private void WinGame(object obj)
    {
        view.ShowGUIEndGame();
    }

    private void UpdateGameState(object obj)
    {
        view.UpdateTimer((int) obj);
    }

    private void GameOver(object obj)
    {
        Debug.Log("Game over");
        view.ShowGUIGameOver();
    }

    private void UpdateScore(object obj)
    {
        view.UpdateScore(GameManager.Instance.GetScoreInfo());
    }

    public void OnAchieveMatchBlock(object o)
    {
        GameManager.Instance.CreaseAndUpdateScore((int)o);
        EventManager.Instance.Fire(UIEvent.GET_SCORE_INFO);
    }

    // Update is called once per frame
    void Update()    
    {
        
    }

    public void StartGame(object o)
    {
        RefreshGame();
        EventManager.Instance.Fire(UIEvent.GET_SCORE_INFO);
        model.SpawnStartGame();
    }

    public void RefreshGame()
    {
        model.RefreshGame();
        GameManager.Instance.RefreshGame();
        view.UpdateTarget(model.thresholdTarget);
        view.UpdateTitleModeGame(GameManager.Instance.modeGame);
    }

    public void NextLevel(object o)
    {
        GameManager.Instance.NextLevel();
        EventManager.Instance.Fire(UIEvent.ENTER_PLAY_STATE);
    }

    public void SaveGame()
    {
        Local.LocalManager.SaveGame();
    }

    public void ReloadGame(object o)
    {
        DataLocal dataLocal = Local.LocalManager.LoadGame();
        if (dataLocal != null && dataLocal.stateGame != StateGame.EndGame && GameManager.Instance.isSaveLocal)
        {
            GameManager.Instance.DeSerialize(dataLocal);
            model.UpdateThresholdTarget();
            model.SpawnBlocksFromCached(dataLocal);
            view.UpdateTarget(model.thresholdTarget);
            view.UpdateTitleModeGame(GameManager.Instance.modeGame);
            EventManager.Instance.Fire(UIEvent.UPDATE_GAME_STATE, GameManager.Instance.countThreshold);
            EventManager.Instance.Fire(UIEvent.GET_SCORE_INFO);    
        }
        else
        {
            EventManager.Instance.Fire(UIEvent.ENTER_PLAY_STATE);
        }
        
    }

}
