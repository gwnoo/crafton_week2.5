using UnityEngine;

public class CliffDash : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 'cliff' �±׸� ���� ������Ʈ�� �浹�� ��
        if (collision.gameObject.CompareTag("dash"))
        {
            // �浹 ����
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("dash"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
