using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script will be attached to the resource object, and will be used to determine how much resource the player will gain when they gather it

public class Resource : MonoBehaviour
{
    public int maxResource = 10;
    public int currentResource;

    public int resourceGain = 1;

    public ResourceData resourceData;

    public void Start()
    {
        SOData(resourceData);
    }

    public void Update()
    {
        if (currentResource <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GatherResource()
    {
        currentResource -= resourceGain;
    }

    public void GatherResource(int amount)
    {
        currentResource -= amount;
    }

    public void SOData(ResourceData data)
    {
        maxResource = resourceData.resourceMaxAmount;
        currentResource = resourceData.resourceAmount;
    }
}