using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private List<ResourceData> resources;
    [SerializeField] private float resourceSpawnRadius;
    [SerializeField] private int maxResourcesForDay = 100;

    public static int currentResourcesForDay;
    private WaitForSeconds spawnDelay = new WaitForSeconds(0.1f);

    private void Start()
    {
        GenerateResources();
    }

    public int NeededResourcesPerDay()
    {
        return maxResourcesForDay - currentResourcesForDay;
    }

    public void GenerateResources()
    {
        StartCoroutine(GenerateResourcesCoroutine());
    }

    private IEnumerator GenerateResourcesCoroutine()
    {
        int resourcesToGenerate = NeededResourcesPerDay();
        for (int i = 0; i < resourcesToGenerate; i++)
        {
            GenerateResource();
            yield return spawnDelay;
        }
    }

    private void GenerateResource()
    {
        ResourceData resource = GetRandomResource();
        if (resource == null) return; // Check if there are no valid resources

        Vector3 randomPosition = GetRandomPosition();
        GameObject resourceObject = Instantiate(resource.resourcePrefab, randomPosition, Quaternion.identity);
        resourceObject.GetComponent<Resource>().resourceData = resource;
        resourceObject.transform.SetParent(transform);
        currentResourcesForDay++;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * resourceSpawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, resourceSpawnRadius, 1))
        {
            return hit.position;
        }
        return Vector3.zero; // Return a default position if no valid position is found
    }

    private ResourceData GetRandomResource()
    {
        int totalWeight = 0;
        foreach (ResourceData resource in resources)
        {
            totalWeight += resource.resourceWeight;
        }
        if (totalWeight <= 0) return null; // No valid resources to choose from

        int randomWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;
        foreach (ResourceData resource in resources)
        {
            currentWeight += resource.resourceWeight;
            if (currentWeight >= randomWeight)
            {
                return resource;
            }
        }
        return null; // Return null if no resource is selected (shouldn't happen)
    }

    public void ResetResourcesForDay()
    {
        currentResourcesForDay = 0;
    }
}
