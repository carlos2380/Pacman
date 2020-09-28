using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemy : BaseEnemyAgent
{
    public Transform[] transfPositionsPower;
    private int lastPosition;
    protected override void Start()
    {
        base.Start();
        lastPosition = 0;
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
            agent.SetDestination(transfPositionsPower[(lastPosition+Random.Range(1, transfPositionsPower.Length -1)) % transfPositionsPower.Length].position);
        }
    }
}
