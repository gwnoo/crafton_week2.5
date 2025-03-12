using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TurnCounting : MonoBehaviour
{
    //싱글톤패턴
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
        // 싱글톤 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시 삭제되지 않도록 설정
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // 중복 방지
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

    // 씬이 로드될 때 변수 초기화
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIElements(); // 텍스트누락방지 요소 할당
        ResetVariables(); // 변수를 기본값으로 초기화
        UpdateScene();
    }

    // 변수 초기화 메서드
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
                //갱신
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

    //텍스트 갱신
    public void UpdateScene()
    {
        monsterManager.GetComponent<MonsterSpawner>().UpdateMonster();
        limitTurnText.text = "Turn : " + turnCount + " / " + limitTurn;
        goalScoreText.text = "Goal : " + goalScore;

    }
}
