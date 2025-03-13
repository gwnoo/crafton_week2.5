using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 40f;  // 파이어볼의 속도
    public float lifetime = 2f; // 파이어볼의 수명 (시간 후 삭제)
    public LayerMask damageLayer;  // 데미지를 줄 대상 레이어 (예: Player)
    public GameObject shooter;  // 발사자(적 또는 플레이어)


    private Rigidbody2D rb;

    void Start()
    {
        // 일정 시간 후 파이어볼 삭제 (충돌을 감지하지 않으므로 일정 시간 후 자동 삭제)
        Destroy(gameObject, lifetime);

        // 발사자 설정이 되어 있지 않다면 이 객체의 부모(발사자)로 설정
        if (shooter == null)
        {
            shooter = transform.parent?.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != shooter)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
            {
                // 여기서 총알에 대한 처리를 할 수 있습니다 (예: 데미지, 효과 등)
                Destroy(gameObject);  // 충돌한 후 총알 삭제
            }
        }
    }
}