using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatternAttack : MonoBehaviour
{
    public Transform target;                   // 추격 및 공격 대상
    public float detectRange = 10f;            // 플레이어 탐지 거리
    public float attackRange = 3f;             // 공격 범위 반지름
    public float attackAngle = 90f;            // 공격 각도 (부채꼴 범위)
    public float attackDelay = 1.5f;           // 공격 예고 시간
    public float attackCooldown = 3f;          // 공격 쿨타임
    public GameObject attackAreaPrefab;        // 공격 시각화 프리팹
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

        // 탐지 범위 안에 있을 경우
        if (distance <= detectRange)
        {
            // 공격 범위 안에 있고 쿨타임이 지났다면 공격 시작
            if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                StartCoroutine(AttackPattern());
            }
            else
            {
                // 평상시에는 추격 및 회전 허용
                agent.isStopped = false;
                agent.updateRotation = true;
                agent.SetDestination(target.position);
            }
        }
        else
        {
            // 탐지 범위 밖이면 멈춤
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
        areaScript?.SetColor(new Color(1f, 0.5f, 0f, 0.3f)); // 주황색

        // 딜레이 중 후반부에 색 변경
        float waitBeforeColorChange = Mathf.Clamp(attackDelay - colorChangeTime, 0f, attackDelay);
        yield return new WaitForSeconds(waitBeforeColorChange);

        areaScript?.SetColor(new Color(1f, 0f, 0f, 0.4f)); // 빨간색

        yield return new WaitForSeconds(colorChangeTime);

        Destroy(area);

        // 데미지 판정 생략 없이 유지
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
        // 강제로 한 번 플레이어 방향으로 회전
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
