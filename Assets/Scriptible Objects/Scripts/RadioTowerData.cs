using UnityEngine;

[CreateAssetMenu(fileName = "RadioTowerData", menuName = "Game/Radio Tower Data")]
public class RadioTowerData : ScriptableObject
{
    public BuildingType buildingType;
    public new string name;
    public GameObject radioTowerPrefab;
    public float radius;
}

