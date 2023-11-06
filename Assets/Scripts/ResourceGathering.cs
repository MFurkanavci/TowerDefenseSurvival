using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGathering : MonoBehaviour
{
    public float maxResourceWeight = 10;
    public float resourceGain = 1;
    public float resourceGatherSpeed = 1.0f;
    public TextMeshProUGUI materialInfoText, totalWeightText,keyInfoText;

    private bool isInRange = false;

    private float resourceTimer = 0.0f;
    private CharaterMovement charaterMovement;
    private Resource resource;
    private ResourceTypes.Resources resourceTypes;
    private readonly Dictionary<ResourceTypes.Resources, float> resourceAndWeight = new Dictionary<ResourceTypes.Resources, float>();

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Respawning:
                Respawning();
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
    }

    Player player;
    private PlayerData playerData;
    public GameObject resourcePanel;

    private void Start()
    {
        playerData = GetComponent<Player>().playerData;
        SOData(playerData);
        InitializeResourceAmounts();
        UpdateInfoText();
        charaterMovement = GetComponent<CharaterMovement>();
    }

    private void InitializeResourceAmounts()
    {
        foreach (ResourceTypes.Resources _resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            resourceAndWeight.Add(_resource, 0);
        }
    }

    private void UpdateInfoText()
    {
        resourcePanel.SetActive(true);

        foreach (ResourceTypes.Resources _resource in resourceAndWeight.Keys)
        {
            float weight = resourceAndWeight[_resource];
            var resourceText = resourcePanel.transform.Find(_resource.ToString()).GetComponent<TextMeshProUGUI>();
            resourceText.text = $"{_resource}\n{Math.Round(weight,1)} kg.";
        }
        totalWeightText.text = $"Total Weight\n{Math.Round(GetTotalWeight(), 1)} kg. / {maxResourceWeight} kg.";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource _resource))
        {
            isInRange = true;
            string resourceWeight = Math.Round(_resource.currentResource, 2).ToString();
            string resourceType = "";
            resourceTypes = _resource.resourceData.resourceTypes;
            if(_resource.resourceData != null)
            {
                resourceType = _resource.resourceData.resourceTypes.ToString();
            }

            materialInfoText.text = $"{resourceWeight} kg. of {resourceType}";
            this.resource = _resource;

            if (_resource.currentResource > resourceGain)
            {
                keyInfoText.text = $"Hold E to gather \n{Math.Round(resourceGain,1)} kg. of {resourceType}";
            }
            else
            {
                keyInfoText.text = $"Hold E to gather \n{Math.Round(_resource.currentResource,1)} kg. of {resourceType}";
            }
            UpdateInfoText();
        }
    }

    private void LeavingResourceArea()
    {
        isInRange = false;
        resourceTimer = 0.0f;
        resource = null;
        materialInfoText.text = "";
        keyInfoText.text = "";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource _resource))
        {
            LeavingResourceArea();
        }
    }

    private void GatherResource()
    {
        if (isInRange && resource != null && !resource.IsEmpty())
        {
            resourceTimer += Time.deltaTime;
            if (resourceTimer >= resourceGatherSpeed)
            {
                resourceTimer = 0.0f;
                if (resourceGain < resource.currentResource)
                {
                    resourceAndWeight[resourceTypes] += resourceGain;
                    resource.currentResource -= resourceGain;
                }
                else
                {
                    resourceAndWeight[resourceTypes] += resource.currentResource;
                    resource.currentResource = 0;
                }
                resource.SetScale();
                UpdateInfoText();
            }
            resource.DeactivateResource();
        }
        else
        {
            LeavingResourceArea();
        }
    }

    private void Update()
    {
        if(resource == null)
        {
            keyInfoText.text = "";
            materialInfoText.text = "";
            isInRange = false;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            GatherResource();
        }

        UpdateWeightStats();
    }

    private void UpdateWeightStats()
    {
        switch (CurrentWeight())
        {
            case Weight.UnderWeight:
                SetUnderWeightStats();
                break;
            case Weight.NormalWeight:
                SetNormalWeightStats();
                break;
            case Weight.OverWeight:
                SetOverWeightStats();
                break;
        }
    }

    public void ResetResource()
    {
        foreach (ResourceTypes.Resources _resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            resourceAndWeight[_resource] = 0;
        }
        UpdateInfoText();
    }

    public float GetResourceAmount(ResourceTypes.Resources _resource)
    {
        return resourceAndWeight[_resource];
    }

    public void SOData(PlayerData data)
    {
        maxResourceWeight *= MathF.Round(data.maxWeightMultiplier,1);
        resourceGain *= MathF.Round(data.resourceGainMultiplier,1);
        resourceGatherSpeed /= MathF.Round(data.resourceGatherSpeedMultiplier,1);
    }

    public float GetTotalWeight()
    {
        float totalWeight = 0;
        foreach (ResourceTypes.Resources _resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            totalWeight += resourceAndWeight[_resource];
        }
        return totalWeight;
    }

    public enum Weight
    {
        UnderWeight,
        NormalWeight,
        OverWeight
    }

    public Weight CurrentWeight()
    {
        if (GetTotalWeight() < maxResourceWeight / 2)
        {
            return Weight.UnderWeight;
        }
        else if (GetTotalWeight() < maxResourceWeight)
        {
            return Weight.NormalWeight;
        }
        else
        {
            return Weight.OverWeight;
        }
    }

    private void SetUnderWeightStats()
    {
        resourceGatherSpeed = 1.0f / 1.5f;
        charaterMovement.SetSpeed(playerData.speed * 1.5f);
    }

    private void SetNormalWeightStats()
    {
        resourceGatherSpeed = 1.0f;
        charaterMovement.SetSpeed(playerData.speed);
    }

    private void SetOverWeightStats()
    {
        resourceGatherSpeed = 1.0f * 1.15f;
        charaterMovement.SetSpeed(playerData.OverWeightSpeed);
    }

    private void Respawning()
    {
        ResetResource();
        UpdateWeightStats();
        UpdateInfoText();
    }
}
