using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 portingPosition;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.gameObject.GetComponent<BaseEnemyAgent>().port(portingPosition);
        }else if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<CtrlPlayer>().port(portingPosition);
        }
    }
}
