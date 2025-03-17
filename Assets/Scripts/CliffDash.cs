using UnityEngine;

public class CliffDash : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 'cliff' 태그를 가진 오브젝트와 충돌할 때
        if (collision.gameObject.CompareTag("dash"))
        {
            // 충돌 무시
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
