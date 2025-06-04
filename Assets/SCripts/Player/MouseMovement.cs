using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera cam;
    private Animator animator;
    private PlayerAttack attack;

    private bool isDashing = false; // 대시 중 여부

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        // 우클릭 이동 (공격 중/대시 중 아닐 때만)
        if (Input.GetMouseButtonDown(1) && !attack.IsAttacking && !isDashing)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                agent.SetDestination(hit.point);
        }

        // 좌클릭 공격
        if (Input.GetMouseButtonUp(0) && !attack.IsAttacking && !isDashing)
        {
            RotateToMouse();

            // 이동 중단
            agent.isStopped = true;
            agent.ResetPath();
            agent.updateRotation = false;

            // 공격 실행 후 다시 이동 재개
            attack.TryAttack(() =>
            {
                agent.isStopped = false;
                agent.updateRotation = true;
            });
        }

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        Vector2 moveDir = new Vector2(localVelocity.x, localVelocity.z);

        // Clamp해서 너무 큰 값 방지
        moveDir = Vector2.ClampMagnitude(moveDir, 1f);

        // Animator 파라미터 업데이트
        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.SetFloat("Horizontal", moveDir.x);
        animator.SetFloat("Vertical", moveDir.y);
    }

    void RotateToMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    // 대시 시작/종료 시 외부에서 호출
    public void SetDashing(bool value)
    {
        isDashing = value;

        if (isDashing)
        {
            agent.isStopped = true;
            agent.ResetPath();   // 목적지 제거 (다시 돌아가지 않도록)
        }
        else
        {
            agent.isStopped = true; // 대시 끝나도 정지 상태 유지
            agent.ResetPath();      // 혹시 모를 잔여 이동 제거
        }
    }
}
