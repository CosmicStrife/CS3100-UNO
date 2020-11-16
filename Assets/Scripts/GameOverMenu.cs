using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public bool ExitMenuIsActive = false;
    public GameObject exitMenuUI;
    private UNO UNOsystem;

    // Start is called before the first frame update
    void Start()
    {
        UNOsystem = FindObjectOfType<UNO>();
        ExitMenuIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExitMenuIsActive == true)
        {
            Pause();
        }
        else
            Resume();
    }

    void Pause()
    {
        exitMenuUI.SetActive(true);
        Time.timeScale = 0f;
        ExitMenuIsActive = true;
    }

    public void Resume()
    {
        exitMenuUI.SetActive(false);
        Time.timeScale = 1f;
        ExitMenuIsActive = false;
    }
}
