using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionItem : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F;          // ��ȣ�ۿ� Ű
    public float interactionTime = 2f;                  // ��ȣ�ۿ� �ð�
    public string interactionAnimation = "Interact";    // �ִϸ��̼� Ʈ����
    public Slider progressSlider;                       // �����
    public GameObject uiCanvas;                         // UI �׷�

    private bool isPlayerInRange = false;
    private bool isInteracting = false;
    private float timer = 0f;
    private Animator playerAnimator;
    private ItemUse itemUser;                          // ������ ���� ���� ��ũ��Ʈ

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
        Debug.Log("������ ��ȣ�ۿ� �Ϸ�!");

        if (itemUser != null)
        {
            itemUser.AddItem(1); // ������ 1�� �߰�
        }
        else
        {
            Debug.LogWarning("ItemUse�� ã�� �� �����ϴ�!");
        }

        Destroy(gameObject); // ������ ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            playerAnimator = other.GetComponent<Animator>();
            itemUser = other.GetComponent<ItemUse>(); // �÷��̾�� ���� ã��

            if (itemUser == null)
            {
                Debug.LogWarning("Player���� ItemUse ��ũ��Ʈ�� �����ϴ�!");
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
