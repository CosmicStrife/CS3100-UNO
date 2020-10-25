using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Including for ToList()
using System.Linq;

public class UserInput : MonoBehaviour
{
    //TEMPORARY VARIABLES FOR DEMO
    public bool deckClick;
    public bool cardClick;
    //public bool player1Click;
    public bool discardPileClick;
    public bool UNObuttonClick;

    List<string> cardData;
    public string currColor;
    public string currSymbol;
    List<string> currRules;


    private Selectable selectable;
    // Start is called before the first frame update
    void Start()
    {
        //TEMPORARY VARIABLES FOR DEMO
        deckClick = false;
        cardClick = false;
        //player1Click = false;
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
        /*
        if (player1Click)
        {
            player1Click = false;
        }
        */
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
                else if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == false))
                {
                    //Shouldn't detect anything if clicked on another player's card
                    
                    //Card();
                }
                else if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == true))
                {
                    // clicked top
                    string cardName = hit.transform.name;
                    //Player1(cardName);
                    Card(cardName);
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
                    // clicked UNO button this frame

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

        //May need to add code to store data of drawn card
    }

    void Card(string cardName)
    {
        print("Clicked on Card ("+cardName+")");
        cardClick = true;

        //Adding card name analysis
        cardName_Analysis(cardName);
    }

    /*
    void Player1(string cardName)
    {
        print("Clicked on Player1 (" + cardName + ")");
        player1Click = true;
    }
    */
    /*
    void Discard_Pile()
    {
        print("Clicked on Discard_Pile");
        discardPileClick = true;
    }
    */

    void UNO_Button()
    {
        print("Clicked on UNO_Button");
        UNObuttonClick = true;
    }

    void cardName_Analysis(string cardName)
    {
        print("cardName_Analysis("+cardName+")");
        print("\""+cardName+"\"");

        //Preparing data storage
        cardData.Clear();
        currRules.Clear();

        string temp = cardName;
        

        //Processing card name
        //string temp[] = cardName.Split('_');
        cardData = (temp.Split(' ')).ToList();
        /*
        foreach (string data in cardName.Split('_'))
        {
            print("  "+data);
            cardData.Add(data);
        }
        */
        
        //Color detection
        currColor = cardData[0];

        //Symbol Detection
        //  NOTE: Acting as though "Draw" is a symbol; ignoring quantity of drawn cards.
        //          +Shouldn't matter; only draw-2s and draw-4s, and the draw-4s are all wild.
        if(cardData.Count>1)
        {
            currSymbol = cardData[1];
        }
        else
        {
            currSymbol = "Wild";
        }

        //Rule Detection
        switch (currSymbol)
        {
            case "Skip":
                currRules.Add("s");
                break;
            case "Draw":
                switch (cardData[2])
                {
                    case "2":
                        currRules.Add("d2");
                        break;
                    case "4":
                        currRules.Add("d4");
                        break;
                    case "Two":
                        currRules.Add("d2");
                        break;
                    case "Four":
                        currRules.Add("d4");
                        break;
                    default:
                        break;
                }
                break;
            case "Reverse":
                currRules.Add("r");
                break;
            default:
                break;
        }

        print("Card data:");
        print("  "+currColor);
        print("  "+currSymbol);

        return;
    }
    
}
