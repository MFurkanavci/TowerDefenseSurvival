using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource Data")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public int resourceAmount;
    public int resourceMaxAmount;
    public int resourceRegenAmount;
    public float resourceRegenRate;
}