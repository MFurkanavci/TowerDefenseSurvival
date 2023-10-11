using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefence : MonoBehaviour
{
    public TurretData turretData;

    private Transform target;
    private float fireCountdown = 0f;

    [Header("Turret Settings")]
    public float range = 10f;
    public float fireRate = 1f;
    public float bulletSpeed = 50f;
    public float rotationSpeed = 5.0f;

    [Header("Turret Components")]
    public Transform rotationPart;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    [Header("Turret Settings")]
    public int targetPriority = 0;

    enum TargetPriority
    {
        First,
        Last,
        Strongest,
        Weakest
    }

    void Start()
    {
        InitializeTurret();
    }

    void Update()
    {
        FindTargetByPriority();

        if (target != null)
        {
            RotateTurret();

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void FindTargetByPriority()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<Transform> validTargets = new List<Transform>();

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range)
            {
                validTargets.Add(enemy.transform);
            }
        }
        if (validTargets.Count > 0)
        {
            validTargets.Sort((a, b) => CompareTargets(a, b));
            target = validTargets[0];
        }
        else
        {
            target = null;
        }
    }

    int CompareTargets(Transform a, Transform b)
    {
        switch ((TargetPriority)targetPriority)
        {
            case TargetPriority.First:
                return 0;
            case TargetPriority.Last:
                return 1;
            case TargetPriority.Strongest:
                return a.GetComponent<EnemyDamage>().health.CompareTo(b.GetComponent<EnemyDamage>().health);
            case TargetPriority.Weakest:
                return b.GetComponent<EnemyDamage>().health.CompareTo(a.GetComponent<EnemyDamage>().health);
            default:
                return 0;
        }
    }

    void RotateTurret()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Bullet>().SOData(turretData);

        Vector3 shootingDirection = bulletSpawnPoint.forward;
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;

        Destroy(bullet, 3f);
    }

    void InitializeTurret()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.5f);
        var turret = Instantiate(turretData.turretPrefab, position, transform.rotation, transform);
        SetTurret(turretData);
    }

    public void SetTurret(TurretData data)
    {
        turretData = data;
        range = data.range;
        fireRate = data.fireRate;
        bulletSpeed = data.bulletSpeed;
        rotationSpeed = data.rotationSpeed;
        bulletPrefab = data.bulletPrefab;
        bulletSpawnPoint = transform.GetChild(0).GetChild(1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void ReplaceTurret()
    {
        Destroy(transform.GetChild(0).gameObject);
        InitializeTurret();
    }
}
