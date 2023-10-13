using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNAV : MonoBehaviour
{ 
    public Transform target;
    public Transform mainTarget;

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
            if(mainTarget != null)
                target = mainTarget;
            else
                return;
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

    public void SetMainTarget(Transform newTarget)
    {
        mainTarget = newTarget;
    }

}
