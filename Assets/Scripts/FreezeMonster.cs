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
            enemyController.enabled = false; // �÷��̾� �̵� ��Ȱ��ȭ
            ice.SetActive(true); // ���� Ȱ��ȭ

            yield return new WaitForSeconds(duration); // ���� �ð� ���
            enemyController.enabled = true; // �÷��̾� �̵� Ȱ��ȭ
            ice.SetActive(false); // ���� ��Ȱ��ȭ
        }
    }
}
