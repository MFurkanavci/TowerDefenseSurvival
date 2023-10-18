using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNAV : MonoBehaviour
{ 
    public Transform target;

    public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = GetComponent<EnemyDamage>().enemyData.speed;
    }

    private void Update()
    {
        if (target == null)
        {
            agent.SetDestination(new Vector3(0, 0, 0));
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

}
