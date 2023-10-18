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

    void Start()
    {
    }

    void ReSpawnResource()
    {
        if (currentResourcesForDay < resourcesPerDay/2)
        {
            SpawnResource();
            currentResourcesForDay++;
        }
    }

    private void SpawnResource()
    {
        GameObject resource = ObjectPooler.Instance.GetObject(resourcePrefab);
        resource.transform.position = GetRandomPosition();
        resource.GetComponent<Resource>().resourceData = GetRandomResource();
        resource.GetComponent<Resource>().SOData(resource.GetComponent<Resource>().resourceData);
        resource.GetComponent<Resource>().SetScale();
        resource.GetComponent<MeshRenderer>().material = resource.GetComponent<Resource>().resourceData.resourceMaterial;
    }

    public void ReSpawnResource(GameObject resource)
    {
        ObjectPooler.Instance.GetObject(resourcePrefab);
        resource.transform.position = GetRandomPosition();
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

    void Update()
    {
        ReSpawnResource();
    }
}