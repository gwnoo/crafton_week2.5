using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    private int cameraStage = 0;
    public int stage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;

        // PlayerPrefs���� ������ ����� stage�� �ҷ��ɴϴ�.
        stage = PlayerPrefs.GetInt("Stage", 1); // �⺻���� 1�� ����
        cameraStage = stage - 1; // stage�� �°� ī�޶� �ʱ�ȭ
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (stage)
        {
            case 1:
                FollowPlayer1();
                break;
            case 2:
                FollowPlayer2();
                break;
            case 3:
                FollowPlayer3();
                break;
            case 4:
                FollowPlayer4();
                break;
            default:
                FollowPlayer4();
                break;
        }
    }

    private void FollowPlayer1()
    {
        if (cameraStage == 0)
        {
            transform.position = new Vector3(0, -13.68f, -36.25f);
            cameraStage++;
            SaveProgress(cameraStage);
        }

        if (player.position.x > 0 && player.position.x < 54)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }

        if (player.position.y < -13.68f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    private void FollowPlayer2()
    {
        if (cameraStage == 1)
        {
            transform.position = new Vector3(-4f, -74f, -36.25f);
            cameraStage++;
            SaveProgress(cameraStage);
        }

        if (player.position.x > -4f && player.position.x < 6)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }

        if (player.position.y > -74f && player.position.y < -66f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    private void FollowPlayer3()
    {
        if (cameraStage == 2)
        {
            transform.position = new Vector3(2f, -126f, -34.25f);
            cameraStage++;
            SaveProgress(cameraStage);
        }
        if (player.position.x > 2f && player.position.x < 48f)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }

        if (player.position.y > -126f && player.position.y < -111f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    private void FollowPlayer4()
    {
        if (cameraStage == 3)
        {
            transform.position = new Vector3(-7.4f, -191f, -34.25f);
            cameraStage++;
            SaveProgress(cameraStage);
        }
        if (player.position.x > -7.4f && player.position.x < 21.12f)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
        if (player.position.y > -191f && player.position.y < -174f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    // ���� ���� ��Ȳ�� �����ϴ� �Լ�
    public void SaveProgress(int currentStage)
    {
        stage = currentStage; // ���� ���� ���������� ����
        PlayerPrefs.SetInt("Stage", stage); // PlayerPrefs�� ����
        PlayerPrefs.Save(); // ���� �Ϸ�
        Debug.Log(PlayerPrefs.GetInt("Stage"));
    }
}