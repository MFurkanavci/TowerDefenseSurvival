using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    //we will be generating resources in the random position on top of the terrain(Plain)

    [SerializeField]
    private GameObject resourcePrefab;

    [SerializeField]
    private int numberOfResources = 10;

    [SerializeField]
    private float radius = 5f;

    void Start()
    {
        GenerateResources();
    }

    void GenerateResources()
    {
        for (int i = 0; i < numberOfResources; i++)
        {
            //generate random position
            Vector3 randomPosition = GetRandomPosition();

            //instantiate resource
            GameObject resource = Instantiate(resourcePrefab, randomPosition, Quaternion.identity);

            //set parent
            resource.transform.SetParent(transform);
        }
    }

    Vector3 GetRandomPosition()
    {
        //get random position on top of the terrain
        Vector3 randomPosition = Random.insideUnitSphere * radius;

        //set y to 0
        randomPosition.y = 0;

        //return random position
        return randomPosition;
    }
}
