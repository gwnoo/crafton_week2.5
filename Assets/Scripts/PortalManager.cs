using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Transform targetPortal; // 텔레포트될 포탈의 위치
    public GameObject[] monsters; // 스테이지의 모든 몬스터

    private bool isPlayerInRange = false; // 플레이어가 포탈 근처에 있는지 체크
    private bool isPortalActive = false; // 포탈이 활성화되었는지 여부
    private Transform player; // 플레이어의 트랜스폼
    private GameObject cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GameObject.Find("CinemachineCamera");
    }

    void Update()
    {
        // 스테이지의 모든 몬스터가 제거되었는지 확인
        CheckMonsters();

        // 포탈이 활성화되고 플레이어가 포탈 근처에 있을 때 상호작용 키를 눌러야 텔레포트
        if (isPortalActive && isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TeleportPlayer();
        }
    }

    private void CheckMonsters()
    {
        // 모든 몬스터가 제거되었는지 확인
        foreach (GameObject monster in monsters)
        {
            if (monster != null)
            {
                return; // 아직 제거되지 않은 몬스터가 있음
            }
        }

        // 모든 몬스터가 제거되었으면 포탈 활성화
        isPortalActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 포탈에 접근하면 상호작용 가능
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
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