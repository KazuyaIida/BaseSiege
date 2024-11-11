using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // �C�e�v���n�u
    public float attackCooldown = 3f; // �U���Ԋu
    public float attackRange = 5f; // �U���͈�
    private float lastAttackTime = 0f;

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttemptAttack();
            lastAttackTime = Time.time;
        }
    }

    void AttemptAttack()
    {
        // �U���͈͓��̓G��T��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (IsEnemy(hitCollider))
            {
                FireProjectile(hitCollider.transform.position);
                break; // �ŏ��̓G�������U��
            }
        }
    }

    void FireProjectile(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.SetTarget(targetPosition);
    }

    // �G���ǂ����𔻒f
    private bool IsEnemy(Collider2D collider)
    {
        return (gameObject.CompareTag("PlayerIntermediateBase") && collider.CompareTag("EnemySoldier")) ||
               (gameObject.CompareTag("EnemyIntermediateBase") && collider.CompareTag("PlayerSoldier"));
    }

    // �U���͈͂����o���i�I�v�V�����j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
