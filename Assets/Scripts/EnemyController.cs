using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; 
    public float detectionRange = 15f; 
    public float attackRange = 10f; 
    public float closeRange = 2f; 
    public float attackDelay = 1f; 
    public GameObject projectilePrefab; 
    public Transform attackPoint; 
    public Transform player; 

    private float attackCooldown = 0f; 
    private bool isAttacking = false; 

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        attackCooldown -= Time.deltaTime;

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= closeRange && attackCooldown <= 0f)
            {
                if (!isAttacking)
                {
                    AttackCloseRange();
                }
            }
            else if (distanceToPlayer <= attackRange && attackCooldown <= 0f)
            {
                if (!isAttacking)
                {
                    AttackRanged();
                }
            }
            else if (distanceToPlayer >= attackRange)
            {
                MoveTowardPlayer();
            }
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackRanged()
    {
        Debug.Log("Attacking Player with ranged attack!");

        attackCooldown = attackDelay;

        ShootProjectile();
    }

    void ShootProjectile()
    {
        // 원거리 공격 발사 (예: 파이어볼)
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity, transform);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // 공격 방향 계산 (플레이어 방향으로 발사)
        Vector2 direction = (player.position - attackPoint.position).normalized;

        // 발사된 공격에 속도 부여
        rb.linearVelocity = direction * 40f;  // 속도 조절
    }

    void AttackCloseRange()
    {
        isAttacking = true;
        Debug.Log("Attacking Player with close range attack!");

        attackCooldown = attackDelay;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); 
        }

        Invoke("ResetAttackState", attackDelay);
    }

    
    void ResetAttackState()
    {
        isAttacking = false;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, closeRange);  
    }
}