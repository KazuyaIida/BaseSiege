using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // 砲弾プレハブ
    public float attackCooldown = 3f; // 攻撃間隔
    public float attackRange = 5f; // 攻撃範囲
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
        // 攻撃範囲内の敵を探す
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (IsEnemy(hitCollider))
            {
                FireProjectile(hitCollider.transform.position);
                break; // 最初の敵だけを攻撃
            }
        }
    }

    void FireProjectile(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.SetTarget(targetPosition);
    }

    // 敵かどうかを判断
    private bool IsEnemy(Collider2D collider)
    {
        return (gameObject.CompareTag("PlayerIntermediateBase") && collider.CompareTag("EnemySoldier")) ||
               (gameObject.CompareTag("EnemyIntermediateBase") && collider.CompareTag("PlayerSoldier"));
    }

    // 攻撃範囲を視覚化（オプション）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
