using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ResourceGenerator : MonoBehaviour
{
    public GameObject resourcePrefab;
    public int poolSize = 10;
    public float resourceSpawnRadius = 10.0f;
    public ResourceData[] resources;

    private List<GameObject> resourcePool = new List<GameObject>();
    private int resourceIndex = 0;
    private int currentResourcesForDay = 0;

    public void RemoveAResource()
    {
        currentResourcesForDay--;

        if (currentResourcesForDay <= 0)
        {
            SpawnAllResources();
        }
    }

    public static ResourceGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject resource = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
            resource.transform.SetParent(transform);
            resource.SetActive(false);
            resourcePool.Add(resource);
        }

        SpawnAllResources();
    }

    private void SpawnResource()
    {
        GameObject resource = resourcePool[resourceIndex];
        resource.SetActive(true);
        resource.transform.position = GetRandomPosition();
        resource.GetComponent<Resource>().resourceData = GetRandomResource();
        resource.GetComponent<MeshRenderer>().material = resource.GetComponent<Resource>().resourceData.resourceMaterial;
        resourceIndex = (resourceIndex + 1) % poolSize;
        currentResourcesForDay++;
    }

    public void ReSpawnResource(GameObject resource)
    {
        resource.transform.position = GetRandomPosition();
        resource.GetComponent<Resource>().resourceData = GetRandomResource();
        resource.GetComponent<Resource>().SOData(resource.GetComponent<Resource>().resourceData);
        resource.GetComponent<Resource>().SetScale();
        resource.GetComponent<MeshRenderer>().material = resource.GetComponent<Resource>().resourceData.resourceMaterial;
        resource.SetActive(true);
        currentResourcesForDay++;
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

    public void ResetResourcesForDay()
    {
        currentResourcesForDay = 0;
    }

    public void SpawnAllResources()
    {
        foreach (GameObject resource in resourcePool)
        {
            if (!resource.activeSelf)
            {
                SpawnResource();
            }
        }
    }
}