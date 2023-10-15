using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefense : MonoBehaviour
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

    public TargetPriority targetPriority = TargetPriority.Closest;

    public enum TargetPriority
    {
        Closest,
        Furthest,
        Strongest,
        Weakest
    }

    private void Start()
    {
        // InitializeTurret(turretData);
    }

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            RotateTurret();

            if (IsInMinRange())
            {
                return;
            }

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    private bool IsInMinRange()
    {
        if (target != null)
        {
            float angleToTarget = Vector3.Angle(rotationPart.forward, target.position - rotationPart.position);
            return angleToTarget > 40f;
        }
        return false;
    }

    private void FindTarget()
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

    private int CompareTargets(Transform a, Transform b)
    {
        switch (targetPriority)
        {
            case TargetPriority.Closest:
                return Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position));
            case TargetPriority.Furthest:
                return Vector3.Distance(transform.position, b.position).CompareTo(Vector3.Distance(transform.position, a.position));
            case TargetPriority.Strongest:
                return a.GetComponent<EnemyDamage>().health.CompareTo(b.GetComponent<EnemyDamage>().health);
            case TargetPriority.Weakest:
                return b.GetComponent<EnemyDamage>().health.CompareTo(a.GetComponent<EnemyDamage>().health);
            default:
                return 0;
        }
    }

    private void RotateTurret()
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - rotationPart.position);
        rotationPart.rotation = Quaternion.Slerp(rotationPart.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Bullet>().SOData(turretData);

        Vector3 shootingDirection = bulletSpawnPoint.forward;
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;

        Destroy(bullet, 3f);
    }

    public void InitializeTurret(TurretData data)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.5f);
        var turret = Instantiate(data.turretPrefab, position, transform.rotation, transform);

        turretData = data;
        range = data.range;
        fireRate = data.fireRate;
        bulletSpeed = data.bulletSpeed;
        rotationSpeed = data.rotationSpeed;
        bulletPrefab = data.bulletPrefab;
        bulletSpawnPoint = transform.GetChild(0).GetChild(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
