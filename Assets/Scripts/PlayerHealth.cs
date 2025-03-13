using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;  // �÷��̾��� ü��

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // �÷��̾ �׾��� �� ó���ϴ� �ڵ�
        Debug.Log("Player died!");
        // ���� ���, ���� ���� ó���� ������ ���� �߰��� �� �ֽ��ϴ�.
    }
}