using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAgent : MonoBehaviour
{
    //Size of level 
    public int mapMaxX = 25;
    public int mapMaxZ = 28;
    //States
    public enum StateEnemy
    {
        STARTING, FIRSTWAY, ATTACKING, ESCAPE, DEADING
    };
    public StateEnemy stateEnemy = StateEnemy.STARTING;
    //Starting variables
    [Header("Starting Propierties")]
    public Vector3 waypointStarting1;
    public Vector3 waypointStarting2;
    public float timeStarting;
    protected float initTimeStarting;
    protected bool selectorStartingPosition;
    //First position to block the respown
    [Header("First position to visit")]
    public Vector3 firstWaypoint;
    //Escaping variables
    protected GameObject player;
    protected int heightZones = 8;
    //Deading variables
    [Header("Deading Propierties")]
    public Vector3 waypointRespown;
    public float speedAttackingStarting;
    public float speedEscaping;
    public float speedDeading;
    protected StateEnemy lastStateEnemy;
    protected NavMeshAgent agent;
    protected Vector3 startingPosition;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        lastStateEnemy = StateEnemy.STARTING;
        selectorStartingPosition = false;
        initTimeStarting = timeStarting;
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (agent.enabled == true)
        {
            updateStateAndIfResetPath();

            switch (stateEnemy)
            {
                case StateEnemy.STARTING:
                    starting();
                    break;
                case StateEnemy.FIRSTWAY:
                    firstway();
                    break;
                case StateEnemy.ATTACKING:
                    attacking();
                    break;
                case StateEnemy.ESCAPE:
                    escape();
                    break;
                case StateEnemy.DEADING:
                    deading();
                    break;
                default:
                    attacking();
                    break;
            }
            FixLookRotation();
        }
    }

    protected void updateStateAndIfResetPath()
    {
        if (lastStateEnemy != stateEnemy)
        {
            agent.ResetPath();
            lastStateEnemy = stateEnemy;
            switch (stateEnemy)
            {
                case StateEnemy.STARTING:
                case StateEnemy.ATTACKING:
                    agent.speed = speedAttackingStarting;
                    break;
                case StateEnemy.ESCAPE:
                    agent.speed = speedEscaping;
                    break;
                case StateEnemy.DEADING:
                    agent.speed = speedDeading;
                    break;
                default:
                    agent.speed = speedAttackingStarting;
                    break;
            }
        }
    }

    protected void FixLookRotation()
    {
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.z))
        {
            if (agent.velocity.x != 0) transform.rotation = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0, 0));
        }
        else
        {
            if (agent.velocity.z != 0) transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, agent.velocity.z));
        }
    }
    protected void starting()
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
            lastStateEnemy = StateEnemy.STARTING;
            stateEnemy = StateEnemy.FIRSTWAY;
        }
    }
    protected void firstway()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            agent.SetDestination(firstWaypoint);
        }
        timeStarting -= Time.deltaTime;
    }
    public virtual void attacking()
    {
        
    }

    //Divide de map in 4 zones and check that the destination is different of the 
    //Player position, if is the same change the zone to the oposite.
    protected void escape()
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

    protected void deading()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {  
           agent.SetDestination(waypointRespown);   
        }
        if (Vector3.Distance(gameObject.transform.position, agent.destination) < 0.5f)
        {
            lastStateEnemy = stateEnemy;
            stateEnemy = StateEnemy.ATTACKING;
        }
    }
    public void port (Vector3 pos)
    {
        agent.nextPosition = pos;
        gameObject.transform.position = pos;
    }
    public void respown()
    {
        agent.nextPosition = startingPosition;
        agent.enabled = false;
        timeStarting = initTimeStarting;
        gameObject.transform.position = startingPosition;
        StartCoroutine(respownCoroutine());
    }

    IEnumerator respownCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        agent.enabled = true;
        agent.nextPosition = startingPosition;
        gameObject.transform.position = startingPosition;
        stateEnemy = StateEnemy.STARTING;
    }
}
