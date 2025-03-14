using UnityEngine;


public class BarrierManager : MonoBehaviour
{
    public GameObject fireballPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && collision.transform.parent != transform.parent)
        {
            Vector2 attackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D bulletRb = collision.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = -bulletRb.linearVelocity;

                GameObject newBullet = Instantiate(fireballPrefab, bulletRb.transform.position, Quaternion.identity, transform.parent);

                Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
                if (newBulletRb != null)
                {
                    newBulletRb.linearVelocity = bulletRb.linearVelocity;
                }

                Destroy(collision.gameObject);
            }
        }
    }
}
