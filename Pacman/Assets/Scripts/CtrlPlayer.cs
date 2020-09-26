using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPlayer : MonoBehaviour
{
    public float speed;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(1, 0, 0);
        gameObject.transform.LookAt(velocity.normalized);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float maxDistHit = 0.5f;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistHit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxDistHit, Color.white);
            Debug.Log("Did not Hit");
            gameObject.transform.LookAt(velocity.normalized + transform.position);
            gameObject.transform.position += velocity * speed * Time.deltaTime;
        }
        
    }


}
