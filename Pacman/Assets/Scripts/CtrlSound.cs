using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlSound : MonoBehaviour
{
    public AudioSource track;
    public AudioSource dead;
    public AudioSource escape;
    public AudioSource eat;
    public AudioSource eatGhost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playEat()
    {
        eat.Play();
    }
    public void playEatGhost()
    {
        eatGhost.Play();
    }
    public void playDead()
    {
        dead.Play();
    }
    public void playEascape()
    {
        escape.Play();
    }

    public void restartTrack()
    {
        track.Stop();
        track.Play();
    }
}
