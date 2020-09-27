using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPlayer : MonoBehaviour
{
    public float speed;

    private float maxDistHit = 0.5f;
    private Vector3 velocity;
    //to check if player can go straight
    private bool canStraight;
    //to check if player can turn
    private bool canLeftA, canLeftB, canUpA, canUpB;
    private bool canDownA, canDownB, canRightA, canRightB;
    private bool canTurnUp, canTurnDown, canTurnLeft, canTurnRight;
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
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
        canStraight = rayCastCheckDirectionIsEmpty(transform.position, velocity, maxDistHit);
        canUpA = rayCastCheckDirectionIsEmpty(new Vector3(0.48f, 0f, 0f) + transform.position, Vector3.forward, maxDistHit * 2);
        canUpB = rayCastCheckDirectionIsEmpty(new Vector3(-0.48f, 0f, 0f) + transform.position, Vector3.forward, maxDistHit * 2);
        canDownA = rayCastCheckDirectionIsEmpty(new Vector3(0.48f, 0f, 0f) + transform.position, Vector3.back, maxDistHit * 2);
        canDownB = rayCastCheckDirectionIsEmpty(new Vector3(-0.48f, 0f, 0f) + transform.position, Vector3.back, maxDistHit * 2);
        canLeftA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.48f) + transform.position, Vector3.left, maxDistHit * 2);
        canLeftB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.48f) + transform.position, Vector3.left, maxDistHit * 2);
        canRightA = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, 0.48f) + transform.position, Vector3.right, maxDistHit * 2);
        canRightB = rayCastCheckDirectionIsEmpty(new Vector3(0f, 0f, -0.48f) + transform.position, Vector3.right, maxDistHit * 2);
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
            Debug.DrawRay(position, direction * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Collider")
            {
                return false;
            }
            return true;
        }
        else
        {
            Debug.DrawRay(position, direction * 2, Color.blue);
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
}
