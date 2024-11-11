using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerSoldierPrefab;
    public GameObject cpuSoldierPrefab;
    public GameObject playerIntermediateBasePrefab;
    public GameObject cpuIntermediateBasePrefab;
    public Transform playerSpawnPoint;
    public Transform cpuSpawnPoint;

    // ���������G���A�͈̔́i��: �v���C���[���j
    public Vector2 playerSpawnMin = new Vector2(-5, -3);
    public Vector2 playerSpawnMax = new Vector2(0, 3);

    // ���������G���A�͈̔́i��: CPU���j
    public Vector2 cpuSpawnMin = new Vector2(0, -3);
    public Vector2 cpuSpawnMax = new Vector2(5, 3);

    // ���_�Ԃ̍ŏ�����
    public float minDistanceBetweenBases = 2.0f;

    private List<GameObject> spawnedBases = new List<GameObject>();

    public void SpawnPlayerSoldier()
    {
        GameObject soldier = Instantiate(playerSoldierPrefab, playerSpawnPoint.position, Quaternion.identity);
        soldier.GetComponent<SoldierMovement>().SetDestination(new Vector2(3, 0));
    }

    public void SpawnCPUSoldier()
    {
        GameObject soldier = Instantiate(cpuSoldierPrefab, cpuSpawnPoint.position, Quaternion.identity);
        soldier.GetComponent<SoldierMovement>().SetDestination(new Vector2(-3, 0));
    }

    // �v���C���[�̒��ԋ��_���������郁�\�b�h
    public void SpawnPlayerIntermediateBase()
    {
        Vector2 spawnPosition = GetValidSpawnPosition(playerSpawnMin, playerSpawnMax);
        if (spawnPosition != Vector2.zero)
        {
            GameObject baseObj = Instantiate(playerIntermediateBasePrefab, spawnPosition, Quaternion.identity);
            spawnedBases.Add(baseObj);
            baseObj.GetComponent<SoldierMovement>().SetDestination(new Vector2(3, 0));
        }
        else
        {
            Debug.Log("�K�؂ȏ����ʒu��������܂���ł����B");
        }
    }

    public void SpawnCPUIntermediateBase()
    {
        Vector2 spawnPosition = GetValidSpawnPosition(cpuSpawnMin, cpuSpawnMax);
        if (spawnPosition != Vector2.zero)
        {
            GameObject baseObj = Instantiate(cpuIntermediateBasePrefab, spawnPosition, Quaternion.identity);
            spawnedBases.Add(baseObj);
            baseObj.GetComponent<SoldierMovement>().SetDestination(new Vector2(-3, 0));
        }
        else
        {
            Debug.Log("�K�؂ȏ����ʒu��������܂���ł����B");
        }
    }

    // �L���ȏ����ʒu�������_���Ɏ擾���郁�\�b�h
    private Vector2 GetValidSpawnPosition(Vector2 minRange, Vector2 maxRange)
    {
        for (int i = 0; i < 10; i++) // 10��܂Ń����_���Ȉʒu�����s
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(minRange.x, maxRange.x),
                Random.Range(minRange.y, maxRange.y)
            );

            // ���̋��_�Ƃ̏d�Ȃ���`�F�b�N
            bool isPositionValid = true;
            foreach (var baseObj in spawnedBases)
            {
                if (baseObj != null && Vector2.Distance(randomPosition, baseObj.transform.position) < minDistanceBetweenBases)
                {
                    isPositionValid = false;
                    break;
                }
            }

            if (isPositionValid)
            {
                return randomPosition; // �L���Ȉʒu�����������ꍇ�A���̈ʒu��Ԃ�
            }
        }

        return Vector2.zero; // �L���Ȉʒu��������Ȃ��ꍇ
    }
}
