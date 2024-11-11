using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // –C’e‚Ì‘¬“x
    public int damage = 2; // –C’e‚Ìƒ_ƒ[ƒW

    private Vector2 targetPosition;

    public void SetTarget(Vector2 target)
    {
        targetPosition = target;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Attackable target = collision.GetComponent<Attackable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject); // –C’e‚ğÁ‚·
        }
    }
}
