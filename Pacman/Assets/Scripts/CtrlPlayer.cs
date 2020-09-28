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
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
        ctrlGame = GetComponent<CtrlGame>();
        startingPosition = gameObject.transform.position;
    }


    void Update()
    {
        checkDirections();
        updateVelocity();

        if (canStraight == true)
        {
            gameObject.transform.position += velocity * speed * Time.deltaTime;
        }        
    }

    private void checkDirections()
    {
        canStraight = rayCastCheckDirectionIsEmpty(transform.position, velocity, maxDistHitStraiht);
        canUpA = rayCastCheckDirectionIsEmpty(new Vector3(0.48f, 0f, 0f) + transform.position, Vector3.forward, maxDistHitTurn);
        canUpB = rayCastCheckDirectionIsEmpty(new Vector3(-0.48f, 0f, 0f) + transform.position, Vector3.forward, maxDistHitTurn);
        canDownA = rayCastCheckDirectionIsEmpty(new Vector3(0.48f, 0f, 0f) + transform.position, Vector3.back, maxDistHitTurn);
        canDownB = rayCastCheckDirectionIsEmpty(new Vector3(-0.48f, 0f, 0f) + transform.position, Vector3.back, maxDistHitTurn);
        canLeftA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.48f) + transform.position, Vector3.left, maxDistHitTurn);
        canLeftB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.48f) + transform.position, Vector3.left, maxDistHitTurn);
        canRightA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.48f) + transform.position, Vector3.right, maxDistHitTurn);
        canRightB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.48f) + transform.position, Vector3.right, maxDistHitTurn);
        canTurnUp = canUpA & canUpB;
        canTurnDown = canDownA & canDownB;
        canTurnLeft = canLeftA & canLeftB;
        canTurnRight = canRightA & canRightB;
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
        if (canTurnUp && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            velocity = new Vector3(0, 0, 1);
            //gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnDown && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            velocity = new Vector3(0, 0, -1);
            //gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnLeft && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            velocity = new Vector3(-1, 0, 0);
            //gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (canTurnRight && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            velocity = new Vector3(1, 0, 0);
            //gameObject.transform.LookAt(velocity.normalized + transform.position);
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

            }else
            {
                ctrlGame.loseLife();
            }
        }
    }

    public void respown()
    {
        velocity = new Vector3(0, 0, 0);
        gameObject.transform.position = startingPosition;
    }
}
