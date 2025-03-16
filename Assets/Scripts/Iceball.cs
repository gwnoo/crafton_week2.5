using UnityEngine;

public class Iceball : MonoBehaviour
{
    public float lifetime = 2f; // ���̾�� ���� (�ð� �� ����)
    public GameObject shooter;  // �߻���(�� �Ǵ� �÷��̾�)
    public LineRenderer lineRenderer;
    private float speed = 100f;


    private Rigidbody2D rb;
    private Vector2 lastPosition;
    public Color startColor = Color.blue;  // ������ ���� ����
    public Color endColor = Color.white;  // ������ �� ����
    private Vector3 previousPosition;


    void Start()
    {
        // ���� �ð� �� ���̾ ���� (�浹�� �������� �����Ƿ� ���� �ð� �� �ڵ� ����)
        Destroy(gameObject, lifetime);
        previousPosition = transform.position;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // ���� ������ �ʱ�ȭ
            lineRenderer.startWidth = 0; // ���� ���� �ʺ�
            lineRenderer.endWidth = 0.1f; // ���� �� �ʺ�

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        // �Ѿ��� �̵��� ���� ��ġ
        Vector3 currentPosition = transform.position;

        // ����ĳ��Ʈ�� �浹 Ȯ��
        CheckCollision(previousPosition, currentPosition);

        // ���� ��ġ�� ���� ��ġ�� ����
        previousPosition = currentPosition;

        // �Ѿ� �̵�
        rb.linearVelocity = transform.right * speed;

        // ���� �������� ���� ��ġ�� �߰�
        if (lineRenderer != null)
        {
            // ���� ������ ��ġ �߰�
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
        }
    }


    void CheckCollision(Vector3 start, Vector3 end)
    {
        // ���� ��ġ���� ���� ��ġ�� ����ĳ��Ʈ �߻�
        RaycastHit2D hit;
        Vector3 direction = end - start;  // �Ѿ��� �̵� ����

        hit = Physics2D.Raycast(start, direction, direction.magnitude);
        if (hit.collider != null)
        {
            // �浹�� ������Ʈ�� �ִٸ�
            if (hit.collider.gameObject.tag != shooter.tag)
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<PlayerHealth>().Freeze(2f);
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
                    Debug.Log(angle);
                    shooter = hit.collider.gameObject;
                }
            }
        }
    }
}