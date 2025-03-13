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
        // �� Ű�� ������ ��
        if (Input.GetKey(KeyCode.Tab))
        {
            // Canvas�� Ȱ��ȭ
            if (canvas != null)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            // �� Ű�� ������ ���� ������ Canvas�� ��Ȱ��ȭ
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }

}
