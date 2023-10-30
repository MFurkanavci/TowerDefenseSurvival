using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    public FloorData floorData;

    [SerializeField] private int floorIndex;
    [SerializeField] public int floorHealth;
    [SerializeField] private int floorMaxHealth;
    [SerializeField] private List<int> floorCosts;
    [SerializeField] private int floorLevel;

    public void SetFloorData(FloorData floorData)
    {
        this.floorData = floorData;
        floorHealth = floorData.floorMaxHealth;
        floorMaxHealth = floorData.floorMaxHealth;
        floorCosts = new List<int>();
        CalculateUpgradeCost(floorData.buildingType);
    }

    private void CalculateUpgradeCost(BuildingType buildingType)
    {
        floorCosts.Add(buildingType.GetWoodCost(floorLevel));
        floorCosts.Add(buildingType.GetStoneCost(floorLevel));
        floorCosts.Add(buildingType.GetIronCost(floorLevel));
        floorCosts.Add(buildingType.GetGoldCost(floorLevel));
    }

    public void LevelUp()
    {
        floorLevel++;
        floorMaxHealth += floorMaxHealth / 2;
        CalculateUpgradeCost(floorData.buildingType);
    }

    public int GetMaxHealth()
    {
        return floorMaxHealth;
    }
}
