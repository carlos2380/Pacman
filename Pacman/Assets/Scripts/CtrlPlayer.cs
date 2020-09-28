using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPlayer : MonoBehaviour
{
    public float speed;

    private float maxDistHitStraiht = 0.5f;
    private float maxDistHitTurn = 1f;
    private Vector3 velocity;
    //to check if player can go straight
    private bool canStraight;
    //to check if player can turn
    private bool canLeftA, canLeftB, canUpA, canUpB;
    private bool canDownA, canDownB, canRightA, canRightB;
    private bool canTurnUp, canTurnDown, canTurnLeft, canTurnRight;

    private CtrlGame ctrlGame;
    private Vector3 startingPosition;
    //touchscreen
    private Vector2 startPosTouch;
    private Vector2 directionTouch;
    private bool movmentTouch;
    private bool goUp, goDown, goLeft, goRight;
    private GameObject modelPacman;
    private Animator anim;
    void Start()
    {
        velocity = new Vector3(0, 0, 1);
        ctrlGame = GetComponent<CtrlGame>();
        startingPosition = gameObject.transform.position;
        goUp = goDown = goLeft = goRight = false;
        for(int i = 0; i < gameObject.transform.childCount; ++i)
        {
            if(gameObject.transform.GetChild(i).tag == "ModelPacman")
            {
                modelPacman = gameObject.transform.GetChild(i).gameObject;
                anim = gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Animator>();
            }
        }
        anim.SetBool("moving", false);
    }


    void Update()
    {
        checkDirections();
        updateTouchScreen();
        updateVelocity();

        if (canStraight == true)
        {
            gameObject.transform.position += velocity * speed * Time.deltaTime;
            anim.SetBool("moving", true);
        }else
        {
            anim.SetBool("moving", false);
        }        
    }

    private void checkDirections()
    {
        canStraight = rayCastCheckDirectionIsEmpty(transform.position, velocity, maxDistHitStraiht);
        canUpA = rayCastCheckDirectionIsEmpty(new Vector3(0.45f, 0f, 0f) + transform.position, Vector3.forward, maxDistHitTurn);
        canUpB = rayCastCheckDirectionIsEmpty(new Vector3(-0.45f, 0f, 0f) + transform.position, Vector3.forward, maxDistHitTurn);
        canDownA = rayCastCheckDirectionIsEmpty(new Vector3(0.45f, 0f, 0f) + transform.position, Vector3.back, maxDistHitTurn);
        canDownB = rayCastCheckDirectionIsEmpty(new Vector3(-0.45f, 0f, 0f) + transform.position, Vector3.back, maxDistHitTurn);
        canLeftA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.45f) + transform.position, Vector3.left, maxDistHitTurn);
        canLeftB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.45f) + transform.position, Vector3.left, maxDistHitTurn);
        canRightA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.45f) + transform.position, Vector3.right, maxDistHitTurn);
        canRightB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.45f) + transform.position, Vector3.right, maxDistHitTurn);
        canTurnUp = canUpA & canUpB;
        canTurnDown = canDownA & canDownB;
        canTurnLeft = canLeftA & canLeftB;
        canTurnRight = canRightA & canRightB;
    }

    private void updateTouchScreen()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPosTouch = touch.position;
                    movmentTouch = false;
                    break;
                case TouchPhase.Moved:
                    directionTouch = touch.position - startPosTouch;
                    break;

                case TouchPhase.Ended:
                    movmentTouch = true;
                    break;
            }
        }
        if (movmentTouch)
        {
            movmentTouch = false;
            goUp = goDown = goLeft = goRight = false;
            if (Mathf.Abs(directionTouch.x) > Mathf.Abs(directionTouch.y))
            {
                if(directionTouch.x > 0)
                {
                    goRight = true;
                }
                else
                {
                    goLeft = true;
                }
            }
            else
            {
                if (directionTouch.y > 0)
                {
                    goUp = true;
                }
                else
                {
                    goDown = true;
                }
            }
        }
    }

    private bool rayCastCheckDirectionIsEmpty(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, distance))
        {
            Debug.DrawRay(position, direction * hit.distance, Color.red);
            if (hit.collider.gameObject.tag == "Collider")
            {
                return false;
            }
            return true;
        }
        else
        {
            Debug.DrawRay(position, direction * 2, Color.green);
            // Debug.Log("Did not Hit");
            return true;
        }
    }

    void updateVelocity()
    {
        if (canTurnUp && (goUp || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            velocity = new Vector3(0, 0, 1);
            modelPacman.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnDown && (goDown || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            velocity = new Vector3(0, 0, -1);
            modelPacman.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnLeft && (goLeft || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            velocity = new Vector3(-1, 0, 0);
            modelPacman.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnRight && (goRight || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            velocity = new Vector3(1, 0, 0);
            modelPacman.transform.LookAt(velocity.normalized + transform.position);
        }
    }

    public void port(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Pacdot")
        {
            ctrlGame.dotEated();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Power")
        {
            ctrlGame.setEnemiesToEscape();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if(ctrlGame.powerUp == true)
            {
                other.gameObject.GetComponent<BaseEnemyAgent>().stateEnemy = BaseEnemyAgent.StateEnemy.DEADING;
            }
            else
            {
                ctrlGame.loseLife();
            }
        }
    }

    public void respown()
    {
        velocity = new Vector3(0, 0, 1);
        anim.SetBool("moving", false);
        gameObject.transform.position = startingPosition;
    }
}
