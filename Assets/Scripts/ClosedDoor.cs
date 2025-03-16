using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    private DoorManager doorManager;

    private void Awake()
    {
        doorManager = GetComponentInParent<DoorManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Dash")
        {
            doorManager.playerIsNear = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Dash")
        {
            doorManager.playerIsNear = false;
        }
    }
}
