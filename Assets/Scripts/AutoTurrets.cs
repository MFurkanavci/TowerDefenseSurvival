using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurrets : MonoBehaviour
{
    [Header("Turret Settings")]
    public TurretData turretData;

    [Header("Turret Components")]
    [SerializeField] private Transform rotationPart;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Turret Range")]
    [SerializeField] private float baseRange = 10f;

    [Header("Turret Mechanics")]
    [SerializeField] private TargetPriority targetPriority = TargetPriority.Closest;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 50f;

    private Transform target;
    private float fireCountdown = 0f;

    private List<GameObject> validTargets = new List<GameObject>();

    public enum TargetPriority
    {
        Closest,
        Furthest,
        MostHealth,
        LeastHealth
    }

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void InitializeTurret(TurretData turretData)
    {
        Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity, transform);
        this.turretData = turretData;
        baseRange = turretData.range;
        fireRate = turretData.fireRate;
        bulletSpeed = turretData.bulletSpeed;
        bulletPrefab = turretData.bulletPrefab;
        bulletSpawnPoint = transform.GetChild(0).transform;
        rotationPart = transform;
    }

    public void SetRange(float range)
    {
        baseRange = range;
    }

    public float GetRange()
    {
        return baseRange;
    }

    //Get the valid targets
    void GetValidTargets(float turretRange, float maxAngle)
    {
        validTargets.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, turretRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = collider.transform.position - transform.position;
                float distanceToEnemy = directionToEnemy.magnitude;

                if (distanceToEnemy <= turretRange)
                {
                    float angleBetweenTurretAndEnemy = -CalculateAngle(collider.transform);

                    if (angleBetweenTurretAndEnemy <= maxAngle)
                    {
                        validTargets.Add(collider.gameObject);
                    }
                }
            }
        }
    }

    Transform GetTarget()
    {
        if (validTargets.Count > 0)
        {
            switch (targetPriority)
            {
                case TargetPriority.Closest:
                    return GetClosestTarget();
                case TargetPriority.Furthest:
                    return GetFurthestTarget();
                case TargetPriority.MostHealth:
                    return GetMostHealthTarget();
                case TargetPriority.LeastHealth:
                    return GetLeastHealthTarget();
                default:
                    return GetClosestTarget();
            }
        }
        else
        {
            return null;
        }
    }

    Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject validTarget in validTargets)
        {
            float distanceToTarget = Vector3.Distance(transform.position, validTarget.transform.position);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = validTarget.transform;
            }
        }

        return closestTarget;
    }

    Transform GetFurthestTarget()
    {
        Transform furthestTarget = null;
        float furthestDistance = 0f;

        foreach (GameObject validTarget in validTargets)
        {
            float distanceToTarget = Vector3.Distance(transform.position, validTarget.transform.position);

            if (distanceToTarget > furthestDistance)
            {
                furthestDistance = distanceToTarget;
                furthestTarget = validTarget.transform;
            }
        }

        return furthestTarget;
    }

    Transform GetMostHealthTarget()
    {
        Transform mostHealthTarget = null;
        float mostHealth = 0f;

        foreach (GameObject validTarget in validTargets)
        {
            float targetHealth = validTarget.GetComponent<EnemyDamage>().health;

            if (targetHealth > mostHealth)
            {
                mostHealth = targetHealth;
                mostHealthTarget = validTarget.transform;
            }
        }

        return mostHealthTarget;
    }

    Transform GetLeastHealthTarget()
    {
        Transform leastHealthTarget = null;
        float leastHealth = Mathf.Infinity;

        foreach (GameObject validTarget in validTargets)
        {
            float targetHealth = validTarget.GetComponent<EnemyDamage>().health;

            if (targetHealth < leastHealth)
            {
                leastHealth = targetHealth;
                leastHealthTarget = validTarget.transform;
            }
        }

        return leastHealthTarget;
    }

    void UpdateTarget()
    {
        float maxAngle = 25f;
        float turretRange = baseRange;
        GetValidTargets(turretRange, maxAngle);
        target = GetTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseRange);
    }

    void LookAtTarget(Transform target)
    {
        float heigthAngle = -CalculateAngle(target);

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion targetRotation = Quaternion.Euler(heigthAngle, lookRotation.eulerAngles.y, 0f);
        rotationPart.rotation = Quaternion.Slerp(rotationPart.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    float CalculateAngle(Transform target)
    {
        Vector3 direction = target.position - bulletSpawnPoint.position;
        float distance = direction.magnitude;
        float heightDifference = target.transform.position.y - bulletSpawnPoint.position.y;

        float tanAngle = Mathf.Atan2(heightDifference, distance);
        float angle = tanAngle * Mathf.Rad2Deg;
        return angle;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        TurretsCircularPositionAroundTheFloor(transform.parent, target);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.fixedDeltaTime;
    }

    void TurretsCircularPositionAroundTheFloor(Transform floor, Transform target)
    {
        Vector3 floorCenter = floor.position;
        float floorRadius = floor.lossyScale.x / 2;
        Vector3 turretPosition = transform.position;
        Vector3 targetPosition = target.position;
        Vector3 directionToTurret = turretPosition - floorCenter;
        Vector3 directionToTargetNormalized = (targetPosition - floorCenter).normalized;
        Vector3 EdgeOfFloor = floorCenter + directionToTargetNormalized * floorRadius;
        transform.position = EdgeOfFloor;

        transform.position = new Vector3(transform.position.x, turretPosition.y, transform.position.z);

        LookAtTarget(target);

    }

    void Shoot()
    {
        GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
        }
    }
}