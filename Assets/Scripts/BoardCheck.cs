using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;
using System.Reflection.Emit;
using UnityEngine.InputSystem;

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
    public int displayedTileCount = 0;
    private int[] uf = new int[25];
    private GameObject tileGenerator;
    private GameObject player;

    private int[] checkNum = new int[] { 1, 2, 3, 5, 9, 10, 14, 15, 19, 21, 22, 23};

    public static int[,] adj = new int[5, 5];

    private void Awake()
    {
        adj = new int[5, 5] { { 0, 4, 4, 4, 0 }, { 2, 0, 0, 0, 8 }, { 2, 0, 0, 0, 8 }, { 2, 0, 0, 0, 8 }, { 0, 1, 1, 1, 0 } };
        gameover = false;
        score = 0;
        scoreTxt.text = "" + score;
        GameObject boardInventory = GameObject.Find("BoardInventory");
        for (int i = 0; i < 9; i++)
        {
            boardSlot[i] = boardInventory.transform.GetChild(i).gameObject; 
        }
        tileGenerator = GameObject.Find("TileGenerator");
        player = GameObject.Find("Player");
    }

    public void Check()
    {
        for (int i = 0; i < 25; i++) uf[i] = i;
        int cycleCheck = 0;

        // 연결하기
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                if ((adj[i, j] & 1) > 0 && (adj[i - 1, j] & 4) > 0) // 도로와 위쪽이 이어져 있는지
                {
                    UfMerge(5 * i + j, 5 * i + j - 5);
                }
                if ((adj[i, j] & 2) > 0 && (adj[i, j + 1] & 8) > 0) // 도로와 오른쪽이 이어져 있는지
                {
                    UfMerge(5 * i + j, 5 * i + j + 1);
                }
                if ((adj[i, j] & 4) > 0 && (adj[i + 1, j] & 1) > 0) // 도로와 아래쪽이 이어져 있는지
                {
                    UfMerge(5 * i + j, 5 * i + j + 5);
                }
                if ((adj[i, j] & 8) > 0 && (adj[i, j - 1] & 2) > 0) // 도로와 왼쪽이 이어져 있는지
                {
                    UfMerge(5 * i + j, 5 * i + j - 1);
                }
            }
        }

        // 연결 확인
        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if (i > 0 && i < 4 && j > 0 && j < 4) continue;

                if (uf[5 * i + j] != 5 * i + j)
                {
                    cycleCheck = uf[5 * i + j];
                }
            }
        }

        if (cycleCheck > 0)
        {
            GetScore(cycleCheck);
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Array.Exists(checkNum, x => x == uf[5 * i + j]))
                {
                    if (i > 0 && i < 4 && j > 0 && j < 4)
                    {
                        TileChange(uf[5 * i + j]);
                    }
                        
                }
            }
        }

        if (displayedTileCount >= 9) // gameover
        {
            gameover = true;
        }
        if(gameover)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverTxt.gameObject.SetActive(true);
            gameOverTxt.text = "Your Score is " + score;
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

        for(int i = 1; i <= 3; i++)
        {
            for(int j = 1; j <= 3; j++)
            {
                if (UfFind(uf[5 * i + j]) == num)
                {
                    DestroyTile(i, j);
                    len++;
                }
            }
        }

        // 점수 계산 : 배율 정해서. 이부분은 쉽게 수정되게. 배율변수 빼기.
        displayedTileCount -= len;
        score += len * len * len;
        if(len == 1)
        {
            
        }
        else if(len == 2)
        {
            player.GetComponent<PlayerController>().GetDashCount();
        }
        else
        {
            player.GetComponent<PlayerController>().ShootFireball();
        }
            
    }

    private void DestroyTile(int y, int x)
    {
        adj[y, x] = 0;
        if(boardSlot[3 * y + x - 4].transform.childCount > 0)
        {
            boardSlot[3 * y + x - 4].transform.GetChild(0).GetComponent<TileDestroy>().StartBreak();
            uf[5 * y + x] = 5 * y + x;
        }
    }

    private void TileChange(int num)
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                if (UfFind(uf[5 * i + j]) == num && adj[i, j] != 0)
                {
                    tileGenerator.GetComponent<TileGenerator>().TileReGenerate(boardSlot[3 * i + j - 4].transform, boardSlot[3 * i + j - 4].transform.GetChild(0).GetComponent<TileDraggable>().tileType);
                }
            }
        }
    }
}
