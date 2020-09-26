using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAgent : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;


    private NavMeshAgent agent;
    private int x;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.SetDestination(waypoint2.position);
        x = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.z))
        {
            if(agent.velocity.x != 0) transform.rotation = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0, 0));
        }else
        {
            if (agent.velocity.z != 0) transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, agent.velocity.z));
        }

        if(agent.hasPath == false && agent.pathPending == false)
        {
            if(x == 2)
            {
                agent.SetDestination(waypoint1.position);
                x = 1;
            }
            else
            {
                x = 2;
                agent.SetDestination(waypoint2.position);
            }
        }
    }

    public void port (Vector3 pos)
    {
        agent.nextPosition = pos;
        gameObject.transform.position = pos;
    }
}
