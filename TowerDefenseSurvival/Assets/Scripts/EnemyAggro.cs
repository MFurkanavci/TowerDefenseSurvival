using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public float aggroRange = 10.0f;
    public float aggroAngle = 45.0f;
    public float aggroTime = 5.0f;
    public float aggroTimer = 0.0f;
    public bool isAggro = false;
    public Transform target;
    public EnemyNAV enemyNAV;

    private void Update()
    {
        if (isAggro)
        {
            aggroTimer += Time.deltaTime;
            if (aggroTimer >= aggroTime)
            {
                isAggro = false;
                aggroTimer = 0.0f;
                enemyNAV.SetTarget(null);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isAggro)
            {
                return;
            }
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (direction.magnitude <= aggroRange && angle <= aggroAngle)
            {
                isAggro = true;
                target = other.transform;
                enemyNAV.SetTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAggro = false;
            aggroTimer = 0.0f;
            enemyNAV.SetTarget(null);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, aggroAngle, 0) * transform.forward * aggroRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -aggroAngle, 0) * transform.forward * aggroRange);
    }
}
