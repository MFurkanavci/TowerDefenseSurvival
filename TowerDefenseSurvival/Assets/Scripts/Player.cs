using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Datas")]
    public PlayerData playerData;
    public WeaponData weaponData;

    [Header("Stats")]
    public int level;
    public int experience;
    public int maxExperience;
    public int maxHealth;
    public int health;

    public void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    public void Start()
    {
        playerData.Initialize();

        InitializeDataToStats();
    }

    public void InitializeDataToStats()
    {
        level = playerData.level;
        experience = playerData.experience;
        maxExperience = playerData.maxExperience;
        maxHealth = playerData.maxHealth;
        health = playerData.health;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Heal(int heal)
    {
        health += heal;
    }

    public void AddExperience(int experience)
    {
        this.experience += experience;
    }

    public void LevelUp()
    {
        level++;
        experience = 0;
        maxExperience += maxExperience * 2;
    }

    public bool IsLevelUp()
    {
        return experience >= maxExperience;
    }
}
