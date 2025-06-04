using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("대시 설정")]
    public KeyCode dashKey = KeyCode.Space;
    public float dashDistance = 5f;
    public float dashDuration = 0.1f;

    [Header("대시 게이지 설정")]
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

    [Header("대시 UI (칸 방식)")]
    public Image[] dashIcons;           // UI 아이콘 배열
    public Sprite fullIcon;             // 찬 칸 이미지
    public Sprite emptyIcon;            // 빈 칸 이미지

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<MouseMovement>();
        currentDashCount = maxDashCount;
        UpdateDashUI();
        attack = GetComponent<PlayerAttack>(); // 공격 상태 체크용
        playerHP = GetComponent<PlayerHP>(); // HP 스크립트 참조
    }

    void Update()
    {
        // 대시 입력
        if (!isDashing && Input.GetKeyDown(dashKey) && currentDashCount > 0)
        {
            StartDash();
        }

        // 대시 이동
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

        // 대시 게이지 회복
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

            // 공격 중이 아닐 때만 구르기 애니메이션 실행
            if (attack != null && !attack.IsAttacking)
            {
                animator.SetTrigger("Roll");
            }

            UpdateDashUI();
            movement.SetDashing(true);
        }
        Vector3 localDir = transform.InverseTransformDirection(dashDirection); // 로컬 기준 변환
        animator.SetFloat("RollX", localDir.x);
        animator.SetFloat("RollY", localDir.z);
        animator.SetTrigger("Roll"); // Blend Tree 활성화
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


