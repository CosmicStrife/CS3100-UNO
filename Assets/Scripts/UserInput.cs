using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Including for ToList()
using System.Linq;

public class UserInput : MonoBehaviour
{
    //GameObject UNOobject;
    private UNO UNOsystem;

    //GameObject turnManager;
    private turnActionManager actionManager;

    //TEMPORARY VARIABLES FOR DEMO
    public bool deckClick;
    public bool cardClick;
    //public bool player1Click;
    public bool discardPileClick;
    public bool UNObuttonClick;


    private Selectable selectable;
    // Start is called before the first frame update
    void Start()
    {
        //UNOsystem = UNOobject.GetComponent<UNO>();
        UNOsystem = FindObjectOfType<UNO>();
        //actionManager = turnManager.GetComponent<turnActionManager>;
        actionManager = FindObjectOfType<turnActionManager>();

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
                    //Card(cardName);
                    //print(hit.transform.name);
                    /*
                    string testCase = hit.transform.name;//" Wild";
                    if(valid(testCase))
                    {
                        print("  valid("+testCase+") works");
                    }
                    else
                    {
                        print("  valid("+testCase+") doesn't work");
                    }
                    */

                    //If the card is valid, play it
                    if(actionManager.playAllowed)
                    {
                        if (valid(hit.transform.name) && ((!(actionManager.cardDrawn)) || (hit.transform.name == UNOsystem.Player1[UNOsystem.Player1.Count-1])))//Connecting to turnActionManager.cs; Determining whether allowed to play and which cards to play
                        {
                            print("  >Playing "+hit.transform.name);
                            /*Place the card onto the discard pile and remove from hand*/
                            GameObject temp = GameObject.Find("UNOGame").GetComponent<UNO>().DiscardPos;
                            hit.transform.parent = temp.transform;
                            //Move card onto top of discard pile
                            hit.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z - .03f * (GameObject.Find("UNOGame").GetComponent<UNO>().Discard.Count + 1));
                            //Add to the discard pile and remove from player hand
                            GameObject.Find("UNOGame").GetComponent<UNO>().Player1.Remove(hit.transform.name);
                            GameObject.Find("UNOGame").GetComponent<UNO>().Discard.Add(hit.transform.name);
                            hit.transform.GetComponent<Selectable>().playerCard = false;

                            /* ------ the card play logic goes here ------*/
                            UNOsystem.curColor = hit.transform.name[0];
                            actionManager.playDone = true;
                            
                            print("This is valid");
                        }
                        else //otherwise, ignore the card click
                            print("Invalid, select another.");
                    }
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


    /*Finds if a card is valid*/
    public bool valid(string cardName)
    {
        List<string> discard = GameObject.Find("UNOGame").GetComponent<UNO>().Discard;
        string topCard = discard[discard.Count - 1]; //Find the top card's name

        //If the color is equal or it is a wild
        if (cardName[0] == ' ' || GameObject.Find("UNOGame").GetComponent<UNO>().curColor == cardName[0])
            return true;
        else if (topCard.Substring(1) == cardName.Substring(1)) //If the "second part" of a card is equal
            return true;
        else if (cardName[0] == ' ') //if it is a wild
            return true;
        else
            return false; //Otherwise it is unplayable
    }

    void Card(string cardName)
    {
        print("Clicked on Card ("+cardName+")");
        cardClick = true;

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


}
