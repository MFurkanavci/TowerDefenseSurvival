using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource Data")]
public class ResourceData : ScriptableObject
{
    public ResourceTypes.Resources resourceTypes;
    public int resourceAmount;
    public int resourceMaxAmount;
    public int resourceMaxAmountPerDay;
    public float resourceRegenRate;
    public int resourceWeight;
    public int resourceQuality;

    public GameObject resourcePrefab;
}