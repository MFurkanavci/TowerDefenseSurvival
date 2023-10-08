using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNAV : MonoBehaviour
{ 
    public Transform target;

    public Transform mainTarget;
    private NavMeshAgent agent;

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

    private void destionationReached()
    {
        Debug.Log("Destination Reached");
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
