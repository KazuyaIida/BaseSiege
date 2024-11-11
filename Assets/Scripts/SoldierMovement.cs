using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SoldierMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameManager gameManager;
    public string enemyTag;
    public float attackRange = 1.0f;
    public int attackDamage = 1;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;

    private List<Transform> pathPoints = new List<Transform>(); // 経由する拠点リスト
    private int currentTargetIndex = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        // 経由拠点リストを設定
        SetPathPoints();
        MoveToNextTarget();
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            Attackable target = hitCollider.GetComponent<Attackable>();
            if (target != null && IsEnemy(hitCollider))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    target.TakeDamage(attackDamage);
                    lastAttackTime = Time.time;
                }
            }
        }

        // 現在の目標拠点に到達したかを確認
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextTarget();
        }
    }

    void SetPathPoints()
    {
        // 自分の陣営に応じた中間拠点を順に追加
        string intermediateBaseTag = gameObject.CompareTag("PlayerSoldier") ? "EnemyIntermediateBase" : "PlayerIntermediateBase";
        GameObject[] intermediateBases = GameObject.FindGameObjectsWithTag(intermediateBaseTag);

        foreach (var baseObj in intermediateBases)
        {
            pathPoints.Add(baseObj.transform);
        }

        // 最終的な敵拠点をリストに追加
        GameObject finalBase = GameObject.FindGameObjectWithTag(enemyTag);
        if (finalBase != null)
        {
            pathPoints.Add(finalBase.transform);
        }
    }

    void MoveToNextTarget()
    {
        // 破壊されたターゲットをスキップ
        while (currentTargetIndex < pathPoints.Count)
        {
            if (pathPoints[currentTargetIndex] == null)
            {
                currentTargetIndex++; // ターゲットが破壊されている場合は次に進む
            }
            else if (!ShouldTargetBase(pathPoints[currentTargetIndex].gameObject))
            {
                currentTargetIndex++; // ターゲットが無効である場合も次に進む
            }
            else
            {
                break; // 有効なターゲットが見つかったらループを抜ける
            }
        }

        if (currentTargetIndex < pathPoints.Count)
        {
            SetDestination(pathPoints[currentTargetIndex].position);
            currentTargetIndex++;
        }
    }

    public void SetDestination(Vector2 destination)
    {
        agent.SetDestination(destination);
    }

    private bool IsEnemy(Collider2D collider)
    {
        return (collider.CompareTag(enemyTag) ||
                (gameObject.CompareTag("PlayerSoldier") && collider.CompareTag("EnemyIntermediateBase")) ||
                (gameObject.CompareTag("EnemySoldier") && collider.CompareTag("PlayerIntermediateBase")));
    }

    // 拠点が敵のものでありターゲット対象にすべきかを判定
    private bool ShouldTargetBase(GameObject baseObj)
    {
        if (baseObj == null) return false; // オブジェクトが既に破壊されている場合

        if (gameObject.CompareTag("PlayerSoldier") && baseObj.CompareTag("EnemyIntermediateBase"))
            return true;
        if (gameObject.CompareTag("EnemySoldier") && baseObj.CompareTag("PlayerIntermediateBase"))
            return true;
        return baseObj.CompareTag(enemyTag); // 最終拠点
    }
}