using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ� 
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D, ��/��
        float vertical = Input.GetAxis("Vertical");     // W/S, ��/��

        Vector3 move = new Vector3(horizontal, 0, vertical);
        float speed = move.magnitude;

        // ���� �̵�
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // �ִϸ��̼� �Ķ���� ����
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.z);
        animator.SetFloat("Speed", speed);
    }
}


