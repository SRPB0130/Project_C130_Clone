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

    private bool isDashing = false; // ��� �� ����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        // ��Ŭ�� �̵� (���� ��/��� �� �ƴ� ����)
        if (Input.GetMouseButtonDown(1) && !attack.IsAttacking && !isDashing)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                agent.SetDestination(hit.point);
        }

        // ��Ŭ�� ����
        if (Input.GetMouseButtonUp(0) && !attack.IsAttacking && !isDashing)
        {
            RotateToMouse();

            // �̵� �ߴ�
            agent.isStopped = true;
            agent.ResetPath();
            agent.updateRotation = false;

            // ���� ���� �� �ٽ� �̵� �簳
            attack.TryAttack(() =>
            {
                agent.isStopped = false;
                agent.updateRotation = true;
            });
        }

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        Vector2 moveDir = new Vector2(localVelocity.x, localVelocity.z);

        // Clamp�ؼ� �ʹ� ū �� ����
        moveDir = Vector2.ClampMagnitude(moveDir, 1f);

        // Animator �Ķ���� ������Ʈ
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

    // ��� ����/���� �� �ܺο��� ȣ��
    public void SetDashing(bool value)
    {
        isDashing = value;

        if (isDashing)
        {
            agent.isStopped = true;
            agent.ResetPath();   // ������ ���� (�ٽ� ���ư��� �ʵ���)
        }
        else
        {
            agent.isStopped = true; // ��� ������ ���� ���� ����
            agent.ResetPath();      // Ȥ�� �� �ܿ� �̵� ����
        }
    }
}
