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
}