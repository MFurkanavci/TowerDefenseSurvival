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
    private bool isDead = false;
    private Plane ground;

    [SerializeField] private GameObject towerTopFloor;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        ground = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (isInside && !isDead)
        {
            TurretsCircularPositionAroundTheFloor(towerTopFloor.transform,MousePosition());
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
        Vector3 targetPoint = GetMousePositionOnGround();
        Vector3 turretPosition = turret.transform.position;
        Vector3 direction = targetPoint - turretPosition;

        Quaternion rotation = Quaternion.LookRotation(direction);
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Limit the tilt angle
        float tiltAngle = Quaternion.Angle(turret.transform.rotation, rotation);
        if (tiltAngle > maxTiltAngle)
        {
            Quaternion maxTiltRotation = Quaternion.Euler(maxTiltAngle * Mathf.Sign(direction.y), rotation.eulerAngles.y, rotation.eulerAngles.z);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, maxTiltRotation, rotationSpeed * Time.deltaTime);
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

    private Vector3 GetMousePositionOnGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public void SetIsInside(bool inside)
    {
        isInside = inside;
    }

    private void TurretsCircularPositionAroundTheFloor(Transform floor, Vector3 target)
    {
        Vector3 floorCenter = floor.position;
        float floorRadius = floor.lossyScale.x;
        Vector3 directionToTargetNormalized = (target - floorCenter).normalized;
        Vector3 edgeOfFloor = floorCenter + directionToTargetNormalized * floorRadius;

        transform.position = new Vector3(edgeOfFloor.x, transform.position.y, edgeOfFloor.z);
    }

    private void HandleGameStateChanged(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.Respawning:
                isDead = true;
                break;
            case GameState.Playing:
                isDead = false;
                break;
        }
    }
}
