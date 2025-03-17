using UnityEngine;

public class LaserMove : MonoBehaviour
{
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float moveRange = 5f; // �̵� ����

    private Vector3 startPosition;
    private int moveDirection = 1; // �̵� ���� (1: ������, -1: ����)

    void Start()
    {
        startPosition = transform.position; // ���� ��ġ ����
    }

    void Update()
    {
        // �¿�� �̵�
        transform.position += Vector3.right * moveSpeed * moveDirection * Time.deltaTime;

        // �̵� ������ ����� ���� ����
        if (transform.position.x - startPosition.x >= moveRange)
        {
            moveDirection = -1;
        }
        else if (transform.position.x - startPosition.x <= -moveRange)
        {
            moveDirection = 1;
        }
    }
}