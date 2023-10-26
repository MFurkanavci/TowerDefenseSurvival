using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerResources : MonoBehaviour
{
    ResourceTypes.Resources resourceTypes;
    public float Wood { get; private set; }
    public float Stone { get; private set; }
    public float Iron { get; private set; }
    public float Gold { get; private set; }


    public int woodMax;
    public int stoneMax;
    public int ironMax;
    public int goldMax;

    public GameObject resourcepanel;

    public void Start()
    {
        Wood = 0;
        Stone = 0;
        Iron = 0;
        Gold = 0;
        UpdateResources();
    }

    public void Update()
    {
        if (Wood > woodMax)
        {
            Wood = woodMax;
        }
        if (Stone > stoneMax)
        {
            Stone = stoneMax;
        }
        if (Iron > ironMax)
        {
            Iron = ironMax;
        }
        if (Gold > goldMax)
        {
            Gold = goldMax;
        }
    }

    public void ModifyResource(ResourceTypes.Resources resource, float amount)
    {
        switch (resource)
        {
            case ResourceTypes.Resources.Wood:
                Wood += amount;
                break;
            case ResourceTypes.Resources.Stone:
                Stone += amount;
                break;
            case ResourceTypes.Resources.Iron:
                Iron += amount;
                break;
            case ResourceTypes.Resources.Gold:
                Gold += amount;
                break;
        }
        UpdateResources();
    }
    public void UpdateResources()
    {
        resourcepanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Wood: {Wood} kg. / {woodMax} kg.";
        resourcepanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Stone: {Stone} kg. / {stoneMax} kg.";
        resourcepanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Gold: {Gold} kg. / {goldMax} kg.";
        resourcepanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Iron: {Iron} kg. / {ironMax} kg.";
    }

    public bool HasResources(BuildingType buildingType)
    {
        if (Wood >= buildingType.baseWoodCost && Stone >= buildingType.baseStoneCost && Iron >= buildingType.baseIronCost && Gold >= buildingType.baseGoldCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendResources(BuildingType buildingType)
    {
        Wood -= buildingType.baseWoodCost;
        Stone -= buildingType.baseStoneCost;
        Iron -= buildingType.baseIronCost;
        Gold -= buildingType.baseGoldCost;
        UpdateResources();
    }

    public void AddResources(BuildingType buildingType)
    {
        Wood += buildingType.baseWoodCost;
        Stone += buildingType.baseStoneCost;
        Iron += buildingType.baseIronCost;
        Gold += buildingType.baseGoldCost;
        UpdateResources();
    }

}
