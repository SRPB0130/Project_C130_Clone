using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamageText : MonoBehaviour
{
    public float floatSpeed = 2f; // ������ �۾� �ö󰡴� �ӵ�
    public float duration = 1f; // ������ �۾� �����ִ� �ð�

    private TextMeshPro text; // ������ ������ �۾�

     void Awake()
    {
        text = GetComponent<TextMeshPro>(); 
    }

    public void Show(int damage)
    {
        text.text = damage.ToString(); // �������� ���ڷ� ǥ�� 
        Destroy(gameObject, duration); // �ð� ������ ������Ʈ �ı�
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime; // ������Ʈ�� �� �������� �ö� 
        transform.LookAt(Camera.main.transform); // ī�޶� �ٶ󺸵��� 

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // ī�޶�� ������ ������ �ٶ󺸵��� 
    }
}
