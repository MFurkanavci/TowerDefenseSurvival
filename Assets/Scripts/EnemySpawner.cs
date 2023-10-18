using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 10.0f;
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
            ObjectPooler.Instance.SpawnFromPool(enemyPrefab, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
        }
    }

    private void KillAllEnemies()
    {
        ObjectPooler.Instance.ReturnAllObjects(enemyPrefab);
    }
}