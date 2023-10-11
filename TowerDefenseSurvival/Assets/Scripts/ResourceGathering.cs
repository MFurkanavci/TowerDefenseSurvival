using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGathering : MonoBehaviour
{
    public int maxResource = 10;
    public int resourceGain = 1;
    [SerializeField] private bool isInRange = false;
    public float resourceGatherSpeed = 1.0f;
    private float resourceTimer = 0.0f;

    public TextMeshProUGUI infoText;

    private Resource resource;
    private ResourceTypes.Resources resourceTypes;

    public PlayerData playerData;
    public GameObject resourcePanel;
    public Dictionary<ResourceTypes.Resources, int> resourceAmounts = new Dictionary<ResourceTypes.Resources, int>();

    private void Start()
    {
        SOData(playerData);
        InitializeResourceAmounts();
        UpdateInfoText();
    }

    private void InitializeResourceAmounts()
    {
        foreach (ResourceTypes.Resources resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            resourceAmounts.Add(resource, 0);
        }
    }

    private void UpdateInfoText()
    {
        resourcePanel.SetActive(true);

        foreach (ResourceTypes.Resources resource in resourceAmounts.Keys)
        {
            int amount = resourceAmounts[resource];
            int max = maxResource;
            resourcePanel.transform.Find(resource.ToString()).GetComponent<TextMeshProUGUI>().text =
                $"{resource}: {amount}/{max}";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            isInRange = true;
            infoText.text = other.GetComponent<Resource>().resourceData.resourceTypes.ToString();
            resourceTypes = other.GetComponent<Resource>().resourceData.resourceTypes;
            resource = other.GetComponent<Resource>();
            UpdateInfoText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            isInRange = false;
        }
    }

    private void GatherResource()
    {
        if (isInRange && resource != null)
        {
            resourceTimer += Time.deltaTime;
            if (resourceTimer >= resourceGatherSpeed)
            {
                resourceTimer = 0.0f;
                if (resourceAmounts[resourceTypes] < maxResource)
                {
                    if (resource.currentResource <= resourceGain)
                    {
                        resourceAmounts[resourceTypes] += resource.currentResource;
                        resource.currentResource = 0;
                    }
                    else
                    {
                        resourceAmounts[resourceTypes] += resourceGain;
                        resource.currentResource -= resourceGain;
                    }
                    UpdateInfoText();
                    print("düzeltilmeli");

                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            GatherResource();
        }
    }

    public void ResetResource()
    {
        foreach (ResourceTypes.Resources resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            resourceAmounts[resource] = 0;
        }
        UpdateInfoText();
    }

    public int GetResourceAmount(ResourceTypes.Resources resource)
    {
        return resourceAmounts[resource];
    }

    public void SOData(PlayerData data)
    {
        maxResource *= data.maxResourceMultiplier;
        resourceGain *= data.resourceGainMultiplier;
        resourceGatherSpeed /= data.resourceGatherSpeedMultiplier;
    }
}
