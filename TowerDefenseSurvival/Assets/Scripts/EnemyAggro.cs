using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public float aggroRange = 10.0f;
    public float aggroAngle = 45.0f;
    public float aggroTime = 5.0f;
    public Vector3 aggroStopDistance;
    public Transform target;
    public EnemyNAV enemyNAV;

    private Transform player;
    private float aggroTimer = 0.0f;
    private bool isAggro = false;

    private void Start()
    {
        Player playerComponent = FindObjectOfType<Player>();
        if (playerComponent != null)
        {
            player = playerComponent.playerTransform;
        }
    }

    private void Update()
    {
        if (!isAggro)
        {
            CheckForAggro();
        }
        else
        {
            HandleAggroTimer();
        }
    }

    private void HandleAggroTimer()
    {
        aggroTimer += Time.deltaTime;
        if (aggroTimer >= aggroTime)
        {
            isAggro = false;
            aggroTimer = 0.0f;
            enemyNAV.SetTarget(null);
        }
    }

    private void CheckForAggro()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (direction.magnitude <= aggroRange && angle <= aggroAngle)
        {
            isAggro = true;
            target = player;
            enemyNAV.SetTarget(target);
            print("fix");
        }
    }
}
