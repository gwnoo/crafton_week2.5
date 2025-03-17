using UnityEngine;

public class LaserMove : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public float moveRange = 5f; // 이동 범위

    private Vector3 startPosition;
    private int moveDirection = 1; // 이동 방향 (1: 오른쪽, -1: 왼쪽)

    void Start()
    {
        startPosition = transform.position; // 시작 위치 저장
    }

    void Update()
    {
        // 좌우로 이동
        transform.position += Vector3.right * moveSpeed * moveDirection * Time.deltaTime;

        // 이동 범위를 벗어나면 방향 반전
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