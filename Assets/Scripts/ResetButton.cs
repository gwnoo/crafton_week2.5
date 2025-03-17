using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    private Vector3[] spawnPoints =
    {
        new Vector3(-18.9f, 1.9f, 0),
        new Vector3(-21f, -74f, 0),
        new Vector3(-15.5f, -124f, 0),
        new Vector3(-25f, -191f, 0)
    }; // 각 스테이지마다 설정된 스폰 위치들

    private void OnEnable()
    {
        // 씬이 로드될 때마다 ResetPlayerPosition 호출
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬이 로드될 때마다 ResetPlayerPosition 호출을 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로딩 후 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPlayerPosition();
    }

    // 플레이어 위치 재설정
    private void ResetPlayerPosition()
    {
        // PlayerPrefs에서 저장된 스테이지 값 불러오기
        int lastStage = PlayerPrefs.GetInt("Stage", 1);  // 기본값 1 (처음 시작하는 위치)

        // 스테이지 진행 상황에 맞는 스폰 위치로 플레이어 이동
        if (lastStage >= 1 && lastStage <= spawnPoints.Length)
        {
            Vector3 spawnPoint = spawnPoints[lastStage - 1];
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint;
            }
        }
    }

    // 게임을 다시 시작할 때, 씬을 재시작
    public void ResetGame()
    {
        // 씬을 재시작 (현재 씬 다시 로드)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}