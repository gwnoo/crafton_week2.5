using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public Transform[] spawnPoints; // 각 스테이지마다 설정된 스폰 위치들

    // 게임 진행 상황 저장 (예: 마지막으로 도달한 스테이지 번호)
    public void SavePlayerProgress(int stageIndex)
    {
        PlayerPrefs.SetInt("LastStage", stageIndex);  // 스테이지 번호를 PlayerPrefs에 저장
        PlayerPrefs.Save();  // 저장 적용
    }

    // 게임 다시 시작할 때, 진행 상황에 맞는 플레이어 위치로 이동
    public void ResetGame()
    {
        int lastStage = PlayerPrefs.GetInt("LastStage", 0);  // 기본값 0 (처음 시작하는 위치)

        // 스테이지 진행 상황에 맞는 스폰 위치로 플레이어 이동
        if (lastStage >= 0 && lastStage < spawnPoints.Length)
        {
            Transform spawnPoint = spawnPoints[lastStage];
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint.position;
            }
        }

        // 씬을 재시작 (현재 씬 다시 로드)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}