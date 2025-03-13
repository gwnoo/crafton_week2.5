using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;

    public GameObject fireballPrefab; 
    public Transform firePoint;     

    public GameObject barrier;
    public float barrierDuration = 2f; 

    private bool isBarrierActive = false;  
    private float barrierTimer = 0f; 

    public float attackRange = 1.5f;  
    public float attackCooldown = 0.5f; 
    private float lastAttackTime = 0f;  
    public LayerMask bulletLayer; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // jump check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // move left and right
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // jump
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }


        if (isBarrierActive)
        {
            barrierTimer += Time.deltaTime; 

            if (barrierTimer >= barrierDuration)
            {
                DeactivateBarrier();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformMeleeAttack();
        }

    }

    public void ShootFireball()
    {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }

    public void ActivateBarrier()
    {
        barrier.SetActive(true);  
        isBarrierActive = true; 
        barrierTimer = 0f; 
    }

    private void DeactivateBarrier()
    {
        barrier.SetActive(false); 
        isBarrierActive = false;
    }

    void PerformMeleeAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, bulletLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Bullet"))
            {
                Vector2 attackDirection = (hit.transform.position - transform.position).normalized;
                Rigidbody2D bulletRb = hit.GetComponent<Rigidbody2D>();
                bulletRb.transform.SetParent(transform);

                if (bulletRb != null)
                {
                    // 총알의 이동 방향을 반대로 튕겨내기
                    bulletRb.linearVelocity = -bulletRb.linearVelocity;

                    // 새로 튕겨낸 총알의 속도 유지 및 반대 방향으로 발사
                    GameObject newBullet = Instantiate(fireballPrefab, bulletRb.transform.position, Quaternion.identity, transform);

                    // 새 총알에 물리적 속도 부여
                    Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
                    if (newBulletRb != null)
                    {
                        newBulletRb.linearVelocity = bulletRb.linearVelocity;  // 기존 총알의 속도 유지
                    }

                    // 기존 총알 삭제
                    Destroy(hit.gameObject);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}