using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkEnemy : BaseEnemyAgent
{

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        updateStateAndIfResetPath();

        switch (stateEnemy)
        {
            case StateEnemy.STARTING:
                starting();
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
        //FIX LOOKROTATION
        FixLookRotation();
    }

    public override void attacking()
    {
        if (agent.hasPath == false && agent.pathPending == false)
        {
            agent.SetDestination(new Vector3(Random.Range(0f, mapMaxX/2), 0, Random.Range(0f, -mapMaxZ/2)));
        }
    }
}
