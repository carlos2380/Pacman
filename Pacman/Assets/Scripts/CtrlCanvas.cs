using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlCanvas : MonoBehaviour
{
    private CtrlResult ctrlResult;
    public GameObject panelMenu;
    public GameObject panelWin;
    public GameObject panelLose;
    // Start is called before the first frame update
    void Start()
    {
        ctrlResult = GameObject.FindGameObjectWithTag("Result").GetComponent<CtrlResult>();
        panelMenu.SetActive(false);
        panelWin.SetActive(false);
        panelLose.SetActive(false);
        switch(ctrlResult.result)
        {
            case CtrlResult.CanvasResult.MENU:
                panelMenu.SetActive(true);
                break;
            case CtrlResult.CanvasResult.WIN:
                panelWin.SetActive(true);
                break;
            case CtrlResult.CanvasResult.LOSE:
                panelLose.SetActive(true);
                break;
             default:
                panelMenu.SetActive(true);
                break;
        }
    }

    public void backToMenu()
    {
        panelMenu.SetActive(true);
        panelWin.SetActive(false);
        panelLose.SetActive(false);
    }
}
