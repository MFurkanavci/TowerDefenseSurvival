using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    private const int POOL_SIZE = 200;

    [SerializeField] private GameObject bulletPoolHolder;
    [SerializeField] private GameObject enemyPoolHolder;
    [SerializeField] private GameObject resourcePoolHolder;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private GameObject dieEffectPrefab;

    private void Start()
    {
        CreateObjectPool(bulletPrefab, bulletPoolHolder.transform);
        CreateObjectPool(enemyPrefab, enemyPoolHolder.transform);
        CreateObjectPool(experiencePrefab, enemyPoolHolder.transform);
        CreateObjectPool(dieEffectPrefab, enemyPoolHolder.transform);
        CreateObjectPool(resourcePrefab, resourcePoolHolder.transform);
    }

    private void CreateObjectPool(GameObject prefab, Transform parent)
    {
        ObjectPooler.Instance.CreatePool(prefab, POOL_SIZE, parent);
    }
}