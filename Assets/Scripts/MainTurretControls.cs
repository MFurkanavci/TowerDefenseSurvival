using System.Collections;
using UnityEngine;

public class MainTurretControls : MonoBehaviour
{
    public GameObject turret;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float rotationSpeed = 1f;
    public float maxTiltAngle = 45f;
    public float bulletSpeed = 100f;
    public float fireRate = 1f;

    private float nextFire = 0f;
    private bool isInside = false;

    private Plane ground;

    private void Start()
    {
        ground = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (isInside)
        {
            TurretsCircularPositionAroundTheFloor(transform.parent,MousePosition());
            HandleRotation();
            HandleShooting();
        }
    }

    private Vector3 MousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            return targetPoint;
        }
        return Vector3.zero;
    }

    private void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 turretPosition = turret.transform.position;
            Vector3 direction = targetPoint - turretPosition;

            Quaternion rotation = Quaternion.LookRotation(direction);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation,1f);

            // Limit the tilt angle
            float tiltAngle = Quaternion.Angle(turret.transform.rotation, rotation);
            if (tiltAngle > maxTiltAngle)
            {
                Quaternion maxTiltRotation = Quaternion.Euler(maxTiltAngle * Mathf.Sign(direction.y), rotation.eulerAngles.y, rotation.eulerAngles.z);
                turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, maxTiltRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate; // Use inverse for better precision
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Vector3 shootingDirection = bulletSpawn.forward;
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletSpeed;
    }

    public void SetIsInside(bool inside)
    {
        isInside = inside;
    }

    private void TurretsCircularPositionAroundTheFloor(Transform floor,Vector3 target)
    {
        Vector3 floorCenter = floor.position;
        float floorRadius = floor.lossyScale.x / 2;
        Vector3 turretPosition = transform.position;
        Vector3 targetPosition = target;
        Vector3 directionToTurret = turretPosition - floorCenter;
        Vector3 directionToTargetNormalized = (targetPosition - floorCenter).normalized;
        Vector3 EdgeOfFloor = floorCenter + directionToTargetNormalized * floorRadius;
        transform.position = EdgeOfFloor;

        transform.position = new Vector3(transform.position.x, turretPosition.y, transform.position.z);

    }
}
