using UnityEngine;

public class Attackable : MonoBehaviour
{
    public int health = 10;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager���擾
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null && (gameObject.CompareTag("PlayerBase") || gameObject.CompareTag("EnemyBase")))
            {
                gameManager.OnBaseDestroyed(gameObject.tag); // ���_�j���ʒm
            }

            if (gameObject.CompareTag("PlayerIntermediateBase"))
            {
                // �v���C���[���_��CPU�ɐ������ꂽ�ꍇ
                gameObject.tag = "EnemyIntermediateBase";
            }
            else if (gameObject.CompareTag("EnemyIntermediateBase"))
            {
                // CPU���_���v���C���[�ɐ������ꂽ�ꍇ
                gameObject.tag = "PlayerIntermediateBase";
            }

            Destroy(gameObject);
        }
    }
}
