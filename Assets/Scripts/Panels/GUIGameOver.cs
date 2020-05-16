using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIGameOver : BasePanel
{
    public Button btnClose;
    public Text lbScore;
    public Text lbBestScore;
    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override BasePanel Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        lbScore.text = GameManager.Instance.score + "";
        lbBestScore.text = GameManager.Instance.GetHighScore() + "";
        return this;
    }

    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            Debug.Log("vao day close");
            UIManager.Instance.SetClose(this);
        });
    }

    public override void Hide()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public override void Destroy()
    {
    }
}
