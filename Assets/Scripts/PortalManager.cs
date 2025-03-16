using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Transform targetPortal; // 텔레포트될 포탈의 위치

    private bool isPlayerInRange = false; // 플레이어가 포탈 근처에 있는지 체크
    private Transform player; // 플레이어의 트랜스폼
    private GameObject cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GameObject.Find("CinemachineCamera");
    }



    private void Update()
    {
        // 플레이어가 포탈 근처에 있을 때 상호작용 키를 눌러야 텔레포트
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        // 플레이어가 포탈에 접근하면 상호작용 가능
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range");
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 포탈을 떠나면 상호작용 불가능
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
            // 플레이어를 다른 포탈 위치로 이동시킴
            player.position = targetPortal.position;
            cinemachineCamera.GetComponent<FollowPlayer>().stage++;
            // 텔레포트 효과 (선택사항, 이펙트나 소리 등 추가 가능)
        }
    }
}