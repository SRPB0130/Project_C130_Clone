using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatternAttack : MonoBehaviour
{
    public Transform target;                   // �߰� �� ���� ���
    public float detectRange = 10f;            // �÷��̾� Ž�� �Ÿ�
    public float attackRange = 3f;             // ���� ���� ������
    public float attackAngle = 90f;            // ���� ���� (��ä�� ����)
    public float attackDelay = 1.5f;           // ���� ���� �ð�
    public float attackCooldown = 3f;          // ���� ��Ÿ��
    public GameObject attackAreaPrefab;        // ���� �ð�ȭ ������
    public float colorChangeTime = 0.3f;


    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        if (target == null || isAttacking) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // Ž�� ���� �ȿ� ���� ���
        if (distance <= detectRange)
        {
            // ���� ���� �ȿ� �ְ� ��Ÿ���� �����ٸ� ���� ����
            if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                StartCoroutine(AttackPattern());
            }
            else
            {
                // ���ÿ��� �߰� �� ȸ�� ���
                agent.isStopped = false;
                agent.updateRotation = true;
                agent.SetDestination(target.position);
            }
        }
        else
        {
            // Ž�� ���� ���̸� ����
            agent.isStopped = true;
        }
    }

    IEnumerator AttackPattern()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        agent.isStopped = true;
        agent.updateRotation = false;

        GameObject area = Instantiate(attackAreaPrefab, transform.position, transform.rotation);
        area.transform.SetParent(transform);

        FanAttackArea areaScript = area.GetComponent<FanAttackArea>();
        areaScript?.SetColor(new Color(1f, 0.5f, 0f, 0.3f)); // ��Ȳ��

        // ������ �� �Ĺݺο� �� ����
        float waitBeforeColorChange = Mathf.Clamp(attackDelay - colorChangeTime, 0f, attackDelay);
        yield return new WaitForSeconds(waitBeforeColorChange);

        areaScript?.SetColor(new Color(1f, 0f, 0f, 0.4f)); // ������

        yield return new WaitForSeconds(colorChangeTime);

        Destroy(area);

        // ������ ���� ���� ���� ����
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 toTarget = (hit.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, toTarget);
                float dist = Vector3.Distance(transform.position, hit.transform.position);

                if (angle < attackAngle / 2f && dist <= attackRange)
                {
                    hit.GetComponent<PlayerHP>()?.TakeDamage(20);
                }
            }
        }

        isAttacking = false;
        agent.isStopped = false;
        agent.updateRotation = true;
        // ������ �� �� �÷��̾� �������� ȸ��
        if (target != null)
        {
            Vector3 toTarget = (target.position - transform.position).normalized;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(toTarget);
                transform.rotation = lookRotation;
            }
        }
    }
}
