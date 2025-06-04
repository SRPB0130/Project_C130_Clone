using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float AttackInterval = 2f; // ���� �ֱ� 
    public int damage = 10; // ������
    private float timer; 


    private GameObject player;
    public PlayerHP playerHP;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        if(player != null )
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
    }
    private void Update()
    {
        if (player == null || playerHP == null) return;
        timer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance < 2f && timer  >= AttackInterval)
        {
            timer = 0f;
            AttackPlayer();
        }

    }

    void AttackPlayer()
    {
        playerHP.TakeDamage(damage);
        Debug.Log("�������� �޾ҽ��ϴ�");
    }


}
