using System.Collections;
using UnityEngine;

public class MainTurretControls : MonoBehaviour
{
    public GameObject turret;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    
    public float rotationSpeed = 5.0f;
    public float maxTiltAngle = 45f;
    public float bulletSpeed = 20f;    
    public float fireRate = 1f;
    private float nextFire = 0f;
    public bool isInside = false;

    private void Update()
    {
        if (isInside)
        {
            HandleRotation();
            HandleShooting();
        }
    }

    private void HandleRotation()
    {
        // Get the mouse position in world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            // Calculate the rotation angle
            Vector3 turretPosition = turret.transform.position;
            Vector3 direction = targetPoint - turretPosition;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Smoothly interpolate turret rotation
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            // Limit the tilt angle
            float tiltAngle = Quaternion.Angle(turret.transform.rotation, rotation);
            if (tiltAngle > maxTiltAngle)
            {
                // Calculate a rotation with the maximum allowed tilt angle
                Quaternion maxTiltRotation = Quaternion.Euler(
                    maxTiltAngle * Mathf.Sign(direction.y), rotation.eulerAngles.y, rotation.eulerAngles.z);

                // Apply the maximum tilt rotation
                turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, maxTiltRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        // Create the bullet at the turret's position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Calculate the initial velocity based on the turret's rotation and tilt angle in world space
        Vector3 shootingDirection = bulletSpawn.forward;

        // Apply the velocity
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;

        // Destroy the bullet after a set time
        Destroy(bullet, 3f);
    }

    public void SetIsInside(bool inside)
    {
        isInside = inside;
    }
}
