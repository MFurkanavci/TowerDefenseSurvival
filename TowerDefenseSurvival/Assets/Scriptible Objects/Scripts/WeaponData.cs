using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Data")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float attackRange;
    public float attackRate;
}

