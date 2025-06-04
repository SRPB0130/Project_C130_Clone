using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public float detectRange = 5f;  // �߰� ����
    public float Stopdistance = 0.5f; // �߰� ���� �ּ� �Ÿ�
    public Transform target; // �߰� ���
    public float ChaseSpeed = 2.0f;

    private NavMeshAgent agent;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = ChaseSpeed;
        if(agent == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player"); // ã�� ����� �±� 
            if(playerObj != null )
            {
                target = playerObj.transform;
            }
        }
        agent.stoppingDistance = Stopdistance;
    }

    private void Update()
    {
        if (target == null) return;
         float distanse =Vector3.Distance(transform.position, target.position);

        if (distanse < detectRange)
        {
            agent.isStopped = false;

            if (distanse > Stopdistance)
            {
                agent.SetDestination(target.position); // ��ǥ ��ġ���� �̵�
            }
            else
            {
                agent.ResetPath(); // �ٽ� Ž��
            }
        }
        else
        {
            agent.isStopped = true;
        }

    }
    private void OnDrawGizmosSelected() // scene â�� ���� ǥ�� 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
