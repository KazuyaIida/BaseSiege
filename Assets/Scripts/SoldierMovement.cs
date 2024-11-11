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

    private List<Transform> pathPoints = new List<Transform>(); // �o�R���鋒�_���X�g
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
        // �o�R���_���X�g��ݒ�
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

        // ���݂̖ڕW���_�ɓ��B���������m�F
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextTarget();
        }
    }

    void SetPathPoints()
    {
        // �����̐w�c�ɉ��������ԋ��_�����ɒǉ�
        string intermediateBaseTag = gameObject.CompareTag("PlayerSoldier") ? "EnemyIntermediateBase" : "PlayerIntermediateBase";
        GameObject[] intermediateBases = GameObject.FindGameObjectsWithTag(intermediateBaseTag);

        foreach (var baseObj in intermediateBases)
        {
            pathPoints.Add(baseObj.transform);
        }

        // �ŏI�I�ȓG���_�����X�g�ɒǉ�
        GameObject finalBase = GameObject.FindGameObjectWithTag(enemyTag);
        if (finalBase != null)
        {
            pathPoints.Add(finalBase.transform);
        }
    }

    void MoveToNextTarget()
    {
        // �j�󂳂ꂽ�^�[�Q�b�g���X�L�b�v
        while (currentTargetIndex < pathPoints.Count)
        {
            if (pathPoints[currentTargetIndex] == null)
            {
                currentTargetIndex++; // �^�[�Q�b�g���j�󂳂�Ă���ꍇ�͎��ɐi��
            }
            else if (!ShouldTargetBase(pathPoints[currentTargetIndex].gameObject))
            {
                currentTargetIndex++; // �^�[�Q�b�g�������ł���ꍇ�����ɐi��
            }
            else
            {
                break; // �L���ȃ^�[�Q�b�g�����������烋�[�v�𔲂���
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

    // ���_���G�̂��̂ł���^�[�Q�b�g�Ώۂɂ��ׂ����𔻒�
    private bool ShouldTargetBase(GameObject baseObj)
    {
        if (baseObj == null) return false; // �I�u�W�F�N�g�����ɔj�󂳂�Ă���ꍇ

        if (gameObject.CompareTag("PlayerSoldier") && baseObj.CompareTag("EnemyIntermediateBase"))
            return true;
        if (gameObject.CompareTag("EnemySoldier") && baseObj.CompareTag("PlayerIntermediateBase"))
            return true;
        return baseObj.CompareTag(enemyTag); // �ŏI���_
    }
}