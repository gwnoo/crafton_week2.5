using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool playerIsNear = false;
    public int doorType = 0;

    public void Update()
    {
        if (Input.GetKey(KeyCode.A) && playerIsNear && doorType == 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.D) && playerIsNear && doorType == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

}
