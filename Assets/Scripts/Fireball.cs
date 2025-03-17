using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime = 2f; // 파이어볼의 수명 (시간 후 삭제)
    public GameObject shooter;  // 발사자(적 또는 플레이어)
    public LineRenderer lineRenderer;
    private float speed = 100f;


    private Rigidbody2D rb;
    private Vector2 lastPosition;
    public Color startColor = Color.yellow;  // 궤적의 시작 색상
    public Color endColor = Color.red;  // 궤적의 끝 색상
    private Vector3 previousPosition;


    void Start()
    {
        // 일정 시간 후 파이어볼 삭제 (충돌을 감지하지 않으므로 일정 시간 후 자동 삭제)
        Destroy(gameObject, lifetime);
        previousPosition = transform.position;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 라인 렌더러 초기화
            lineRenderer.startWidth = 0; // 라인 시작 너비
            lineRenderer.endWidth = 0.1f; // 라인 끝 너비

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        // 총알이 이동한 현재 위치
        Vector3 currentPosition = transform.position;

        // 레이캐스트로 충돌 확인
        if (gameObject != null)
        {
            CheckCollision(previousPosition, currentPosition);
        }

        // 이전 위치를 현재 위치로 갱신
        previousPosition = currentPosition;

        // 총알 이동
        rb.linearVelocity = transform.right * speed;

        // 라인 렌더러에 현재 위치를 추가
        if (lineRenderer != null)
        {
            // 라인 렌더러 위치 추가
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
        }
    }


    void CheckCollision(Vector3 start, Vector3 end)
    {
        // 이전 위치에서 현재 위치로 레이캐스트 발사
        RaycastHit2D hit;
        Vector3 direction = end - start;  // 총알의 이동 방향

        hit = Physics2D.Raycast(start, direction, direction.magnitude);
        if (hit.collider != null)
        {
            // 충돌한 오브젝트가 있다면
            if (hit.collider.gameObject.tag != shooter.tag)
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
                {
                    if(hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<PlayerHealth>().TakeDamage(10);
                    }
                    else if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<EnemyController>().TakeDamage(10);
                    }
                    Destroy(gameObject);
                }
                if (hit.collider.CompareTag("Barrier"))
                {
                    transform.position = hit.point;
                    Vector3 gunDirection = (shooter.transform.position - transform.position).normalized;
                    float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    shooter = hit.collider.gameObject;
                }
            }
        }
    }
}