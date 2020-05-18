using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Model : MonoBehaviour
{
    public GameObject[] prefabBlock;

    public int columnMaxMatrix;
    public int rowMaxMatrix;
    public int posYSpawn;
    public int valueOfBlock;
    public int thresholdCondition;
    public int thresholdTarget;
    public GameObject[,] allBlocks;
    // Start is called before the first frame update
    void Start()
    {
        allBlocks = new GameObject[columnMaxMatrix, rowMaxMatrix];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isValidMapByPosition(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < columnMaxMatrix && pos.y >= 0 && pos.y < rowMaxMatrix;
    }

    public GameObject SpawnBlock(int i, int j, bool isCheckMatch = false)
    {
        int iterator = 0;
        int maxIterator = 5;
        GameObject obj = null;
        while (true)
        {
            Transform containBlocks = GameManager.Instance.containBlocks;
            int idx = Random.Range(0, prefabBlock.Length);
            obj = Instantiate(prefabBlock[idx]);
            obj.transform.parent = containBlocks;
            obj.name = "[" + i + "," + j + "]";
            Block block = obj.GetComponent<Block>();
            block.setPositionTarget(new Vector2(i, j));
            obj.transform.position = new Vector2(i, posYSpawn + j);
            if (!isCheckMatch || !IsAchieveAround(i, j, obj) || iterator >= maxIterator)
            {
                return obj;
            }
            Destroy(obj);
            iterator++;
        }
        
        return obj;
    }

    public GameObject SpawnBlockByTag(int i, int j, string tag)
    {
        GameObject obj = null;
        Transform containBlocks = GameManager.Instance.containBlocks;
        int idx = Random.Range(0, prefabBlock.Length);
        for (int k = 0; k < prefabBlock.Length; k++)
        {
            if (prefabBlock[k].tag == tag)
            {
                idx = k;
                break;
            }
        }
        obj = Instantiate(prefabBlock[idx]);
        obj.transform.parent = containBlocks;
        obj.name = "[" + i + "," + j + "]";
        Block block = obj.GetComponent<Block>();
        block.setPositionTarget(new Vector2(i, j));
        obj.transform.position = new Vector2(i, posYSpawn + j);
        return obj;
    }

    public bool IsAchieveAround(int i, int j, GameObject obj)
    {
        List<GameObject> achieves = new List<GameObject>();
        
        for (int r = -2; r <= 0; r++)
        {
            if (j + r < rowMaxMatrix
                && j + r >= 0
                && allBlocks[i, j + r] != null
                && allBlocks[i, j + r].CompareTag(obj.tag)
                )
            {
                achieves.Add(allBlocks[i, j + r]);
            }
        }

        if (achieves.Count >= 2)
        {
            return true;
        }
        achieves.Clear();
        for (int r = -2; r <= 0; r++)
        {
            if (i + r < rowMaxMatrix
                && i + r >= 0
                && allBlocks[i + r, j] != null
                && allBlocks[i  + r, j].CompareTag(obj.tag)
            )
            {
                achieves.Add(allBlocks[i + r, j]);
            }
        }
        if (achieves.Count >= 2)
        {
            return true;
        }
        return false;
    }
    

    public void SpawnStartGame()
    {
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                allBlocks[i, j] = SpawnBlock(i, j, true);
                allBlocks[i, j].GetComponent<Block>().MovePosTarget();
            }
        }

        CheckAndDestroyMatchBlock(0.7f);
    }

    public bool SpawnBlocksFromCached(DataLocal dataLocal)
    {
        if (dataLocal.stateGame != StateGame.EndGame)
        {
            for (int i = 0; i < columnMaxMatrix; i++)
            {
                for (int j = 0; j < rowMaxMatrix; j++)
                {
                    allBlocks[i, j] = SpawnBlockByTag(i, j, dataLocal.allBlock[i, j]);
                    allBlocks[i, j].GetComponent<Block>().MovePosTarget();
                }
            }
            return true;
        }

        return false;
    }

    public void CheckAndDestroyMatchBlock(float delay = 1f)
    {
        StartCoroutine(CheckMapAfterStartGame(delay));
    }

    IEnumerator CheckMapAfterStartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckAndAchieveBlock();
    }

    IEnumerator FillMapCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        FillMap();
    }

    public void CheckAndAchieveBlock()
    {
        List<GameObject> allMatchBlock = GetAchieveBlock();
        if (allMatchBlock.Count > 0)
        {
            DeleteMatchBlock(allMatchBlock);    
        }
        else
        {
            GameManager.Instance.SetStateGame(StateGame.Idle);
        }
        
    }

    public List<GameObject> GetAchieveBlock()
    {
        List<GameObject> allMatchBlock = new List<GameObject>();
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                if (allBlocks[i, j] != null)
                {
                    List<GameObject> blocks = allBlocks[i, j].GetComponent<Block>().GetMatchBlocks();
                    allMatchBlock = allMatchBlock.Union(blocks).ToList();    
                }
                
            }
        }

        return allMatchBlock;
    }

    public void DeleteMatchBlock(List<GameObject> allMatchBlock)
    {
        foreach (var obj in allMatchBlock)
        {
            obj.GetComponent<Block>().AnimateDestroy(0.2f);
        }
        EventManager.Instance.Fire(UIEvent.ACHIEVE_BLOCK, allMatchBlock.Count);
        GameObject[,] tempAllBlocks = new GameObject[columnMaxMatrix, rowMaxMatrix];
        int countI = 0;
        int countJ = 0;
        //Check down null
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                if (allBlocks[i, j] != null)
                {
                    tempAllBlocks[countI, countJ ++] = allBlocks[i, j];
                }
            }

            countJ = 0;
            countI ++;
        }
        allBlocks = tempAllBlocks;
        StartCoroutine(FillMapCoroutine(0.5f));
    }

    public void FillMap()
    {
        //create block replace null
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                if (allBlocks[i, j] == null)
                {
                    allBlocks[i, j] = SpawnBlock(i, j);
                }
                else
                {
                    allBlocks[i, j].GetComponent<Block>().setPositionTarget(new Vector2(i, j));
                }
            }
        }
        
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                allBlocks[i, j].GetComponent<Block>().MovePosTarget();
            }
        }
        CheckAndDestroyMatchBlock(0.5f);
    }

    public void SwapBlock(GameObject obj, Vector2 direction, bool isOnlySwap)
    {
        Block block1 = obj.GetComponent<Block>();
        GameObject obj2 = allBlocks[(int)(block1.posTarget.x + direction.x), (int)(block1.posTarget.y + direction.y)];
        Block block2 = obj2.GetComponent<Block>();

        //swap pos
        Vector2 tempPos = block1.posTarget;
        block1.setPositionTarget(block2.posTarget);
        block2.setPositionTarget(tempPos);
        
        //swap obj
        allBlocks[(int) block1.posTarget.x, (int) block1.posTarget.y] = obj;
        allBlocks[(int) block2.posTarget.x, (int) block2.posTarget.y] = obj2;
        
        block1.MovePosTarget();
        block2.MovePosTarget();
        if (isOnlySwap) return;
        EventManager.Instance.Fire(UIEvent.SWAP_BLOCK, 1);
        StartCoroutine(CheckAfterSwap(obj, direction));
    }

    public IEnumerator CheckAfterSwap(GameObject obj, Vector2 direction)
    {
        yield return new WaitForSeconds(0.5f);
        List<GameObject> allMatchBlocks = GetAchieveBlock(); 
        if (allMatchBlocks.Count > 0)
        {
            DeleteMatchBlock(allMatchBlocks);
        }
        else
        {
            SwapBlock(obj, direction * -1, true);
            yield return new WaitForSeconds(0.4f);
            GameManager.Instance.SetStateGame(StateGame.Idle);
        }
    }

    public void TestCheckMap(List<GameObject> allMatchBlock)
    {
        
        
    }

    public void RefreshGame()
    {
        for (int i = 0; i < columnMaxMatrix; i++)
        {
            for (int j = 0; j < rowMaxMatrix; j++)
            {
                if (allBlocks[i, j] != null)
                {
                    Destroy(allBlocks[i, j]);
                }
            }
        }

        UpdateThresholdTarget();
    }

    public void UpdateThresholdTarget()
    {
        int level = GameManager.Instance.level;
        LevelConfig levelConfig = GameConfig.Instance.GetLevelConfig(level);
        thresholdTarget = levelConfig.target;
        thresholdCondition = levelConfig.thresholdC;
        if (levelConfig.modeGame == "Timer")
        {
            GameManager.Instance.modeGame = ModeGame.Timer;
        }else if (levelConfig.modeGame == "Moves")
        {
            GameManager.Instance.modeGame = ModeGame.Moves;
        }
    }
}
