using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player Data")]

public class PlayerData : ScriptableObject
{
    public string playerName;
    public int health;
    public int maxResourceMultiplier;
    public int resourceGainMultiplier;
    public float resourceGatherSpeedMultiplier;

    public WeaponData weaponData;
}