using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 40f;  // ���̾�� �ӵ�
    public float lifetime = 2f; // ���̾�� ���� (�ð� �� ����)
    public LayerMask damageLayer;  // �������� �� ��� ���̾� (��: Player)
    public GameObject shooter;  // �߻���(�� �Ǵ� �÷��̾�)


    private Rigidbody2D rb;

    void Start()
    {
        // ���� �ð� �� ���̾ ���� (�浹�� �������� �����Ƿ� ���� �ð� �� �ڵ� ����)
        Destroy(gameObject, lifetime);

        // �߻��� ������ �Ǿ� ���� �ʴٸ� �� ��ü�� �θ�(�߻���)�� ����
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
                // ���⼭ �Ѿ˿� ���� ó���� �� �� �ֽ��ϴ� (��: ������, ȿ�� ��)
                Destroy(gameObject);  // �浹�� �� �Ѿ� ����
            }
        }
    }
}