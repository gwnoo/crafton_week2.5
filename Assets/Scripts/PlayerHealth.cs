using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;  // 플레이어의 체력
    [SerializeField]
    private TextMeshProUGUI gameOverTxt;
    private PlayerController playerController;
    public GameObject ice;
    private GameObject reset;

    void Awake()
    {
        playerController = GetComponent<PlayerController>(); // PlayerController 컴포넌트 가져오기
        reset = GameObject.Find("ResetButton");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        if (playerController != null)
        {
            playerController.enabled = false; // 플레이어 이동 비활성화
            ice.SetActive(true); // 얼음 활성화

            yield return new WaitForSeconds(duration); // 일정 시간 대기
            playerController.enabled = true; // 플레이어 이동 활성화
            ice.SetActive(false); // 얼음 비활성화
        }
    }

    void Die()
    {
        SoundManager.Instance.PlayGameOverSound();
        reset.GetComponent<ResetButton>().ResetGame();
        //gameOverTxt.gameObject.SetActive(true);
        //gameOverTxt.text = "GameOver";
    }
}