using UnityEngine;
using UnityEngine.UI;

public class HpBarControll : MonoBehaviour
{
    // HpBar Slider를 연동하기 위한 Slider 객체
    [SerializeField] 
    private Slider _hpBar;

    // 플레이어의 HP
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
