using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; 
    public float detectionRange = 15f; 
    public float attackRange = 10f; 
    public float closeRange = 2f; 
    public float attackDelay = 1f; 
    public GameObject FireballPrefab;
    public GameObject IceballPrefab;
    public Transform attackPoint; 
    public Transform player;
    public int health = 10;
    public int enemyType = 0;

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

        attackCooldown = attackDelay;

        ShootProjectile();
    }

    void ShootProjectile()
    {
        Vector3 gunDirection = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;

        // 원거리 공격 발사 (예: 파이어볼)
        if(enemyType == 0)
        {
            GameObject projectile = Instantiate(FireballPrefab, attackPoint.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            projectile.GetComponent<Fireball>().shooter = gameObject;
        }
        else if (enemyType == 1)
        {
            GameObject projectile = Instantiate(IceballPrefab, attackPoint.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            projectile.GetComponent<Iceball>().shooter = gameObject;
        }
    }

    void AttackCloseRange()
    {
        isAttacking = true;

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
        Destroy(gameObject);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, closeRange);  
    }
}