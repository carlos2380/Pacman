using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAgent : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;

    public Vector3 waypointStarting1;
    public Vector3 waypointStarting2;
    public float timeStarting;
    private float initTimeStarting;
    private bool selectorStartingPosition;

    public enum StateEnemy { 
        STARTING, ATTACKING, LEAVING, DEADING };
    public StateEnemy stateEnemy = StateEnemy.STARTING;

    private StateEnemy lastStateEnemy;
    private NavMeshAgent agent;
    private int x;
    private bool getNewPath = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        //agent.SetDestination(waypoint2.position);
        x = 2;
        lastStateEnemy = StateEnemy.STARTING;
        selectorStartingPosition = false;
        initTimeStarting = timeStarting;
    }

    // Update is called once per frame
    void Update()
    {
        //Update State
       
        if(lastStateEnemy != stateEnemy)
        {
            agent.ResetPath();
            lastStateEnemy = stateEnemy;
        }

        switch (stateEnemy)
        {
            case StateEnemy.STARTING:
                starting();
                break;
            default:
                defaultMov();
                break;
        }

       
        //FIX LOOKROTATION
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.z))
        {
            if(agent.velocity.x != 0) transform.rotation = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0, 0));
        }else
        {
            if (agent.velocity.z != 0) transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, agent.velocity.z));
        }

        
    }

    private void starting()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            if (selectorStartingPosition)
            {
                agent.SetDestination(waypointStarting1);
            }
            else
            {
                agent.SetDestination(waypointStarting2);
            }
            selectorStartingPosition = !selectorStartingPosition;
        }
        timeStarting -= Time.deltaTime;
        if(timeStarting < 0)
        {
            stateEnemy = StateEnemy.ATTACKING;
        }
    }

    private void defaultMov()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            if (x == 2)
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
