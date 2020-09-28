using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGame : MonoBehaviour
{
    public GameObject groupDots;
    private int numDots;
    // Start is called before the first frame update
    void Start()
    {
        numDots = groupDots.transform.childCount;
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
}
