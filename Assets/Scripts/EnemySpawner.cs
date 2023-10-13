using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject mainTarget;
    public float spawnRate = 1.0f;
    public float spawnRadius = 10.0f;
    public float spawnTimer = 0.0f;
    public int maxEnemies = 10;
    public int enemyCount = 0;

    public List<GameObject> enemies = new List<GameObject>();

    public DayNightCycle dayNightCycle;

    private void Update()
    {
        if (dayNightCycle.IsNight())
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnRate && enemyCount < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = 0.0f;
            }
        }
        else if (dayNightCycle.IsDay())
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null)
                {
                    continue;
                }
                enemy.GetComponent<EnemyDamage>().Die();
            }
            enemies.Clear();
            enemyCount = 0;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = 0.0f;
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyNAV>().target = mainTarget.transform;
        enemies.Add(enemy);
        enemyCount++;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        
        enemy.GetComponent<EnemyDamage>().Die();
        enemies.Remove(enemy);
        enemyCount--;
    }

}
