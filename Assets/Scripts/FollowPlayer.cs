using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x > 0)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        if (player.position.y < -13.68)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }

    }
}
