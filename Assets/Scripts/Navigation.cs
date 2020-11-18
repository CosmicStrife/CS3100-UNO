using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    //[System.Obsolete]


    public static bool easyAI;
    
    public void MENU_ACTION_GotoPage_EASY(string sceneName)
    {
        easyAI = true;
        Application.LoadLevel(sceneName);
        print("Easy AI is set");
    }

    public void MENU_ACTION_GotoPage_HARD(string sceneName)
    {
        easyAI = false;
        Application.LoadLevel(sceneName);
        print("Hard AI is set");
    }

    public bool getAI()
    {
        return easyAI;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
