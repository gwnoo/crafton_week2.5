using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    public int stage = 1;
    private int cameraStage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(stage)
        {
            case 1:
                FollowPlayer1();
                break;
            case 2:
                FollowPlayer2();
                break;
            case 3:
                FollowPlayer3();
                break;
        }
        

    }

    void FollowPlayer1()
    {
        if(cameraStage == 0)
        {
            transform.position = new Vector3(0, -13.68f, -36.25f);
            cameraStage++;
        }

        if (player.position.x > 0 && player.position.x < 54)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }

        if (player.position.y < -13.68f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    void FollowPlayer2()
    {
        if(cameraStage == 1)
        {
            transform.position = new Vector3(-4f, -74f, -36.25f);
            cameraStage++;
        }

        if (player.position.x > -4f && player.position.x < 6)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }

        if (player.position.y > -74f && player.position.y < -66f)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }


    }

    void FollowPlayer3()
    {
        if(cameraStage == 2)
        {
            //transform.position = new Vector3(-4f, -74f, -36.25f);
            cameraStage++;
        }

        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
