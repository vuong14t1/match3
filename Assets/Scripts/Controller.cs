using System.Collections;
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
        RegisterListener();
    }

    // Start is called before the first frame update
    void Start()
    {
        view = FindObjectOfType<View>();
        model = FindObjectOfType<Model>();
        mEvenManager.Fire(UIEvent.ENTER_PLAY_STATE);
    }

    public void RegisterListener()
    {
        mEvenManager = EventManager.Instance;
        mEvenManager.Listen(UIEvent.ENTER_PLAY_STATE, StartGame);
        mEvenManager.Listen(UIEvent.GET_SCORE_INFO, UpdateScore); 
        mEvenManager.Listen(UIEvent.ACHIEVE_BLOCK, OnAchieveMatchBlock);
        mEvenManager.Listen(UIEvent.GAME_OVER, GameOver);
        mEvenManager.Listen(UIEvent.UPDATE_GAME_STATE, UpdateGameState);
        mEvenManager.Listen(UIEvent.WIN_GAME, WinGame);
    }

    private void WinGame(object obj)
    {
        Debug.Log("Win game");
    }

    private void UpdateGameState(object obj)
    {
        view.UpdateTimer((int) obj);
    }

    private void GameOver(object obj)
    {
        Debug.Log("Game over");        
    }

    private void UpdateScore(object obj)
    {
        view.UpdateScore(GameManager.Instance.GetScoreInfo());
    }

    public void OnAchieveMatchBlock(object o)
    {
        GameManager.Instance.CreaseAndUpdateScore((int)o);
        mEvenManager.Fire(UIEvent.GET_SCORE_INFO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(object o)
    {
        RefreshGame();
        mEvenManager.Fire(UIEvent.GET_SCORE_INFO);
        model.SpawnStartGame();
    }

    public void RefreshGame()
    {
        model.RefreshGame();
        GameManager.Instance.RefreshGame();
        view.UpdateTarget(model.thresholdTarget);
    }
}
