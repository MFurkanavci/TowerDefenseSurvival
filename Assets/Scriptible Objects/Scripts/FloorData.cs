using UnityEngine;

[CreateAssetMenu(fileName = "FloorData", menuName = "Game/Floor Data")]
public class FloorData : ScriptableObject
{
    public GameObject floorPrefab;
    public BuildingType buildingType;

    public int floorHealth;
}
