using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGame : MonoBehaviour
{
    public GameObject groupDots;
    public GameObject[] enemies;
    public int numLifes;
    public GameObject door;
    public float timeToCloseDoor;
    private CtrlPlayer ctrlPlayer;
    private int numDots;
    public float timePowerUp;
    [System.NonSerialized]
    public bool powerUp;
    public Light lightRed;
    public CtrlMenu ctrlMenu;
    private float nowTimePowerUp;
    private float partialTime = 1f;
    private CtrlResult ctrlResult;
    public GameObject[] iconLifes;
    private int indexIcon;
    // Start is called before the first frame update
    void Start()
    {
        powerUp = false;
        numDots = groupDots.transform.childCount;
        ctrlPlayer = GetComponent<CtrlPlayer>();
        ctrlResult = GameObject.FindGameObjectWithTag("Result").GetComponent<CtrlResult>();
        closeDoor();
        indexIcon = 0;
    }
   
    void Update()
    {
        
        if (powerUp == true)
        {
            nowTimePowerUp -= Time.deltaTime;
            if(nowTimePowerUp < 3f)
            {
                partialTime += Time.deltaTime;
                if (partialTime > 0.25f)
                {
                    partialTime = 0f;
                    lightRed.enabled = !lightRed.enabled;
                }
            }
            if(nowTimePowerUp < 0)
            {
                lightRed.enabled = false;
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
        ctrlResult.result = CtrlResult.CanvasResult.WIN;
        ctrlMenu.goMainMenu();
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

    public void loseLife()
    {
        --numLifes;
        if(indexIcon < iconLifes.Length)
        {
            iconLifes[indexIcon].SetActive(false);
            ++indexIcon;
        }
        if(numLifes <= 0)
        {
            lose();
        }else
        {
            door.SetActive(false);
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i].GetComponent<BaseEnemyAgent>().respown();
            }
            ctrlPlayer.respown();
            closeDoor();
        }
    }

    private void lose()
    {
        ctrlResult.result = CtrlResult.CanvasResult.LOSE;
        ctrlMenu.goMainMenu();
    }

    private void closeDoor()
    {

        StartCoroutine(closeDoorCoroutine());
    }

    IEnumerator closeDoorCoroutine()
    {
        yield return new WaitForSeconds(timeToCloseDoor);
        door.SetActive(true);
        if(powerUp == false)
        {
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i].GetComponent<BaseEnemyAgent>().stateEnemy = BaseEnemyAgent.StateEnemy.ATTACKING;
            }
        }
    }
}
