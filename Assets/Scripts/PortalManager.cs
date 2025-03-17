using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Transform targetPortal; // �ڷ���Ʈ�� ��Ż�� ��ġ
    public GameObject[] monsters; // ���������� ��� ����

    private bool isPlayerInRange = false; // �÷��̾ ��Ż ��ó�� �ִ��� üũ
    private bool isPortalActive = false; // ��Ż�� Ȱ��ȭ�Ǿ����� ����
    private Transform player; // �÷��̾��� Ʈ������
    private GameObject cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GameObject.Find("CinemachineCamera");
    }

    void Update()
    {
        // ���������� ��� ���Ͱ� ���ŵǾ����� Ȯ��
        CheckMonsters();

        // ��Ż�� Ȱ��ȭ�ǰ� �÷��̾ ��Ż ��ó�� ���� �� ��ȣ�ۿ� Ű�� ������ �ڷ���Ʈ
        if (isPortalActive && isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TeleportPlayer();
        }
    }

    private void CheckMonsters()
    {
        // ��� ���Ͱ� ���ŵǾ����� Ȯ��
        foreach (GameObject monster in monsters)
        {
            if (monster != null)
            {
                return; // ���� ���ŵ��� ���� ���Ͱ� ����
            }
        }

        // ��� ���Ͱ� ���ŵǾ����� ��Ż Ȱ��ȭ
        isPortalActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ ��Ż�� �����ϸ� ��ȣ�ۿ� ����
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // �÷��̾ ��Ż�� ������ ��ȣ�ۿ� �Ұ���
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    private void TeleportPlayer()
    {
        if (player != null && targetPortal != null)
        {
            // �÷��̾ �ٸ� ��Ż ��ġ�� �̵���Ŵ
            player.position = targetPortal.position;
            cinemachineCamera.GetComponent<FollowPlayer>().stage++;
            // �ڷ���Ʈ ȿ�� (���û���, ����Ʈ�� �Ҹ� �� �߰� ����)
        }
    }
}