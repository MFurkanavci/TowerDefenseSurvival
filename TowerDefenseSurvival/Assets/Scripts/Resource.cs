using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resource : MonoBehaviour
{
    public int maxResource = 10;
    public int currentResource;

    public int resourceGain = 1;

    public ResourceData resourceData;

    public ResourceTypes.Resources resourceTypes;

    public void Start()
    {
        SOData(resourceData);
    }

    public void Update()
    {
        if (currentResource <= 0)
        {
            ResourceGenerator.currentResourcesForDay--;
            Destroy(gameObject);
        }
    }

    public int GatherResource(int amount)
    {
        if (currentResource >= amount)
        {
            currentResource -= amount;
            return amount;
        }
        else
        {
            int temp = currentResource;
            currentResource = 0;
            return temp;
        }
    }

    public void SOData(ResourceData data)
    {
        maxResource = resourceData.resourceMaxAmount;
        currentResource = resourceData.resourceAmount;
        resourceTypes = resourceData.resourceTypes;
    }
}