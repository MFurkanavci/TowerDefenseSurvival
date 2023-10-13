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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            Vector3 turretPosition = turret.transform.position;
            Vector3 direction = targetPoint - turretPosition;
            Quaternion rotation = Quaternion.LookRotation(direction);

            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            float tiltAngle = Quaternion.Angle(turret.transform.rotation, rotation);
            if (tiltAngle > maxTiltAngle)
            {
                Quaternion maxTiltRotation = Quaternion.Euler(
                    maxTiltAngle * Mathf.Sign(direction.y), rotation.eulerAngles.y, rotation.eulerAngles.z);

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
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        Vector3 shootingDirection = bulletSpawn.forward;
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;

        Destroy(bullet, 3f);
    }

    public void SetIsInside(bool inside)
    {
        isInside = inside;
    }
}
