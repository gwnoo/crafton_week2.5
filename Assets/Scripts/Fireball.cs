using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 40f;  // 파이어볼의 속도
    public float lifetime = 5f; // 파이어볼의 수명 (시간 후 삭제)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 파이어볼을 직선 방향으로 발사
        rb.linearVelocity = transform.right * speed;

        // 일정 시간 후 파이어볼 삭제 (충돌을 감지하지 않으므로 일정 시간 후 자동 삭제)
        Destroy(gameObject, lifetime);
    }

    // 충돌 처리 (옵션)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 예를 들어, 파이어볼이 적과 충돌하면 파괴
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);  // 파이어볼 파괴
            // 적에게 데미지를 주는 코드 등을 추가할 수 있습니다.
        }
    }
}