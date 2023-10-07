using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGathering : MonoBehaviour
{
    public int resource = 0;
    public int maxResource = 10;
    public int resourceGain = 1;
    public TextMeshProUGUI resourceText;
    public Resource resourceScript;

    private bool isGathering = false;

    private void Update()
    {
        // Check if the resource is full before proceeding
        if (isFull)
        {
            return;
        }

        // Gather resources if conditions are met
        if (isGathering && resourceScript != null)
        {
            resourceTimer += Time.deltaTime;
            if (resourceTimer >= 1.0f)
            {
                GatherResource();
                UpdateResourceText();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Resource"))
        {
            if (Input.GetKey(KeyCode.E) && !isFull)
            {
                StartGathering(other.gameObject.GetComponent<Resource>());
            }
        }
    }

    private bool isFull = false;
    private float resourceTimer = 0.0f;

    private void StartGathering(Resource resourceToGather)
    {
        isGathering = true;
        resourceScript = resourceToGather;
    }

    private void GatherResource()
    {
        resourceScript.GatherResource(resourceGain);
        resource += resourceGain;
        resourceTimer = 0.0f;

        // Check if the resource is now full
        if (resource >= maxResource)
        {
            isFull = true;
        }
    }

    private void UpdateResourceText()
    {
        resourceText.text = "Resource: " + resource + "/" + maxResource;
    }
}
