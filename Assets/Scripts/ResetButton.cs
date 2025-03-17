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
    }; // �� ������������ ������ ���� ��ġ��

    private void OnEnable()
    {
        // ���� �ε�� ������ ResetPlayerPosition ȣ��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ���� �ε�� ������ ResetPlayerPosition ȣ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �� �ε� �� ȣ��Ǵ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPlayerPosition();
    }

    // �÷��̾� ��ġ �缳��
    private void ResetPlayerPosition()
    {
        // PlayerPrefs���� ����� �������� �� �ҷ�����
        int lastStage = PlayerPrefs.GetInt("Stage", 1);  // �⺻�� 1 (ó�� �����ϴ� ��ġ)

        // �������� ���� ��Ȳ�� �´� ���� ��ġ�� �÷��̾� �̵�
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

    // ������ �ٽ� ������ ��, ���� �����
    public void ResetGame()
    {
        // ���� ����� (���� �� �ٽ� �ε�)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}