using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 20f;
    private Vector2 moveDirection = Vector2.right;
    private float jumpForce = 30f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;
    private bool isDashing = false;

    public GameObject fireballPrefab; 
    public Transform firePoint;     

    public GameObject barrier;
    public float barrierDuration = 2f; 

    private bool isBarrierActive = false;  
    private float barrierTimer = 0f; 

    private float attackRange = 2.5f;  
    private float attackCooldown = 0.5f; 
    private float lastAttackTime = 0f;  
    public LayerMask bulletLayer;

    private float dashSpeed = 50f;      
    private float dashDuration = 0.2f;      
    private float dashCooldown = 0.5f;  
    private float dashTime = 0f;          
    private float lastDashTime = 0f;
    private TrailRenderer trailRenderer;
    public int maxDashCount = 300;           // 최대 회피 가능 횟수
    private int currentDashCount;          // 현재 회피 가능 횟수
    public UnityEngine.UI.Image dashBar;                  // 회피 횟수를 표시할 UI (Image)

    public bool isScaling = false;  // 스케일 변화 여부
    private float scaleTime = 0f;
    private float scaleDuration = 0.1f;
    private float holdDuration = 0.3f;  // 배리어가 잠시 동안 유지될 시간 (초)
    private float holdTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();  // TrailRenderer 컴포넌트 가져오기
        trailRenderer.enabled = false;

        currentDashCount = maxDashCount;
        UpdateDashUI();
    }

    void Update()
    {
        // jump check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetKeyDown(KeyCode.S) && Time.time >= lastDashTime + dashCooldown && currentDashCount > 0 && isGrounded)
        {
            Dash();
        }

        if (!isDashing)
        {
            Move();
        }

        if (isDashing && Time.time >= dashTime)
        {
            isDashing = false;
            trailRenderer.enabled = false;

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bullet"), false);
        }

        // jump
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }


        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartScaleAnimation();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * 1200f, ForceMode2D.Impulse);

        }

        if (isScaling)
        {
            // 점진적으로 스케일을 증가
            scaleTime += Time.deltaTime / scaleDuration;  // scaleDuration 동안 스케일 변화
            float scaleValue = Mathf.Lerp(1f, 5f, scaleTime);  // 0에서 5까지 스케일 변화

            // Barrier의 스케일을 설정
            barrier.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

            // 애니메이션 종료 처리
            if (scaleTime >= 1f)
            {
                isScaling = false;  // 스케일 변화 완료
                holdTime = 0f;  // 유지 시간 초기화
            }
        }
        // 스케일 애니메이션이 끝난 후 배리어가 일정 시간 동안 유지되도록 처리
        else if (holdTime < holdDuration)
        {
            holdTime += Time.deltaTime;  // 배리어가 유지되는 동안 시간 추적
        }
        else if (holdTime >= holdDuration)
        {
            // 일정 시간 후 Barrier를 비활성화
            barrier.SetActive(false);
        }

    }

    private void Move()
    {
        // move left and right
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0)
        {
            moveDirection = Vector2.right; 
        }
        else if (moveInput < 0)
        {
            moveDirection = Vector2.left; 
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(Input.GetKey(KeyCode.S) && !isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -moveSpeed);
        }
    }

    void Dash()
    {
        lastDashTime = Time.time;
        
        currentDashCount--;
        UpdateDashUI();

        dashTime = Time.time + dashDuration;

        isDashing = true;
        trailRenderer.enabled = true;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bullet"), true);

        if (moveDirection == Vector2.right)
        {
            rb.linearVelocity = new Vector2(dashSpeed, rb.linearVelocity.y);
        }
        else if (moveDirection == Vector2.left)
        {
            rb.linearVelocity = new Vector2(-dashSpeed, rb.linearVelocity.y);
        }


    }

    private void UpdateDashUI()
    {
        // 회피 UI 채우기 비율 계산 (남은 회피 횟수 / 최대 회피 횟수)
        float fillAmount = (float)currentDashCount / maxDashCount;
        dashBar.fillAmount = fillAmount;
    }

    public void GetDashCount()
    {
        if(currentDashCount < maxDashCount)
        {
            currentDashCount = maxDashCount;
            UpdateDashUI();
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    public void ShootFireball()
    {
        GameObject newBullet = Instantiate(fireballPrefab, transform.position, Quaternion.identity, transform);

        // 총알이 적을 향하도록 방향 설정
        Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
        if (newBulletRb != null)
        {
            Vector2 directionToEnemy = (FindClosestEnemy().position - transform.position).normalized;
            newBulletRb.linearVelocity = directionToEnemy * 40f;  // 새로운 총알의 속도 설정
        }
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
                    bulletRb.linearVelocity = -bulletRb.linearVelocity;

                    GameObject newBullet = Instantiate(fireballPrefab, bulletRb.transform.position, Quaternion.identity, transform);

                    Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
                    if (newBulletRb != null)
                    {
                        newBulletRb.linearVelocity = bulletRb.linearVelocity;  
                    }

                    Destroy(hit.gameObject);
                }
            }
        }
        StartScaleAnimation();
    }

    void StartScaleAnimation()
    {
        // 스케일 애니메이션 시작 시점
        if (!isScaling)
        {
            barrier.SetActive(true);
            isScaling = true;
            scaleTime = 0f;  // 시간 초기화
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, dashDuration);
    }
}