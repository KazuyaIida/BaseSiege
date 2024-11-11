using UnityEngine;

public class Attackable : MonoBehaviour
{
    public int health = 10;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManagerを取得
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null && (gameObject.CompareTag("PlayerBase") || gameObject.CompareTag("EnemyBase")))
            {
                gameManager.OnBaseDestroyed(gameObject.tag); // 拠点破壊を通知
            }

            if (gameObject.CompareTag("PlayerIntermediateBase"))
            {
                // プレイヤー拠点がCPUに制圧された場合
                gameObject.tag = "EnemyIntermediateBase";
            }
            else if (gameObject.CompareTag("EnemyIntermediateBase"))
            {
                // CPU拠点がプレイヤーに制圧された場合
                gameObject.tag = "PlayerIntermediateBase";
            }

            Destroy(gameObject);
        }
    }
}
