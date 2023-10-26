using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resource : MonoBehaviour
{
    public float maxResource;
    public float minResource;
    public float currentResource;

    public ResourceData resourceData;

    public ResourceTypes.Resources resourceTypes;

    public GameObject resourcePrefab;
    public bool IsEmpty()
    {
        return currentResource < 0.1f;
    }

    public void DeactivateResource()
    {
        if (IsEmpty())
        {
            gameObject.name = "Resource";
            resourceData = null;
            ObjectPooler.Instance.ReturnObject(gameObject,gameObject);
            
            ResourceGenerator.currentResourcesForDay--;
        }
    }

    public float GatherResource(float amount)
    {
        if (currentResource >= amount)
        {
            currentResource -= amount;
            return amount;
        }
        else
        {
            float temp = currentResource;
            currentResource = 0;
            return temp;
        }
    }

    public void SOData(ResourceData data)
    {
        resourceTypes = data.resourceTypes;
        maxResource = data.resourceMaxWeightAmount;
        minResource = data.resourceMinWeightAmount;
        currentResource = MathF.Round(UnityEngine.Random.Range(minResource, maxResource), 1);

        resourcePrefab = data.resourcePrefab;

        gameObject.name = resourceTypes.ToString();
    }

    public void SetScale()
    {
        float scale = Mathf.Clamp(currentResource / maxResource, 0.5f, 1.5f);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}