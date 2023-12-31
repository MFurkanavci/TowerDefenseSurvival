using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNAV : MonoBehaviour
{ 
    public Transform target;
    public Transform mainTarget;

    public bool isAggro = false;
    public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
}
