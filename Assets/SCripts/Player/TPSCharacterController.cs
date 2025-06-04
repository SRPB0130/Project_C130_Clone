using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 3f;

    [Header("���� �� �߷�")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;          // y�� �ӵ� (�߷�, ����)
    private bool isGrounded;           // �ٴڿ� �ִ��� ����

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. ���콺�� ĳ���� ȸ�� (�¿츸)
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * mouseSensitivity, 0);

        // 2. �̵� �Է� (ĳ���� ���� ���� ����)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.right * h + transform.forward * v;

        // 3. ���� üũ
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ���鿡 ���̱�
        }

        // 4. ���� �Է�
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // 6. ���� �̵� ����
        Vector3 finalMove = moveDir * moveSpeed + new Vector3(0, velocity.y, 0);
        controller.Move(finalMove * Time.deltaTime);

        // 7. �ִϸ��̼� ����
        animator.SetFloat("Speed", moveDir.magnitude);
        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Vertical", v);
        animator.SetBool("IsGrounded", isGrounded);
    }
}