using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 10;
    public float spawnRadius = 10.0f;
    public GameObject mainTarget;

    public GameObject enemyDeathEffect;

    private List<GameObject> enemyPool = new List<GameObject>();
    private int enemyIndex = 0;

    public float spawnRate = 1.0f;

    void OnEnable()
    {
        DayNightCycle.OnNight += HandleNight;
        DayNightCycle.OnDay += HandleDay;
    }

    void OnDisable()
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
        StopAllCoroutines();
        KillAllEnemies();
    }

    private IEnumerator SpawnEnemiesOnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemy();
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0.0f;
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyNAV>().target = mainTarget.transform;
            enemy.GetComponent<EnemyNAV>().SetMainTarget(mainTarget.transform);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
            enemy.GetComponent<EnemyDamage>().effect = enemyDeathEffect;
            enemy.transform.SetParent(transform);
        }
    }
    private void SpawnEnemy()
    {
        GameObject enemy = enemyPool[enemyIndex];
        enemy.SetActive(true);
        enemy.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
        enemy.GetComponent<EnemyNAV>().target = mainTarget.transform;
        enemy.GetComponent<EnemyNAV>().SetMainTarget(mainTarget.transform);
        enemyIndex = (enemyIndex + 1) % poolSize;
    }

    private void KillAllEnemies()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (enemy.activeSelf)
            {
                enemy.GetComponent<EnemyDamage>().Die();
                enemy.SetActive(false);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyDamage>().Die();
        enemy.SetActive(false);
    }

    
}