using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("��� ����")]
    public KeyCode dashKey = KeyCode.Space;
    public float dashDistance = 5f;
    public float dashDuration = 0.1f;

    [Header("��� ������ ����")]
    public int maxDashCount = 2;
    public float rechargeDelay = 2f;

    private int currentDashCount;
    private float rechargeTimer = 0f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;
    private Vector3 dashTargetPos;
    private PlayerHP playerHP;
    private MouseMovement movement;
    private Animator animator;
    private PlayerAttack attack;

    [Header("��� UI (ĭ ���)")]
    public Image[] dashIcons;           // UI ������ �迭
    public Sprite fullIcon;             // �� ĭ �̹���
    public Sprite emptyIcon;            // �� ĭ �̹���

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<MouseMovement>();
        currentDashCount = maxDashCount;
        UpdateDashUI();
        attack = GetComponent<PlayerAttack>(); // ���� ���� üũ��
        playerHP = GetComponent<PlayerHP>(); // HP ��ũ��Ʈ ����
    }

    void Update()
    {
        // ��� �Է�
        if (!isDashing && Input.GetKeyDown(dashKey) && currentDashCount > 0)
        {
            StartDash();
        }

        // ��� �̵�
        if (isDashing)
        {
            float step = (dashDistance / dashDuration) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, dashTargetPos, step);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f || Vector3.Distance(transform.position, dashTargetPos) < 0.05f)
            {
                isDashing = false;
                playerHP.SetInvincible(false);
                movement.SetDashing(false);
            }
        }

        // ��� ������ ȸ��
        if (currentDashCount < maxDashCount)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeDelay)
            {
                currentDashCount++;
                rechargeTimer = 0f;
                UpdateDashUI();
            }
        }
    }

    void StartDash()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldMousePos = hit.point;
            dashDirection = (worldMousePos - transform.position).normalized;
            dashTargetPos = transform.position + dashDirection * dashDistance;

            isDashing = true;
            dashTimer = dashDuration;

            currentDashCount--;
            rechargeTimer = 0f;

            // ���� ���� �ƴ� ���� ������ �ִϸ��̼� ����
            if (attack != null && !attack.IsAttacking)
            {
                animator.SetTrigger("Roll");
            }

            UpdateDashUI();
            movement.SetDashing(true);
        }
        Vector3 localDir = transform.InverseTransformDirection(dashDirection); // ���� ���� ��ȯ
        animator.SetFloat("RollX", localDir.x);
        animator.SetFloat("RollY", localDir.z);
        animator.SetTrigger("Roll"); // Blend Tree Ȱ��ȭ
    }

    void UpdateDashUI()
    {
        for (int i = 0; i < dashIcons.Length; i++)
        {
            if (i < currentDashCount)
                dashIcons[i].sprite = fullIcon;
            else
                dashIcons[i].sprite = emptyIcon;
        }
    }
}


