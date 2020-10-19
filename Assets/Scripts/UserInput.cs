using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    //TEMPORARY VARIABLES FOR DEMO
    public bool deckClick;
    public bool cardClick;
    public bool player1Click;
    public bool discardPileClick;
    public bool UNObuttonClick;


    private Selectable selectable;
    // Start is called before the first frame update
    void Start()
    {
        //TEMPORARY VARIABLES FOR DEMO
        deckClick = false;
        cardClick = false;
        player1Click = false;
        discardPileClick = false;
        UNObuttonClick = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY CODE; stops sending signal once not pressed anymore
        if(deckClick)
        {
            deckClick = false;
        }
        if (cardClick)
        {
            cardClick = false;
        }
        if (player1Click)
        {
            player1Click = false;
        }
        if (discardPileClick)
        {
            discardPileClick = false;
        }
        if (UNObuttonClick)
        {
            UNObuttonClick = false;
        }

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
                else if ((hit.collider.CompareTag("Card"))) // && (selectable.playerCard == false))
                {
                    // clicked card
                    
                    Card();
                }
                else if ((hit.collider.CompareTag("Card"))) // && (selectable.playerCard == true))
                {
                    // clicked top
                    
                    Player1();
                }
                else if (hit.collider.CompareTag("Discard Pile"))
                {
                    // clicked card

                    Discard_Pile();
                }
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
        deckClick = true;

    }

    void Card()
    {
        print("Clicked on Card");
        cardClick = true;
    }

    void Player1()
    {
        print("Clicked on Player1");
        player1Click = true;
    }
    
    void Discard_Pile()
    {
        print("Clicked on Discard_Pile");
        discardPileClick = true;
    }

    void UNO_Button()
    {
        print("Clicked on UNO_Button");
        UNObuttonClick = true;
    }
}
