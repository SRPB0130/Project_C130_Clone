using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHP = 100; // �ִ� ü��
    public int currentHP; 

    public Slider HPBar; // UI ü�¹�

    private bool isInvincible = false;
    private void Start()
    {
        currentHP = maxHP;
        UpdateHPUI(); // UI ������Ʈ
    }

    public void TakeDamage(int amount)
    {

        if (isInvincible) return;

        currentHP -= amount; // ü�� ���� 
        currentHP = Mathf.Clamp(currentHP, 0 , maxHP); // ������ 0 ���� �ִ� ü��
        UpdateHPUI();

        if (currentHP < 0) // ü���� 0 ������ ��������
        {
            Die(); // ����
        }

    }
    void Die()
    {
        // ���� ���� �� ���� UI�߰�
    }

    public void Heal(int amount) 
    { 
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPUI();
    }
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }
    void UpdateHPUI()
    {
        if(HPBar != null)
        {
            HPBar.value = (float)currentHP / maxHP; // �����̴� �ٸ� 0 �� 1 ���� ������ ����
        }
    }

}
