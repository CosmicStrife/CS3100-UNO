using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private Selectable selectable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();

    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //clickCount++;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                // what has been hit? Deck/Card/EmptySlot...
                if (hit.collider.CompareTag("Deck"))
                {
                    //clicked deck
                    
                    Deck();
                }
                else if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == false))
                {
                    // clicked card
                    
                    Card();
                }
                else if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == true))
                {
                    // clicked top
                    string cardName = hit.transform.name;
                    Player1(cardName);
                }
                /*
                else if (hit.collider.CompareTag("Discard Pile"))
                {
                    // clicked card

                    Discard_Pile();
                }
                */
                else if (hit.collider.CompareTag("UNO Button"))
                {
                    // clicked top

                    UNO_Button();
                }
            }
        }
    }

    void Deck()
    {
        // deck click actions
        print("Clicked on deck");
        

    }

    void Card()
    {
        print("Clicked on Card");
    }

    void Player1(string cardName)
    {
        print("Clicked on Player1 (" + cardName + ")");
    }
    /*
    void Discard_Pile()
    {
        print("Clicked on Discard_Pile");
    }*/

    void UNO_Button()
    {
        print("Clicked on UNO_Button");
    }
}
