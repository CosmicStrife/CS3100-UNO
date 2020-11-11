using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WildMenu : MonoBehaviour
{
    public bool WildMenuIsActive = false;
    public GameObject wildMenuUI;
    private UNO UNOsystem;


    // Start is called before the first frame update
    void Start()
    {
        //UNOsystem = UNOobject.GetComponent<UNO>();
        UNOsystem = FindObjectOfType<UNO>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (WildMenuIsActive)
        //    {
        //        Resume();
        //    }
        //    else 
        //    {
        //        Pause();
        //    }
        //}

        if (WildMenuIsActive)
        {
            Pause();
        }
        else
        {
            Resume();
        }

    }

    public void Resume()
    {
        wildMenuUI.SetActive(false);
        Time.timeScale = 1f;
        WildMenuIsActive = false;


    }

    void Pause()
    {
        wildMenuUI.SetActive(true);
        Time.timeScale = 0f;
        WildMenuIsActive = true;

    }

    public void BLUE_Button()
    {
        UNOsystem.curColor = 'B';
        Resume();
    }

    public void GREEN_Button()
    {
        UNOsystem.curColor = 'G';
        Resume();
    }

    public void RED_Button()
    {
        UNOsystem.curColor = 'R';
        Resume();
    }

    public void YELLOW_Button()
    {
        UNOsystem.curColor = 'Y';
        Resume();
    }
}
