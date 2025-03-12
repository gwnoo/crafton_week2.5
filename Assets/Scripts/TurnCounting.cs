using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TurnCounting : MonoBehaviour
{
    //�̱�������
    public static TurnCounting Instance;

    [SerializeField]
    LevelUpEffect levelUpEffect;
    public int turnCount;
    public int limitTurn;
    public int firstLimitTurn;
    public int goalScore;
    public int turnScore;
    public int breakTurn;
    public int lastbossTrigger;
    public int bossTrigger;
    private int firstGoalScore;
    private int increaseMultiplier = 2;
    private GameObject monsterManager;

    private int level = 1;

    [SerializeField] private TextMeshProUGUI limitTurnText;
    [SerializeField] private TextMeshProUGUI goalScoreText;

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���� �� �������� �ʵ��� ����
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }

        firstLimitTurn = limitTurn;
        firstGoalScore = goalScore;
        turnScore = 0;

        monsterManager = GameObject.Find("MonsterManager");
        UpdateScene();
    }

    // testcode
    /*private void Update()
    {
        if(Input.GetKeyDown("x")){
            levelUpEffect.CrackerShoot(level);
            //level++;
        }
    }*/

    // ���� �ε�� �� ���� �ʱ�ȭ
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIElements(); // �ؽ�Ʈ�������� ��� �Ҵ�
        ResetVariables(); // ������ �⺻������ �ʱ�ȭ
        UpdateScene();
    }

    // ���� �ʱ�ȭ �޼���
    private void ResetVariables()
    {
        turnCount = 0;
        level = 1;
        limitTurn = firstLimitTurn;
        goalScore = firstGoalScore;
        turnScore = 0;
        increaseMultiplier = 2;
        breakTurn = 0;
        bossTrigger = 0;
        lastbossTrigger = 0;
        monsterManager = GameObject.Find("MonsterManager");
    }

    private void AssignUIElements()
    {
        limitTurnText = GameObject.Find("TurnText")?.GetComponent<TextMeshProUGUI>();
        goalScoreText = GameObject.Find("GoalText")?.GetComponent<TextMeshProUGUI>();
    }

    public void CheckTurnAndGoal()
    {
        UpdateScene();
        
        if (turnCount >= limitTurn)
        {
            if(turnScore < goalScore)
            {
                //game over
                BoardCheck.gameover = true;
            }
            else
            {
                //����
                limitTurn += 2;
                turnCount = 0;
                goalScore = firstGoalScore * increaseMultiplier;
                turnScore = 0;
                breakTurn = Random.Range(3, 7);
                if (increaseMultiplier < 30)
                {
                    increaseMultiplier += 1;
                }

                levelUpEffect.CrackerShoot(level);
                level++;
                SoundManager.Instance.PlayLevelUpSound();
            }

            if (monsterManager.GetComponent<MonsterSpawner>().CheckNextMonster() == 1 || (monsterManager.GetComponent<MonsterSpawner>().CheckNextMonster() == 3 && monsterManager.GetComponent<MonsterSpawner>().buckShotMode == 1))
            {
                limitTurn *= 2;
                goalScore *= 10;
            }
        }
    }

    //�ؽ�Ʈ ����
    public void UpdateScene()
    {
        monsterManager.GetComponent<MonsterSpawner>().UpdateMonster();
        limitTurnText.text = "Turn : " + turnCount + " / " + limitTurn;
        goalScoreText.text = "Goal : " + goalScore;

    }
}
