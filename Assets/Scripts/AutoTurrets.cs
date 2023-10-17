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
    Transform GetTarget(float turretRange, float maxAngle)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> validTargets = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= turretRange)
            {
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                float angleBetweenTurretAndEnemy = -CalculateAngle(enemy.transform);

                if (angleBetweenTurretAndEnemy <= maxAngle)
                {
                    validTargets.Add(enemy);
                }
            }
        }

        if (validTargets.Count > 0)
        {
            switch (targetPriority)
            {
                case TargetPriority.Closest:
                    return GetClosestTarget(validTargets);
                case TargetPriority.Furthest:
                    return GetFurthestTarget(validTargets);
                case TargetPriority.MostHealth:
                    return GetMostHealthTarget(validTargets);
                case TargetPriority.LeastHealth:
                    return GetLeastHealthTarget(validTargets);
                default:
                    return GetClosestTarget(validTargets);
            }
        }
        else
        {
            validTargets.RemoveRange(0, validTargets.Count);
            return null;
        }
    }

    TargetPriority GetTargetPriority()
    {
        return targetPriority;
    }

    Transform GetClosestTarget(List<GameObject> validTargets)
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

    Transform GetFurthestTarget(List<GameObject> validTargets)
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

    Transform GetMostHealthTarget(List<GameObject> validTargets)
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

    Transform GetLeastHealthTarget(List<GameObject> validTargets)
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
        float maxAngle = 45f;
        float turretRange = baseRange;
        target = GetTarget(turretRange, maxAngle);
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
        Vector3 rotation = Quaternion.Lerp(rotationPart.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPart.rotation = Quaternion.Euler(heigthAngle, rotation.y, 0f);
        
    }

    float CalculateAngle(Transform target)
    {
        Vector3 direction = target.position - bulletSpawnPoint.position;
        float distance = direction.magnitude;
        float heightDifference = target.transform.position.y - transform.position.y;

        float tanAngle = Mathf.Atan2(heightDifference, distance);
        float angle = tanAngle * Mathf.Rad2Deg;
        return angle * 2;
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        LookAtTarget(target);
    }

    void TurretsCircularPositionAroundTheFloor(Transform floor, Transform target)
    {
        float radius = 1.5f;
        float angle = 0f;
        float angleStep = 360f / floor.childCount;

        foreach (Transform turret in floor)
        {
            Vector3 turretPosition = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0f, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);
            turret.transform.position = turretPosition;
            angle += angleStep;
        }
    }
}
