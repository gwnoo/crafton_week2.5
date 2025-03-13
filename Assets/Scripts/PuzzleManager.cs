using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private GameObject canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        // 탭 키가 눌렸을 때
        if (Input.GetKey(KeyCode.Tab))
        {
            // Canvas를 활성화
            if (canvas != null)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            // 탭 키를 누르고 있지 않으면 Canvas를 비활성화
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }

}
