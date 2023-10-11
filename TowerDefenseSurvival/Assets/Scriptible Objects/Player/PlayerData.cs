using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player Data")]

public class PlayerData : ScriptableObject
{
    [Header("Player Info")]
    public string playerName;
    public int playerID;
    [TextArea(3, 10)]
    public string description;

    [Header("Player Attributes")]
    public int level;
    public int experience;
    public int maxExperience;
    public int maxHealth;
    public int health;


    [Header("Player Stats")]
    public float experienceMultiplier;
    public float healthMultiplier;
    public float maxWeightMultiplier;
    public float resourceGainMultiplier;
    public float resourceGatherSpeedMultiplier;

    [Header("Player Movement")]
    public int speed;
    public int OverWeightSpeed;

    [Header("Player Inventory")]
    public WeaponData weaponData;

    public void Initialize()
    {
        level = 1;
        experience = 0;
        maxExperience = 100;
        health = maxHealth;
    }
}