using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyAgent : BaseEnemyAgent
{
    public float MaxDistToAttack;
    protected override void Start()
    {
        base.Start();
    }    
    protected override void Update()
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

    public override void attacking()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            float x = player.transform.position.x + Random.Range(-MaxDistToAttack, MaxDistToAttack);
            float z = player.transform.position.z + Random.Range(-MaxDistToAttack, MaxDistToAttack);
            if (x < 0) x = 0;
            if (z > 0) z = 0;
            if (x > mapMaxX + 1) x = mapMaxX + 0.5f;
            if (z < -mapMaxZ - 1) z = -mapMaxZ - 0.5f;
            agent.SetDestination(new Vector3(x, 0.5f, z));
        }
    }
}
