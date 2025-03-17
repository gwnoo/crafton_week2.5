using UnityEngine;
using System.Collections;

public class FreezeMonster : MonoBehaviour
{
    private EnemyController enemyController;
    public GameObject ice;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }
    public void Freeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        if (enemyController != null)
        {
            enemyController.enabled = false; // 플레이어 이동 비활성화
            ice.SetActive(true); // 얼음 활성화

            yield return new WaitForSeconds(duration); // 일정 시간 대기
            enemyController.enabled = true; // 플레이어 이동 활성화
            ice.SetActive(false); // 얼음 비활성화
        }
    }
}
