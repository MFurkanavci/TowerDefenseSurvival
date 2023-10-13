using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Turret Data")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public int damage;
    public float range;
    public float fireRate;
    public float bulletSpeed;
    public float rotationSpeed;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public GameObject turretPrefab;
}