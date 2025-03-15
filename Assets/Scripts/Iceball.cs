using UnityEngine;

public class Iceball : MonoBehaviour
{
    public float lifetime = 2f; // ���̾�� ���� (�ð� �� ����)
    public LayerMask damageLayer;  // �������� �� ��� ���̾� (��: Player)
    public GameObject shooter;  // �߻���(�� �Ǵ� �÷��̾�)
    public LineRenderer lineRenderer;


    private Rigidbody2D rb;
    private Vector2 lastPosition;
    public Color startColor = Color.blue;  // ������ ���� ����
    public Color endColor = Color.white;  // ������ �� ����

    void Start()
    {
        // ���� �ð� �� ���̾ ���� (�浹�� �������� �����Ƿ� ���� �ð� �� �ڵ� ����)
        Destroy(gameObject, lifetime);

        // �߻��� ������ �Ǿ� ���� �ʴٸ� �� ��ü�� �θ�(�߻���)�� ����
        if (shooter == null)
        {
            shooter = transform.parent?.gameObject;
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // ���� ������ �ʱ�ȭ
            lineRenderer.startWidth = 0; // ���� ���� �ʺ�
            lineRenderer.endWidth = 0.1f; // ���� �� �ʺ�

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }

        lastPosition = transform.position; // ù ��ġ ����
    }

    void Update()
    {

        // ���� �������� ���� ��ġ�� �߰�
        if (lineRenderer != null)
        {
            // ���� ������ ��ġ �߰�
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != shooter)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
            {
                // ���⼭ �Ѿ˿� ���� ó���� �� �� �ֽ��ϴ� (��: ������, ȿ�� ��)
                Destroy(gameObject);  // �浹�� �� �Ѿ� ����
            }
        }
    }
}