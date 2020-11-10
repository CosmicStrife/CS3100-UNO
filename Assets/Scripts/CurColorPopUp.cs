using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CurColorPopUp : MonoBehaviour
{
    public GameObject signalBlue;
    private bool showBluePop = false, showGreenPop = false, showRedPop = false, showYellowPop = false;
    
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 5.0f;

    //void OnWildPlayed()
    //{
       
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(displayColorPicked());
            

        }
        
    }

    IEnumerator displayColorPicked()
    {
        
        signalBlue.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(3);        
        signalBlue.GetComponent<SpriteRenderer>().enabled = false;
    }
}


//executedTime = Time.time;

//currentTime = Time.time;
//if (executedTime != 0.0f)
//{
//    if (currentTime - executedTime > timeToWait)
//    {
//        executedTime = 0.0f;
//        //someRandomCondition = false;
//        StopAllCoroutines();
//        this.GetComponent<SpriteRenderer>().enabled = false;
//    }
//}



//currentTime = Time.time;
//if (someRandomCondition)
//    showText = true;
//else
//    showText = false;

//if (executedTime !== 0.0f)
//{
//    if (currentTime - executedTime > timeToWait)
//    {
//        executedTime = 0.0f;
//        someRandomCondition = false;
//    }
//}