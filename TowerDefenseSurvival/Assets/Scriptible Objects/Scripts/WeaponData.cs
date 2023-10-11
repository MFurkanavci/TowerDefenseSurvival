using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Data")]

public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public int weaponID;
    [TextArea(3, 10)]
    public string description;

    [Header("Weapon Attributes")]
    public int damage;
    public float attackRange;
    public float attackRate;
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public float recoil;
    public float criticalChance;
    public float criticalDamage;

    [Header("Weapon Stats")]
    public float damageMultiplier;
    public float attackRangeMultiplier;
    public float attackRateMultiplier;
    public float maxAmmoMultiplier;
    public float reloadTimeMultiplier;
    public float recoilMultiplier;
    public float criticalChanceMultiplier;
    public float criticalDamageMultiplier;

}

