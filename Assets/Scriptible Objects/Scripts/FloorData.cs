using UnityEngine;

[CreateAssetMenu(fileName = "FloorData", menuName = "Game/Floor Data")]
public class FloorData : ScriptableObject
{
    public new string name;
    public GameObject floorPrefab;
    public BuildingType buildingType;

    public int floorMaxHealth;
    
}
