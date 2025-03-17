using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    private bool canDamage = false; // ������ �������� �� �� �ִ��� ����

    private void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�Ǹ� ���� �ð� ���ȸ� ������ �������� �� �� �ֵ��� ����
        StartCoroutine(EnableDamageForDuration(0.3f)); // 2�� ���� �������� �� �� �ֵ��� ����
    }

    private IEnumerator EnableDamageForDuration(float duration)
    {
        canDamage = true; // �������� �� �� �ֵ��� ����
        yield return new WaitForSeconds(duration); // ���� �ð� ���
        canDamage = false; // �������� �� �� ������ ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().TakeDamage(10);
        }
    }
}