using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform target;                // ĳ����
    public Vector3 offset = new Vector3(0, 2, -4);
    public float mouseSensitivity = 3f;
    public float minPitch = -30f;           // �Ʒ��� �ִ� ����
    public float maxPitch = 60f;            // ���� �ִ� ����

    private float yaw = 0f;                 // �¿� ȸ��
    private float pitch = 10f;              // ���� ȸ�� (�ʱ� ����)

    void Update()
    {
        // ���콺 �Է�
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �¿� ȸ�� (ĳ���� Y ����)
        yaw += mouseX;

        // ���� ȸ�� (Pitch ���� ����)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // ȸ�� ����
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f); // ĳ���� �Ӹ� �� ��¦ ����
    }
}
