using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Game/Resource Data")]
public class ResourceData : ScriptableObject
{
    public ResourceTypes.Resources resourceTypes;
    public float resourceMinWeightAmount;
    public float resourceMaxWeightAmount;
    public int resourceMaxAmountPerDay;
    public float resourceRegenRate;
    public float resourceWeight;
    public int resourceQuality;
    public GameObject resourcePrefab;
}