using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public Transform[] spawnPoints; // �� ������������ ������ ���� ��ġ��

    // ���� ���� ��Ȳ ���� (��: ���������� ������ �������� ��ȣ)
    public void SavePlayerProgress(int stageIndex)
    {
        PlayerPrefs.SetInt("LastStage", stageIndex);  // �������� ��ȣ�� PlayerPrefs�� ����
        PlayerPrefs.Save();  // ���� ����
    }

    // ���� �ٽ� ������ ��, ���� ��Ȳ�� �´� �÷��̾� ��ġ�� �̵�
    public void ResetGame()
    {
        int lastStage = PlayerPrefs.GetInt("LastStage", 0);  // �⺻�� 0 (ó�� �����ϴ� ��ġ)

        // �������� ���� ��Ȳ�� �´� ���� ��ġ�� �÷��̾� �̵�
        if (lastStage >= 0 && lastStage < spawnPoints.Length)
        {
            Transform spawnPoint = spawnPoints[lastStage];
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint.position;
            }
        }

        // ���� ����� (���� �� �ٽ� �ε�)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}