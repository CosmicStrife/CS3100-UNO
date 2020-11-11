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
        //THIS BLOCK IS USED TO TEST THE WILD MENU
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

        // When a wild card is played (Code from turnActionManager.cs)
        // the  WildMenuIsActive bool is set to true
        if (WildMenuIsActive)
        {
            Pause();
        }
        else
        {
            Resume();
        }

    }

    // Resume exits the Wild Menu
    public void Resume()
    {
        wildMenuUI.SetActive(false);
        Time.timeScale = 1f;
        WildMenuIsActive = false;
    }

    // Pause activates the Wild Menu
    void Pause()
    {
        wildMenuUI.SetActive(true);
        Time.timeScale = 0f;
        WildMenuIsActive = true;

    }

    // Code for pick blue color button
    public void BLUE_Button()
    {
        UNOsystem.curColor = 'B';
        Resume();
    }

    // Code for pick green color button
    public void GREEN_Button()
    {
        UNOsystem.curColor = 'G';
        Resume();
    }
    
    // Code for pick red color button
    public void RED_Button()
    {
        UNOsystem.curColor = 'R';
        Resume();
    }

    // Code for pick yellow color button
    public void YELLOW_Button()
    {
        UNOsystem.curColor = 'Y';
        Resume();
    }
}
