using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemy : BaseEnemyAgent
{
    public Transform[] transfPositionsPower;
    private Vector3[] positionsPower;
    private int lastPosition;
    protected override void Start()
    {
        base.Start();
        lastPosition = 0;
        positionsPower = new Vector3[transfPositionsPower.Length];
        for (int i = 0; i < transfPositionsPower.Length; ++i)
        {
            positionsPower[i] = transfPositionsPower[i].position;
        }
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
            agent.SetDestination(positionsPower[(lastPosition+Random.Range(1, positionsPower.Length -1)) % positionsPower.Length]);
        }
    }
}
