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

    // 召喚制限エリアの範囲（例: プレイヤー側）
    public Vector2 playerSpawnMin = new Vector2(-5, -3);
    public Vector2 playerSpawnMax = new Vector2(0, 3);

    // 召喚制限エリアの範囲（例: CPU側）
    public Vector2 cpuSpawnMin = new Vector2(0, -3);
    public Vector2 cpuSpawnMax = new Vector2(5, 3);

    // 拠点間の最小距離
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

    // プレイヤーの中間拠点を召喚するメソッド
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
            Debug.Log("適切な召喚位置が見つかりませんでした。");
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
            Debug.Log("適切な召喚位置が見つかりませんでした。");
        }
    }

    // 有効な召喚位置をランダムに取得するメソッド
    private Vector2 GetValidSpawnPosition(Vector2 minRange, Vector2 maxRange)
    {
        for (int i = 0; i < 10; i++) // 10回までランダムな位置を試行
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(minRange.x, maxRange.x),
                Random.Range(minRange.y, maxRange.y)
            );

            // 他の拠点との重なりをチェック
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
                return randomPosition; // 有効な位置が見つかった場合、その位置を返す
            }
        }

        return Vector2.zero; // 有効な位置が見つからない場合
    }
}
