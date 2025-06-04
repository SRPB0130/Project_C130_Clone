using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionItem : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F;          // 상호작용 키
    public float interactionTime = 2f;                  // 상호작용 시간
    public string interactionAnimation = "Interact";    // 애니메이션 트리거
    public Slider progressSlider;                       // 진행바
    public GameObject uiCanvas;                         // UI 그룹

    private bool isPlayerInRange = false;
    private bool isInteracting = false;
    private float timer = 0f;
    private Animator playerAnimator;
    private ItemUse itemUser;                          // 아이템 수량 관리 스크립트

    private void Update()
    {
        if (isPlayerInRange && !isInteracting)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                StartCoroutine(DoInteraction());
            }
        }
    }

    IEnumerator DoInteraction()
    {
        isInteracting = true;
        timer = 0f;
        uiCanvas.SetActive(true);

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(interactionAnimation);
        }

        while (timer < interactionTime)
        {
            timer += Time.deltaTime;
            progressSlider.value = timer / interactionTime;
            yield return null;
        }

        uiCanvas.SetActive(false);
        isInteracting = false;

        OnInteractionComplete();
    }

    void OnInteractionComplete()
    {
        Debug.Log("아이템 상호작용 완료!");

        if (itemUser != null)
        {
            itemUser.AddItem(1); // 아이템 1개 추가
        }
        else
        {
            Debug.LogWarning("ItemUse를 찾을 수 없습니다!");
        }

        Destroy(gameObject); // 아이템 제거
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            playerAnimator = other.GetComponent<Animator>();
            itemUser = other.GetComponent<ItemUse>(); // 플레이어에서 직접 찾기

            if (itemUser == null)
            {
                Debug.LogWarning("Player에게 ItemUse 스크립트가 없습니다!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uiCanvas.SetActive(false);
        }
    }
}
