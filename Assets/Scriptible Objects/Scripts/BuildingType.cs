using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/Building Type")]
public class BuildingType : ScriptableObject
{
    public enum Type
    {
        Turret,
        Floor,
        Prospector, 
        Radiotower, 
        Smelter, 
        Constructor, 
        ResearchLab, 
        Storage, 
        Traps
    }

    public Type type;
    public int baseWoodCost;
    public int baseStoneCost;
    public int baseIronCost;
    public int baseGoldCost;

    public int upgradeWoodCost;
    public int upgradeStoneCost;
    public int upgradeIronCost;
    public int upgradeGoldCost;

    public int GetWoodCost(int upgradeLevel)
    {
        return baseWoodCost + (upgradeWoodCost * upgradeLevel);
    }

    public int GetStoneCost(int upgradeLevel)
    {
        return baseStoneCost + (upgradeStoneCost * upgradeLevel);
    }

    public int GetIronCost(int upgradeLevel)
    {
        return baseIronCost + (upgradeIronCost * upgradeLevel);
    }

    public int GetGoldCost(int upgradeLevel)
    {
        return baseGoldCost + (upgradeGoldCost * upgradeLevel);
    }
}