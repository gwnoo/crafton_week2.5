using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;  // 플레이어의 체력

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
        // 플레이어가 죽었을 때 처리하는 코드
        Debug.Log("Player died!");
        // 예를 들어, 게임 오버 처리나 리스폰 등을 추가할 수 있습니다.
    }
}