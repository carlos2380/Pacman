using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPlayer : MonoBehaviour
{
    public float speed;

    private float maxDistHit = 0.5f;
    private Vector3 velocity;


    void Start()
    {
        velocity = new Vector3(0, 0, 0);
    }


    void Update()
    {
        updateVelocity();
        RaycastHit hit;
        bool stopForwad = false;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistHit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if( hit.collider.gameObject.tag == "Collider")
            {
                stopForwad = true;
            }
            //Debug.Log("Did Hit");
        }
       
        if(stopForwad == false)
        {
            // Debug.Log("Did not Hit");
            gameObject.transform.LookAt(velocity.normalized + transform.position);
            gameObject.transform.position += velocity * speed * Time.deltaTime;
        }        
    }

    void updateVelocity()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            velocity = new Vector3(0, 0, 1);
            gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            velocity = new Vector3(0, 0, -1);
            gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            velocity = new Vector3(-1, 0, 0);
            gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            velocity = new Vector3(1, 0, 0);
            gameObject.transform.LookAt(velocity.normalized + transform.position);
        }
    }
}
