using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUse : MonoBehaviour
{
    public int healingAmount = 20;           // 회복량 (퍼블릭으로 조절 가능)
    public int currentItemCount = 0;         // 현재 아이템 수량
    public Text itemCountText;               // UI 텍스트 (인벤토리 표시)
    public PlayerHP playerHP;        // 체력 회복을 위한 컴포넌트 참조

    void Update()
    {
        // E 키로 아이템 사용
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

        // 체력 회복
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
