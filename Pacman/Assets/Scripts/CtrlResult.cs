using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlResult : MonoBehaviour
{
    public enum CanvasResult
    {
        MENU, WIN, LOSE
    };
    [System.NonSerialized]
    public CanvasResult result = CanvasResult.MENU;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
