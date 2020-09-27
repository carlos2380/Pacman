using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAgent : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;
    //Size of level 
    public int mapMaxX = 25;
    public int mapMaxZ = 28;
    //States
    public enum StateEnemy
    {
        STARTING, ATTACKING, ESCAPE, DEADING
    };
    public StateEnemy stateEnemy = StateEnemy.STARTING;
    //Starting variables
    [Header("Starting Propierties")]
    public Vector3 waypointStarting1;
    public Vector3 waypointStarting2;
    public float timeStarting;
    private float initTimeStarting;
    private bool selectorStartingPosition;
    //Escaping variables
    private GameObject player;
    private int heightZones = 8;

    private StateEnemy lastStateEnemy;
    private NavMeshAgent agent;

    


    private int x;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        //agent.SetDestination(waypoint2.position);
        x = 2;
        lastStateEnemy = StateEnemy.STARTING;
        selectorStartingPosition = false;
        initTimeStarting = timeStarting;
        player = GameObject.FindGameObjectWithTag("Player");
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
            case StateEnemy.ATTACKING:
                defaultMov();
                break;
            case StateEnemy.ESCAPE:
                escape();
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
            stateEnemy = StateEnemy.ESCAPE;
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

    //Divide de map in 4 zones and check that the destination is different of the 
    //Player position, if is the same change the zone to the oposite.
    private void escape()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            float xPos = Random.Range(0, mapMaxX) + 0.5f;
            Vector2 vec2 = new Vector2(-Random.Range(0, heightZones) + 0.5f, -Random.Range(mapMaxZ - heightZones, mapMaxZ) + 0.5f);
            float zPos = vec2[Random.Range(0, 2)];

            //Check if the position is in 
            bool changeSquare = true;
            if ((xPos < mapMaxX / 2 && player.transform.position.x > mapMaxX / 2 )|| (xPos > mapMaxX / 2 && player.transform.position.x < mapMaxX / 2)) {
                changeSquare = false;
            }
            else {
                if (zPos > -heightZones && player.transform.position.z < -heightZones)
                {
                    changeSquare = false;
                }
                else if (zPos < (-mapMaxZ + heightZones) && player.transform.position.z > (-mapMaxZ + heightZones))
                {
                    changeSquare = false;
                }
            }
            if(changeSquare == true)
            {
                if(xPos < mapMaxX/2)
                {
                    xPos += mapMaxX / 2f;
                }else
                {
                    xPos -= mapMaxX / 2f;
                }
                zPos = -(zPos + mapMaxZ);
             
            }
            agent.SetDestination(new Vector3(xPos, gameObject.transform.position.y, zPos));
        }
    }

    public void port (Vector3 pos)
    {
        agent.nextPosition = pos;
        gameObject.transform.position = pos;
    }
}
