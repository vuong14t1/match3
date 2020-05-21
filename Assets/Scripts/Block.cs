using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum KindOfBlock
{
    Normal,
    Blank,
    BombVertical,
    BombHorizontal,
    BombSquare
}
public class Block : MonoBehaviour
{
    public Vector2 posTarget;
    public bool isPause = false;
    private Vector2 beginPos;
    private Vector2 endPos;
    private float maxDistanceMove = 1;
    public KindOfBlock kindOfBlock;

    public Block()
    {
        kindOfBlock = KindOfBlock.Normal;
    }

    public void UpdateKindOfBlock(KindOfBlock kindOfBlock)
    {
        if (this.kindOfBlock != KindOfBlock.Normal && this.kindOfBlock != KindOfBlock.Blank)
        {
            if (this.kindOfBlock == KindOfBlock.BombHorizontal || this.kindOfBlock == KindOfBlock.BombVertical)
            {
                this.kindOfBlock = KindOfBlock.BombSquare;
            }
            else
            {
                this.kindOfBlock = kindOfBlock;
            }
        }
        else
        {
            this.kindOfBlock = kindOfBlock;
        }

        UpdateSpriteByKindOfBlock();
    }

    public void UpdateSpriteByKindOfBlock()
    {
        switch (this.kindOfBlock)
        {
            case KindOfBlock.BombHorizontal:
                GetComponent<SpriteRenderer>().sprite = Controller.Instance.model.spriteBombHorizontal;
                break;
            case KindOfBlock.BombVertical:
                GetComponent<SpriteRenderer>().sprite = Controller.Instance.model.spriteBombVertical;
                break;
            case KindOfBlock.BombSquare:
                GetComponent<SpriteRenderer>().sprite = Controller.Instance.model.spriteBombSquare;
                break;
        }
    }
    public void setPositionTarget(Vector2 vec)
    {
        posTarget = vec;
    }

    public void MovePosTarget()
    {
        if (!isPause)
        {
            transform.DOMove(new Vector3(posTarget.x, posTarget.y,transform.position.z), Vector2.Distance(transform.position, posTarget) / GameManager.Instance.speedMove)
                .SetEase(Ease.OutBack, 0.8f);
            
        }
    }

    public List<GameObject> GetMatchBlocks()
    {
        GameObject[,] allBlocks = Controller.Instance.model.allBlocks;
        List<GameObject> matchB = new List<GameObject>();
        int columnMatrix = Controller.Instance.model.columnMaxMatrix;
        int rowMatrix = Controller.Instance.model.rowMaxMatrix;
        //check horizontal
        
        if (Mathf.RoundToInt(posTarget.y) + 1 < rowMatrix
            && Mathf.RoundToInt(posTarget.y) - 1 >= 0
            && allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) + 1] != null
            && allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) - 1] != null
            && allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) + 1].CompareTag(transform.tag)
            && allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) - 1].CompareTag(transform.tag))
        {
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) + 1]);
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y) - 1]);
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y)]);
            
        }else if (Mathf.RoundToInt(posTarget.x) + 1 < columnMatrix 
                  && Mathf.RoundToInt(posTarget.x) - 1 >= 0
                  && allBlocks[Mathf.RoundToInt(posTarget.x) + 1, Mathf.RoundToInt(posTarget.y)] != null
                  && allBlocks[Mathf.RoundToInt(posTarget.x) - 1, Mathf.RoundToInt(posTarget.y)] != null
                  && allBlocks[Mathf.RoundToInt(posTarget.x) + 1, Mathf.RoundToInt(posTarget.y)].CompareTag(transform.tag)
                  && allBlocks[Mathf.RoundToInt(posTarget.x) - 1, Mathf.RoundToInt(posTarget.y)].CompareTag(transform.tag))
        {
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x) - 1, Mathf.RoundToInt(posTarget.y)]);
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x) + 1, Mathf.RoundToInt(posTarget.y)]);
            matchB.Add(allBlocks[Mathf.RoundToInt(posTarget.x), Mathf.RoundToInt(posTarget.y)]);
        }
        return matchB;
    }

    public void AnimateDestroy(float delay = 0)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetDelay(delay));
        mySequence.Append(transform.DOScale(new Vector3(0f, 0f, 0f), .3f).OnComplete(() =>
        {
            Destroy(gameObject);
        }));
        Controller.Instance.model.allBlocks[(int)posTarget.x, (int)posTarget.y] = null;
    }

    private void Update()
    {
        OnTouchEventListener();
    }

    private void OnMouseDown()
    {
        beginPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void OnMouseUp()
    {
        endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        CheckAndHandleInput(beginPos, endPos);
    }

    public void CheckAndHandleInput(Vector3 beginPos, Vector3 endPos)
    {
        if (GameManager.Instance.GetStateGame() == StateGame.Moving)
        {
            Debug.Log("State game is moving");
            return;
        }
        float distance = Vector3.Distance(beginPos, endPos);
        if (distance > maxDistanceMove)
        {
            Vector2 direction = (endPos - beginPos).normalized;
            direction = new Vector2(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
            if (direction.x != direction.y)
            {
                //kiem tra move block co ra ngoai map hay khong
                if (Controller.Instance.model.isValidMapByPosition(new Vector2(posTarget.x + direction.x,
                    posTarget.y + direction.y)))
                {
                    Controller.Instance.model.SwapBlock(gameObject, direction, false);
                    GameManager.Instance.SetStateGame(StateGame.Moving);    
                }
                else
                {
                    Debug.Log("Move bound map");        
                }
                
            }
            else
                Debug.Log("distance too long");{
                
            }
        }
        else
        {
            Debug.Log("distance too short");
        }
    }

    public void OnTouchEventListener()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);
            if (input.phase == TouchPhase.Began)
            {
            }else if (input.phase == TouchPhase.Ended)
            {
                
            }    
        }
        
    }
}
