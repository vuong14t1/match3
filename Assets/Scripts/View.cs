using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class View : MonoBehaviour
{
    public Text lbScore;

    public Text lbTimer;

    public Text lbTarget;

    public Text lbTitleModeGame;

    public GameObject guiGameOver;

    public GameObject guiEndGame;

    public GameObject[] prefabGUI;

    public GameObject containGUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()    
    {
        
    }

    public void ShowGUIGameOver()
    {
        if (guiGameOver == null)
        {
            guiGameOver = Instantiate(prefabGUI[0]);
            guiGameOver.transform.parent = containGUI.transform;
            guiGameOver.transform.localPosition = new Vector3(- Screen.width / 2, - Screen.height / 2, 0);
        }

        UIManager.Instance.ShowOne(guiGameOver.GetComponent<GUIGameOver>());
    }

    public void ShowGUIEndGame()
    {
        if (guiEndGame == null)
        {
            guiEndGame = Instantiate(prefabGUI[1]);
            guiEndGame.transform.parent = containGUI.transform;
            guiEndGame.transform.localPosition = new Vector3(-Screen.width / 2, - Screen.height / 2, 0);
        }

        UIManager.Instance.ShowOne(guiEndGame.GetComponent<GUIEndGame>());
        
    }
    public void UpdateScore(int[] infoScore)
    {
        lbScore.text = infoScore[0] + "";
    }

    public void UpdateTimer(int value)
    {
        lbTimer.text = value + "";
    }

    public void UpdateTarget(int value)
    {
        lbTarget.text = value + "";
    }

    public void UpdateTitleModeGame(ModeGame mode)
    {
        if (mode == ModeGame.Moves)
        {
            lbTitleModeGame.text = "Moves";
        }else if (mode == ModeGame.Timer)
        {
            lbTitleModeGame.text = "Timer";
        }
    }

    
}
