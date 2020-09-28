using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGame : MonoBehaviour
{
    public GameObject groupDots;
    public GameObject[] enemies;
    private int numDots;
    public float timePowerUp;
    private bool powerUp;
    private float nowTimePowerUp;
    // Start is called before the first frame update
    void Start()
    {
        powerUp = false;
        numDots = groupDots.transform.childCount;
    }

    void Update()
    {
        if(powerUp == true)
        {
            nowTimePowerUp -= Time.deltaTime;
            if(nowTimePowerUp < 0)
            {
                powerUp = false;
                setEnemiesToAttack();
            }
        }
    }
    public void dotEated()
    {
        --numDots;
        if(numDots <= 0)
        {
            win();
        }
    }

    public void win()
    {
        Debug.Log("YOU WIN!!!!!");
    }

    public void setEnemiesToEscape()
    {
        nowTimePowerUp = timePowerUp;
        powerUp = true;
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemies[i].GetComponent<BaseEnemyAgent>().stateEnemy = BaseEnemyAgent.StateEnemy.ESCAPE;
        }
    }

    private void setEnemiesToAttack()
    {
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemies[i].GetComponent<BaseEnemyAgent>().stateEnemy = BaseEnemyAgent.StateEnemy.ATTACKING;
        }
    }
}
