using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 10.0f;
    public float spawnRate = 1.0f;

    private ObjectPooler objectPooler;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private Transform player;

    private void Awake()
    {
        objectPooler = ObjectPooler.Instance;
        player = FindObjectOfType<Player>().transform;
    }

    private void OnEnable()
    {
        DayNightCycle.OnNight += HandleNight;
        DayNightCycle.OnDay += HandleDay;
    }

    private void OnDisable()
    {
        DayNightCycle.OnNight -= HandleNight;
        DayNightCycle.OnDay -= HandleDay;
    }

    private void HandleNight()
    {
        StartCoroutine(SpawnEnemiesOnTimer());
    }

    private void HandleDay()
    {
        KillAllEnemies();
    }

    private IEnumerator SpawnEnemiesOnTimer()
    {
        while (true)
        {
            Vector3 spawnPosition = GetRandomPositionOutsideCullingArea();
            if (spawnPosition != transform.position)
            {
                bool isInCullingArea = SpawnRadius.Instance.IsInCullingArea(Camera.main, spawnPosition, spawnRadius);
                if (!isInCullingArea)
                {
                    yield return new WaitForSecondsRealtime(spawnRate);
                    GameObject newEnemy = objectPooler.SpawnFromPool(enemyPrefab, spawnPosition, Quaternion.identity);
                    spawnedEnemies.Add(newEnemy);
                    SetEnemyTarget(newEnemy);
                }
            }
        }
    }

    private void SetEnemyTarget(GameObject enemy)
    {
        EnemyAggro enemyAggro = enemy.GetComponentInChildren<EnemyAggro>();
        enemyAggro.SetPlayer(player);
    }

    private Vector3 GetRandomPositionOutsideCullingArea()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position;
        }
    }

    private void KillAllEnemies()
    {
        StopAllCoroutines();
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy.activeInHierarchy)
            {
                objectPooler.ReturnObject(enemy, enemy);
            }
        }
        spawnedEnemies.Clear();
    }
}