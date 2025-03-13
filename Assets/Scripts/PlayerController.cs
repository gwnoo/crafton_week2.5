using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;

    public GameObject fireballPrefab;  // 파이어볼 프리팹
    public Transform firePoint;        // 파이어볼이 발사될 위치 (플레이어의 손이나 앞쪽)

    public GameObject barrier;  // 배리어 오브젝트
    public float barrierDuration = 2f;  // 배리어 지속 시간

    private bool isBarrierActive = false;  // 배리어가 활성화 되었는지 체크
    private float barrierTimer = 0f;  // 배리어가 활성화된 시간

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
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }


        // 배리어가 활성화된 후 시간 체크
        if (isBarrierActive)
        {
            barrierTimer += Time.deltaTime; // 시간 증가

            // 배리어가 2초 후 비활성화
            if (barrierTimer >= barrierDuration)
            {
                DeactivateBarrier();
            }
        }
    }

    public void ShootFireball()
    {
        // 파이어볼 생성 (발사 위치에서 생성)
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }

    public void ActivateBarrier()
    {
        barrier.SetActive(true);  // 배리어 활성화
        isBarrierActive = true;  // 배리어가 활성화된 상태로 변경
        barrierTimer = 0f;  // 타이머 초기화
    }

    private void DeactivateBarrier()
    {
        barrier.SetActive(false);  // 배리어 비활성화
        isBarrierActive = false;  // 배리어가 비활성화된 상태로 변경
    }
}