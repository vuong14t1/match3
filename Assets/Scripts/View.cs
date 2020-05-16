using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class View : MonoBehaviour
{
    public Text lbScore;

    public Text lbTimer;

    public Text lbTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()    
    {
        
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

    
}
