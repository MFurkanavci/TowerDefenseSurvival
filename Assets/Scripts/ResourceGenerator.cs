using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ResourceGenerator : MonoBehaviour
{
    public GameObject resourcePrefab;
    public float resourceSpawnRadius = 10.0f;
    public ResourceData[] resources;
    public static int currentResourcesForDay = 0;

    public int resourcesPerDay = 100;

    public float delayForOutOfCullingArea = 3f;

    private void OnEnable()
    {
        DayNightCycle.OnDay += HandleDay;
    }

    private void OnDisable()
    {
        DayNightCycle.OnDay -= HandleDay;
    }
    private void HandleDay()
    {
        while(currentResourcesForDay < resourcesPerDay)
        {
            Vector3 position = GetRandomPosition();
            bool isInCullingArea = SpawnRadius.Instance.IsInCullingArea(Camera.main, position, resourceSpawnRadius);
            if (!isInCullingArea)
            {
                currentResourcesForDay++;
                StartCoroutine(SpawnResourcesWithDelay(position));
            }
            else continue;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnResources());
    }

    IEnumerator SpawnResources()
    {
       while(currentResourcesForDay < resourcesPerDay)
        {
            Vector3 position = GetRandomPosition();
            bool isInCullingArea = SpawnRadius.Instance.IsInCullingArea(Camera.main, position, resourceSpawnRadius);
            if (isInCullingArea)
            {
                currentResourcesForDay++;
                SpawnResource(position);
            }
            else
            {
                currentResourcesForDay++;
                StartCoroutine(SpawnResourcesWithDelay(position));
            }
        }
        yield return null;
    }

    IEnumerator SpawnResourcesWithDelay(Vector3 position)
    {
        yield return new WaitForSecondsRealtime(delayForOutOfCullingArea);
        ReSpawnResource(position);
    }

    private void SpawnResource(Vector3 position)
    {
        GameObject resource = ObjectPooler.Instance.GetObject(resourcePrefab);
        resource.transform.position = position;
        resource.GetComponent<Resource>().resourceData = GetRandomResource();
        resource.GetComponent<Resource>().SOData(resource.GetComponent<Resource>().resourceData);
        resource.GetComponent<Resource>().SetScale();
        resource.GetComponent<MeshRenderer>().material = resource.GetComponent<Resource>().resourceData.resourceMaterial;
    }

    public void ReSpawnResource(Vector3 position)
    {
        GameObject resource = ObjectPooler.Instance.SpawnFromPool(resourcePrefab, position, Quaternion.identity);
        resource.transform.position = position;
        resource.GetComponent<Resource>().resourceData = GetRandomResource();
        resource.GetComponent<Resource>().SOData(resource.GetComponent<Resource>().resourceData);
        resource.GetComponent<Resource>().SetScale();
        resource.GetComponent<MeshRenderer>().material = resource.GetComponent<Resource>().resourceData.resourceMaterial;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * resourceSpawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, resourceSpawnRadius, 1))
        {
            return hit.position;
        }
        return Vector3.zero; 
    }

    private ResourceData GetRandomResource()
    {
        int totalQuality = 0;
        foreach (ResourceData resource in resources)
        {
            totalQuality += resource.resourceQuality;
        }
        if (totalQuality <= 0) return null; 

        int randomQuality = UnityEngine.Random.Range(0, totalQuality);
        int currentQuality = 0;
        foreach (ResourceData resource in resources)
        {
            currentQuality += resource.resourceQuality;
            if (currentQuality >= randomQuality)
            {
                return resource;
            }
        }
        return null; 
    }
}