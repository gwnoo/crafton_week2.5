using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] monster;
    [SerializeField]
    private GameObject bossHpBar;
    [SerializeField]
    private GameObject hpBar;
    [SerializeField]
    private int aliveMonster = -1;
    private int nextMonster = 0;
    public int buckShotMode = 1;

    //Boss Stat
    private int bossGoalScore = 15625;
    private int bossLimitTurn = 50;
    public int bossTurnCount = 0;
    public int bossTurnScore = 0;
    public bool bossDeathTrigger = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int CheckMonster()
    {
        return aliveMonster;
    }
    public int CheckNextMonster()
    {
        return nextMonster;
    }

    public void SpawnMonster(int type)
    {
        monster[type].SetActive(true);
    }

    public void KillMonster()
    {
        foreach (GameObject alive in monster)   
        {
            if(alive.activeSelf)
            {
                alive.SetActive(false);
                if(aliveMonster == 1 || (aliveMonster == 3 && buckShotMode == 1))
                {
                    TurnCounting.Instance.limitTurn /= 2;
                    TurnCounting.Instance.goalScore /= 10;
                }
            }
        }
        if(aliveMonster != -1)
        {
            aliveMonster = -1;
            nextMonster = Random.Range(0, 4);
            buckShotMode = Random.Range(1, 3);
        }
    }

    public void SpawnMonsterByLevel()
    {
        SpawnMonster(nextMonster);
        aliveMonster = nextMonster;
    }

        public void SpawnBoss()
    {
        SpawnMonster(8);
        aliveMonster = 8;
    }

    public void UpdateMonster()
    {
        int goalScore = TurnCounting.Instance.goalScore;
        int limitTurn = TurnCounting.Instance.limitTurn;
        int turnCount = TurnCounting.Instance.turnCount;
        int turnScore = TurnCounting.Instance.turnScore;
        if (turnScore >= goalScore)
        {
            KillMonster();
            bossHpBar.SetActive(false);
            hpBar.SetActive(false);
            TurnCounting.Instance.turnCount = limitTurn;
        }
        else
        {
            if(aliveMonster == -1)
            {
                SpawnMonsterByLevel();
            }
            bossHpBar.SetActive(true);
            hpBar.SetActive(true);

            bossHpBar.GetComponent<HpBarControll>().SetHp(goalScore - turnScore, goalScore);
            hpBar.GetComponent<HpBarControll>().SetHp(limitTurn - turnCount, limitTurn);
        }
    }

    public void StartBossScene(int stage) 
    {
        SpawnMonster(stage + 3);
    }

    public void EndBossScene()
    {
        KillMonster();
        
    }

    public void UpdateBoss()
    {
        if (bossTurnScore >= bossGoalScore)
        {
            KillMonster();
            bossHpBar.SetActive(false);
            hpBar.SetActive(false);
            bossTurnCount = bossLimitTurn;
            
        }
        else
        {
            if (aliveMonster == -1)
            {
                SpawnBoss();
            }
            bossHpBar.SetActive(true);
            hpBar.SetActive(true);

            bossHpBar.GetComponent<HpBarControll>().SetHp(bossGoalScore - bossTurnScore, bossGoalScore);
            hpBar.GetComponent<HpBarControll>().SetHp(bossLimitTurn - bossTurnCount, bossLimitTurn);
        }

        if (bossTurnCount >= bossLimitTurn)
        {
            if (bossTurnScore < bossGoalScore)
            {
                //game over
                BoardCheck.gameover = true;
            }
            else if(bossDeathTrigger == false)
            {
                if (aliveMonster == -1)
                {
                    SpawnBoss();
                }
                bossHpBar.SetActive(true);
                hpBar.SetActive(true);

                bossGoalScore = 15625;
                bossLimitTurn = 50;
                bossTurnCount = 0;
                bossTurnScore = 0;


                bossHpBar.GetComponent<HpBarControll>().SetHp(bossGoalScore - bossTurnScore, bossGoalScore);
                hpBar.GetComponent<HpBarControll>().SetHp(bossLimitTurn - bossTurnCount, bossLimitTurn);
            }
            else
            {
                BoardCheck.gameclear = true;
                TurnCounting.Instance.bossTrigger++;
            }
        }
    }
}
