using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    private bool canDamage = false; // 적에게 데미지를 줄 수 있는지 여부

    private void OnEnable()
    {
        // 오브젝트가 활성화되면 일정 시간 동안만 적에게 데미지를 줄 수 있도록 설정
        StartCoroutine(EnableDamageForDuration(0.3f)); // 2초 동안 데미지를 줄 수 있도록 설정
    }

    private IEnumerator EnableDamageForDuration(float duration)
    {
        canDamage = true; // 데미지를 줄 수 있도록 설정
        yield return new WaitForSeconds(duration); // 일정 시간 대기
        canDamage = false; // 데미지를 줄 수 없도록 설정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().TakeDamage(10);
        }
    }
}