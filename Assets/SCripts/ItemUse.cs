using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUse : MonoBehaviour
{
    public int healingAmount = 20;           // ȸ���� (�ۺ����� ���� ����)
    public int currentItemCount = 0;         // ���� ������ ����
    public Text itemCountText;               // UI �ؽ�Ʈ (�κ��丮 ǥ��)
    public PlayerHP playerHP;        // ü�� ȸ���� ���� ������Ʈ ����

    void Update()
    {
        // E Ű�� ������ ���
        if (Input.GetKeyDown(KeyCode.E) && currentItemCount > 0)
        {
            UseItem();
        }
    }

    public void AddItem(int count = 1)
    {
        currentItemCount += count;
        UpdateUI();
    }

    void UseItem()
    {
        currentItemCount--;
        UpdateUI();

        // ü�� ȸ��
        if (playerHP != null)
        {
            playerHP.Heal(healingAmount);
        }
    }

    void UpdateUI()
    {
        if (itemCountText != null)
        {
            itemCountText.text = "x " + currentItemCount;
        }
    }
}
