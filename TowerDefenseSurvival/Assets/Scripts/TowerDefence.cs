using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefence : MonoBehaviour
{
    public Transform target; // The current target (enemy) to shoot at
    public float range = 10f; // The range within which the turret can detect and shoot enemies
    public float fireRate; // Rate of fire (shots per second)
    private float fireCountdown = 0f; // Countdown timer for shooting
    public float bulletSpeed; // Speed of the bullet
    public float rotationSpeed; // Speed of turret rotation

    public Transform rotationPart; // The part of the turret that rotates to aim at the target
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform bulletSpawnPoint; // The position where bullets are spawned

    public TurretData turretData;

    void Start()
    {
        // Set the fire countdown to zero so that the turret shoots immediately
        fireCountdown = 0f;
        SOData(turretData);
    }

    void Update()
    {
        FindTarget(); // Find the nearest target within range

        if (target != null)
        {
            // Rotate the turret to aim at the target
            RotateTurret();

            // Check if it's time to shoot
            if (fireCountdown <= 0f)
            {
                Shoot(); // Fire a bullet at the target
                fireCountdown = 1f / fireRate; // Reset the fire countdown
            }

            // Decrement the fire countdown
            fireCountdown -= Time.deltaTime;
        }
    }

    void FindTarget()
    {
        // Find all enemies in the scene (you might want to optimize this later)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        // Check if the nearest enemy is within range
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null; // No target within range
        }
    }

    void RotateTurret()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate the turret towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Shoot()
    {
        // Create a bullet at the bullet spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        
        Vector3 shootingDirection = bulletSpawnPoint.forward;

        // Apply the velocity
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;

        // Destroy the bullet after a set time
        Destroy(bullet, 3f);

    }

    // Draw a wire sphere to show the range of the turret
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SOData(TurretData turretData)
    {
        range = turretData.range;
        fireRate = turretData.fireRate;
        bulletSpeed = turretData.bulletSpeed;
        rotationSpeed = turretData.rotationSpeed;
        bulletPrefab = turretData.bulletPrefab;
        bulletSpawnPoint = gameObject.transform.GetChild(0);
    }
}
