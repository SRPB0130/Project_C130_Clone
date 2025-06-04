using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public int itemCount = 0;
    public Text itemCountText; // UI ����

    private void Start()
    {
        UpdateItemUI(); // ���� ���۽� ������UI �ʱ�ȭ
    }

    public void AddItem(int amount = 1)
    {
        itemCount += amount;
        UpdateItemUI(); // ������ ���������� ui ������Ʈ
    }

    void UpdateItemUI()
    {
        if (itemCountText != null)
            itemCountText.text = "x " + itemCount.ToString(); // ������ x 0 ���� ǥ����
    }
}
