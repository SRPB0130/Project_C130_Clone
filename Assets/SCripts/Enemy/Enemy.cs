using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 100; // �ִ� ü��
    public int currentHealth; // ���� ü��

    public GameObject damageTextPrefab;


    private void Start()
    {
        currentHealth = MaxHealth; // ���� ����� ���� ü���� �ִ� ü������ ����
    }

    public void TakeDamage(int damage)
{
    Debug.Log($"{gameObject.name} �� ������ ����: {damage}");

    currentHealth -= damage;

    if (damageTextPrefab != null)
    {
        Vector3 spawnPos = transform.position + Vector3.up * 2f;
        GameObject text = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
        text.GetComponent<DamageText>().Show(damage); 
    }

    if (currentHealth <= 0)
    {
        Die();
    }
}

    void Die()
    {
        Destroy(gameObject);
    }

}
