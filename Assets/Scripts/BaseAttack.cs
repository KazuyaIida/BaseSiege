using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // –C’eƒvƒŒƒnƒu
    public float attackCooldown = 3f; // UŒ‚ŠÔŠu
    public float attackRange = 5f; // UŒ‚”ÍˆÍ
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
        // UŒ‚”ÍˆÍ“à‚Ì“G‚ğ’T‚·
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (IsEnemy(hitCollider))
            {
                FireProjectile(hitCollider.transform.position);
                break; // Å‰‚Ì“G‚¾‚¯‚ğUŒ‚
            }
        }
    }

    void FireProjectile(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.SetTarget(targetPosition);
    }

    // “G‚©‚Ç‚¤‚©‚ğ”»’f
    private bool IsEnemy(Collider2D collider)
    {
        return (gameObject.CompareTag("PlayerIntermediateBase") && collider.CompareTag("EnemySoldier")) ||
               (gameObject.CompareTag("EnemyIntermediateBase") && collider.CompareTag("PlayerSoldier"));
    }

    // UŒ‚”ÍˆÍ‚ğ‹Šo‰»iƒIƒvƒVƒ‡ƒ“j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
