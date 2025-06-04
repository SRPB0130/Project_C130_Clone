using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int attackDamage = 30;
    public float attackCooldown = 0.8f;

    [Header("공격 범위")]
    public float attackRadius = 1f;
    [Range(0, 180)] public float attackAngle = 90f;
    public float attackDistance = 1.5f;

    public PlayerAttackRange attackRangeVisualizer;

    private Animator animator;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (attackRangeVisualizer != null)
        {
            attackRangeVisualizer.GenerateFanMesh();
            attackRangeVisualizer.gameObject.SetActive(false);
        }
    }

    public void TryAttack(System.Action onAttackEnd)
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");

        if (attackRangeVisualizer != null)
            attackRangeVisualizer.gameObject.SetActive(true);

        Invoke(nameof(ApplyDamage), 0.4f);
        Invoke(nameof(ApplyDamage), 0.4f);
        StartCoroutine(FinishAttackAfterDelay(0.7f, onAttackEnd));
    }
    IEnumerator FinishAttackAfterDelay(float delay, System.Action onAttackEnd)
    {
        yield return new WaitForSeconds(delay);

        isAttacking = false;

        if (attackRangeVisualizer != null)
            attackRangeVisualizer.gameObject.SetActive(false);

        onAttackEnd?.Invoke();
    }
    void ApplyDamage()
    {
        Vector3 origin = transform.position + transform.forward * attackDistance;
        Collider[] hits = Physics.OverlapSphere(origin, attackRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Enemy enemy = hit.GetComponent<Enemy>() ?? hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Vector3 toTarget = (enemy.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, toTarget);

                if (angleToTarget <= attackAngle)
                    enemy.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 origin = transform.position + transform.forward * attackDistance;
        Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(origin, attackRadius);

        Vector3 forward = transform.forward;
        Quaternion leftRot = Quaternion.Euler(0, -attackAngle, 0);
        Quaternion rightRot = Quaternion.Euler(0, attackAngle, 0);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + (leftRot * forward) * attackRadius);
        Gizmos.DrawLine(origin, origin + (rightRot * forward) * attackRadius);
    }
}
