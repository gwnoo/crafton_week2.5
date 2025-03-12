using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;
using System.Reflection.Emit;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoardCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject[] boardSlot;
    [SerializeField]
    private TextMeshProUGUI scoreTxt;
    [SerializeField]
    private TextMeshProUGUI gameOverTxt;
    public static int score = 0;
    public static bool gameover = false;
    public static bool gameclear = false;
    public int displayedTileCount = 0;
    private int[] uf = new int[49];
    private GameObject tileGenerator;
    private GameObject monsterManager;
    private GameObject boardInventory;
    private UnityEngine.UI.Image image;

    private int[] checkNum = new int[] { 1, 2, 3, 4, 5, 7, 13, 14, 20, 21, 27, 28, 34, 35, 41, 43, 44, 45, 46, 47 };
    private int[] bossCheck = new int[] {0, 0, 0, 0, 0};

    public static int[,] adj = new int[7, 7];

    private void Awake()
    {
        adj = new int[7, 7] { { 0, 4, 4, 4, 4, 4, 0 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 0, 1, 1, 1, 1, 1, 0 } };
        gameover = false;
        score = 0;
        scoreTxt.text = "" + score;
        boardInventory = GameObject.Find("BoardInventory");
        for (int i = 0; i < 25; i++)
        {
            boardSlot[i] = boardInventory.transform.GetChild(i).gameObject;
        }
        tileGenerator = GameObject.Find("TileGenerator");
        monsterManager = GameObject.Find("MonsterManager");
    }

    public void Check()
    {
        for (int i = 0; i < 49; i++) uf[i] = i;
        int cycleCheck = 0;

        // 연결하기
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                if ((adj[i, j] & 1) > 0 && (adj[i - 1, j] & 4) > 0) // 도로와 위쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j - 7);
                }
                if ((adj[i, j] & 2) > 0 && (adj[i, j + 1] & 8) > 0) // 도로와 오른쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j + 1);
                }
                if ((adj[i, j] & 4) > 0 && (adj[i + 1, j] & 1) > 0) // 도로와 아래쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j + 7);
                }
                if ((adj[i, j] & 8) > 0 && (adj[i, j - 1] & 2) > 0) // 도로와 왼쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j - 1);
                }
            }
        }

        // 연결 확인
        for (int i = 0; i < 7; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                if (i > 0 && i < 6 && j > 0 && j < 6) continue;

                if (uf[7 * i + j] != 7 * i + j)
                {
                    cycleCheck = uf[7 * i + j];
                }
            }
        }

        if (cycleCheck > 0)
        {
            GetScore(cycleCheck);
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (Array.Exists(checkNum, x => x == uf[7 * i + j]))
                {
                    if (i > 0 && i < 6 && j > 0 && j < 6)
                    {
                        TileChange(uf[7 * i + j]);
                    }
                        
                }
            }
        }

        if(TurnCounting.Instance.breakTurn > 0)
        {
            if(TurnCounting.Instance.breakTurn == 3 && (TurnCounting.Instance.bossTrigger != TurnCounting.Instance.lastbossTrigger))
            {
                if (TurnCounting.Instance.bossTrigger == 5)
                {
                    monsterManager.GetComponent<MonsterSpawner>().UpdateBoss();
                }
                else
                {
                    monsterManager.GetComponent<MonsterSpawner>().StartBossScene(TurnCounting.Instance.bossTrigger);
                }

            }
            if (TurnCounting.Instance.breakTurn == 2 && (TurnCounting.Instance.bossTrigger != TurnCounting.Instance.lastbossTrigger))
            {
                if (TurnCounting.Instance.bossTrigger == 5)
                {
                    TurnCounting.Instance.breakTurn++;
                    monsterManager.GetComponent<MonsterSpawner>().bossTurnCount++;
                    monsterManager.GetComponent<MonsterSpawner>().UpdateBoss();
                }
                else
                {
                    monsterManager.GetComponent<MonsterSpawner>().EndBossScene();
                    TurnCounting.Instance.lastbossTrigger = TurnCounting.Instance.bossTrigger;
                }
                
            }

            if (TurnCounting.Instance.breakTurn == 1)
            {
                TurnCounting.Instance.UpdateScene();
            }
            TurnCounting.Instance.breakTurn--;
        }
        else
        {
            //턴 증가
            TurnCounting.Instance.turnCount++;
            //턴에 해당하는 점수 충족 여부 확인 및 게임 종료 결정
            TurnCounting.Instance.CheckTurnAndGoal();
        }

        if (displayedTileCount >= 25) // gameover
        {
            gameover = true;
        }
        if(gameover)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverTxt.gameObject.SetActive(true);
            gameOverTxt.text = "Your Score is " + score;
        }

        if(gameclear)
        {
            image = boardInventory.GetComponent<UnityEngine.UI.Image>();
            image.color = new Color32(200, 0, 0, 155);
            score += 100000;
            gameclear = false;
        }

        scoreTxt.text = "" + score;
    }

    private void UfMerge(int a, int b)
    {
        a = UfFind(a);
        b = UfFind(b);

        if (Array.Exists(checkNum, x => x == a))
        {
            uf[b] = a;
        }
        else
        {
            uf[a] = b;
        }
    }

    private int UfFind(int x)
    {
        if (uf[x] == x)
        {
            return x;
        }
        else
        {
            uf[x] = UfFind(uf[x]);
            return uf[x];
            //return UfFind(uf[x]);
        }
    }

    private void GetScore(int num)
    {
        SoundManager.Instance.PlayScoreSound();
        int len = 0;

        for(int i = 1; i <= 5; i++)
        {
            for(int j = 1; j <= 5; j++)
            {
                if (UfFind(uf[7 * i + j]) == num)
                {
                    DestroyTile(i, j);
                    len++;
                }
            }
        }

        if((monsterManager.GetComponent<MonsterSpawner>().CheckMonster() == 2 || (monsterManager.GetComponent<MonsterSpawner>().CheckMonster() == 3 && monsterManager.GetComponent<MonsterSpawner>().buckShotMode == 2)) &&  (len == 1 || len == 2))
        {
            TurnCounting.Instance.turnCount += TurnCounting.Instance.limitTurn / 3;
        }
        else
        {
            if(len >= 21 && bossCheck[len - 21] == 0)
            {
                TurnCounting.Instance.bossTrigger++;
                bossCheck[len - 21] = 1;
            }

            // 점수 계산 : 배율 정해서. 이부분은 쉽게 수정되게. 배율변수 빼기.
            displayedTileCount -= len;
            score += len * len * len;

            if (monsterManager.GetComponent<MonsterSpawner>().CheckMonster() == 8)
            {
                monsterManager.GetComponent<MonsterSpawner>().bossTurnScore += len * len * len;
                if(len == 25)
                {
                    monsterManager.GetComponent<MonsterSpawner>().bossDeathTrigger = true;
                }
            }
            else
            {
                TurnCounting.Instance.turnScore += len * len * len;
            }
                
            

        }
            
    }

    private void DestroyTile(int y, int x)
    {
        adj[y, x] = 0;
        if(boardSlot[5 * y + x - 6].transform.childCount > 0)
        {
            boardSlot[5 * y + x - 6].transform.GetChild(0).GetComponent<TileDestroy>().StartBreak();
            uf[7 * y + x] = 7 * y + x;
        }
    }

    private void TileChange(int num)
    {
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                if (UfFind(uf[7 * i + j]) == num && adj[i, j] != 0)
                {
                    tileGenerator.GetComponent<TileGenerator>().TileReGenerate(boardSlot[5 * i + j - 6].transform, boardSlot[5 * i + j - 6].transform.GetChild(0).GetComponent<TileDraggable>().tileType);
                }
            }
        }
    }

}
