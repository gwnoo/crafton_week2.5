using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;  // �÷��̾��� ü��
    [SerializeField]
    private TextMeshProUGUI gameOverTxt;
    private PlayerController playerController;
    public GameObject ice;
    private GameObject reset;

    void Awake()
    {
        playerController = GetComponent<PlayerController>(); // PlayerController ������Ʈ ��������
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
            playerController.enabled = false; // �÷��̾� �̵� ��Ȱ��ȭ
            ice.SetActive(true); // ���� Ȱ��ȭ

            yield return new WaitForSeconds(duration); // ���� �ð� ���
            playerController.enabled = true; // �÷��̾� �̵� Ȱ��ȭ
            ice.SetActive(false); // ���� ��Ȱ��ȭ
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