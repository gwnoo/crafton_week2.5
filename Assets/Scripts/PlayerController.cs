using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;

    public GameObject fireballPrefab;  // ���̾ ������
    public Transform firePoint;        // ���̾�� �߻�� ��ġ (�÷��̾��� ���̳� ����)

    public GameObject barrier;  // �踮�� ������Ʈ
    public float barrierDuration = 2f;  // �踮�� ���� �ð�

    private bool isBarrierActive = false;  // �踮� Ȱ��ȭ �Ǿ����� üũ
    private float barrierTimer = 0f;  // �踮� Ȱ��ȭ�� �ð�

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


        // �踮� Ȱ��ȭ�� �� �ð� üũ
        if (isBarrierActive)
        {
            barrierTimer += Time.deltaTime; // �ð� ����

            // �踮� 2�� �� ��Ȱ��ȭ
            if (barrierTimer >= barrierDuration)
            {
                DeactivateBarrier();
            }
        }
    }

    public void ShootFireball()
    {
        // ���̾ ���� (�߻� ��ġ���� ����)
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }

    public void ActivateBarrier()
    {
        barrier.SetActive(true);  // �踮�� Ȱ��ȭ
        isBarrierActive = true;  // �踮� Ȱ��ȭ�� ���·� ����
        barrierTimer = 0f;  // Ÿ�̸� �ʱ�ȭ
    }

    private void DeactivateBarrier()
    {
        barrier.SetActive(false);  // �踮�� ��Ȱ��ȭ
        isBarrierActive = false;  // �踮� ��Ȱ��ȭ�� ���·� ����
    }
}