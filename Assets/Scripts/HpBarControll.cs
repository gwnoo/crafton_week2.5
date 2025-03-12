using UnityEngine;
using UnityEngine.UI;

public class HpBarControll : MonoBehaviour
{
    // HpBar Slider�� �����ϱ� ���� Slider ��ü
    [SerializeField] 
    private Slider _hpBar;

    // �÷��̾��� HP
    private float _hp;

    public float Hp
    {
        get => _hp;
        private set => _hp = _hpBar.value;
    }

    private void Awake()
    {
        _hp = 100;
    }

    public void SetHp(int health, int maxHealth)
    {
        _hpBar.maxValue = maxHealth;
        _hpBar.value = health;
    }

}
