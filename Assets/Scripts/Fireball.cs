using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 40f;  // ���̾�� �ӵ�
    public float lifetime = 5f; // ���̾�� ���� (�ð� �� ����)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ���̾�� ���� �������� �߻�
        rb.linearVelocity = transform.right * speed;

        // ���� �ð� �� ���̾ ���� (�浹�� �������� �����Ƿ� ���� �ð� �� �ڵ� ����)
        Destroy(gameObject, lifetime);
    }

    // �浹 ó�� (�ɼ�)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ���, ���̾�� ���� �浹�ϸ� �ı�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);  // ���̾ �ı�
            // ������ �������� �ִ� �ڵ� ���� �߰��� �� �ֽ��ϴ�.
        }
    }
}