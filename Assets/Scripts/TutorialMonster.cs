using System.Collections;
using UnityEngine;

public class TutorialMonster : MonoBehaviour
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
    private float surpriseDelay = 0.3f;
    public LayerMask groundLayer;
    private GameObject bulletTimeManager;

    private float attackCooldown = 0f;
    private bool isAttacking = false;
    private bool isSurprised = false;

    private void Awake()
    {
        bulletTimeManager = GameObject.Find("BulletTimeManager");
    }

    void Update()
    {
        float distanceToPlayer = GetDistanceToPlayer();

        attackCooldown -= Time.deltaTime;

        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(WaitAndAct(surpriseDelay, distanceToPlayer));
        }
    }

    IEnumerator WaitAndAct(float delay, float distanceToPlayer)
    {
        if (!isSurprised)
        {
            yield return new WaitForSeconds(delay);
        }
        isSurprised = true;

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

        isSurprised = false;
    }

    float GetDistanceToPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, groundLayer);

        if (hit.collider != null)
        {
            return detectionRange + 1;
        }

        // 장애물이 없는 경우 실제 거리 반환
        return Vector2.Distance(transform.position, player.position);
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
        //bulletTimeManager.GetComponent<BulletTimeManager>().SlowTime();
    }

    void ShootProjectile()
    {
        Vector3 gunDirection = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;

        // 원거리 공격 발사 (예: 파이어볼)
        if (enemyType == 0)
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
