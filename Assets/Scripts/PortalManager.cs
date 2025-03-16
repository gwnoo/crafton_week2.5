using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Transform targetPortal; // �ڷ���Ʈ�� ��Ż�� ��ġ

    private bool isPlayerInRange = false; // �÷��̾ ��Ż ��ó�� �ִ��� üũ
    private Transform player; // �÷��̾��� Ʈ������
    private GameObject cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GameObject.Find("CinemachineCamera");
    }



    private void Update()
    {
        // �÷��̾ ��Ż ��ó�� ���� �� ��ȣ�ۿ� Ű�� ������ �ڷ���Ʈ
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        // �÷��̾ ��Ż�� �����ϸ� ��ȣ�ۿ� ����
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range");
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