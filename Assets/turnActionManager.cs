using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class turnActionManager : MonoBehaviour
{
    /*---Variables---*/
    //Storees current player
    string currPlayer;

    //Stores current phase of turn
    //  +Start: 0
    //  +Draw/Play: 1
    //  +End: 2
    public byte phase;

    //Keeps track of if UNO is called at an appropriate time
    public bool UNOcalled;

    //Keeps track of if a card was drawn this turn outside of special rules
    public bool cardDrawn;

    private UNO UNOsystem;
    private UserInput input;
    private turnOrderManager orderManager;

    /*----------------------------------------------------------------------------------------------------------------------*/

    //Check turnOrderManager for signal for new player
    //  If new player, send signal that change has been recieved
    string getCurrPlayer()
    {
        //return orderManager.turnOrder[0];
        //return orderManager.turnOrder.First();
        //print(orderManager.turnOrder[0]);
        return orderManager.turnOrder[0];
        //return "temp";
    }

    void turnDraw(string player, byte numCards)
    {
        int i=0;
        while(i<numCards)
        {
            //
            if(input.deckClick)
            {
                print("Card drawn by" + player);
                i++;
            }
        }
        return;
    }

    /*----------------------------------------------------------------------------------------------------------------------*/

    // Start is called before the first frame update
    void Start()
    {
        //Connect to turnOrderManager
        UNOsystem = FindObjectOfType<UNO>();
        input = FindObjectOfType<UserInput>();
        //orderManager = Gameobject.Find("turnManager").GetComponent("turnOrderManager");
        //orderManager = GetComponent("turnOrderManager");
        //orderManager = FindObjectOfType<turnOrderManager>();
        orderManager = GetComponent<turnOrderManager>();

        currPlayer = getCurrPlayer();
        phase = 0;
        UNOcalled = false;
        cardDrawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for new player
        if(currPlayer != getCurrPlayer())
        {
            currPlayer = getCurrPlayer();
            phase = 0;

            print("Turn Change: "+currPlayer);
            print("  "+currPlayer+" start phase");
        }


        //Get data from player
        //  +Get data from userInput

        //UNO checker
        //  +If (multiple cards in hand) AND (UNOcalled)
        //      -Set UNOcalled = false
        //  +If one card left:
        //      -Allow UNO to be called
        //      -If UNO called
        //          +Check for signal from User Interface
        //          +Set UNOcalled = true
        //
        

        //NOTE: will probably need to be replaced. Can't track UNO calls between players
        if (/*hand.numCards(currplayer) > 1*/true)//Change once hands are made
        {
            UNOcalled = false;
        }
        else if(/*hand.numCards == 1*/true)
        {
            if(input.UNObuttonClick)
            {
                print("UNO!");//Temporary; change to display an image and noise
                UNOcalled = true;
            }
        }


        //TEMPORARY CODE; meant to substitute for AI action for now
        if(currPlayer == "CPU1")
        {
            print("  Skipping CPU turn");
            phase = 2;
        }


        //Once per turn:
        //  Get currPlayer from turnOrderManager
        //  Maybe just have turnOrderManager change according to phase stored here
        
        //If (matching phase) AND (player action detected):
        //  Special Actions
        //      +Use rules list tpo detect
        //      +Send signals to UI to change display and interfaceswitch (phase)
        switch (phase)
        {
            case 0:
            //  Start
            //      +Use cardManager to change data
            //      +Send signals to UI to change display and interface
            //      +Process special actions
                //Special Actions
                
                if (orderManager.rules.Contains("s"))
                {
                    phase = 3;
                }
                else
                {
                    foreach (string rule in orderManager.rules)
                    {
                        if(rule[0] == 'd')
                        {
                            //Draw
                            turnDraw(currPlayer, (byte)Char.GetNumericValue(rule[1]));
                        }
                        if(rule[0] == 'r')
                        {
                            orderManager.turnDirection *= (-1);
                        }
                    }
                    //Insert other checks here

                    phase++;
                    print("  "+currPlayer+" draw/play phase");
                    
                }
                break;
            case 1:
            //  Draw/Play
            //      +Check for matching cards in hand.
            //          -If none, require draw.
                
                    
                    if (input.deckClick)
                    {
                        //Move card from deck to hand

                        cardDrawn = true;
                    }

                    //Not drawing a card
                    if (!cardDrawn && input.cardClick)
                    {
                        //Attempting to play a card
                        if(true/*Card legal to play*/)
                        {
                            phase++;
                            print("  "+currPlayer+" end phase");
                        }
                        else
                        {
                            print("Illegal card");
                        }
                    }
                    //Drawing a card
                    else if(cardDrawn)
                    {
                        //Check if card that was drawn is playable.
                        //  +If so, play card. (?)
                        //  +If not, end turn.

                        //Placeholder phase change
                        phase++;
                        print("  "+currPlayer+" end phase");
                    }
                break;
                /*
            case 2:
            //  Play
            //      +Use cardManager to change data
            //      +Use cardManager to check if legal move
            //      +Send signals to UI to change display and interface
                if(false)//No cards in hand legal to play
                {
                    phase = 0;
                }
                else if (input.cardClick)
                {
                    //Get card info from UserInput.cs
                    //selectedCard = UserInput.getCard();//?

                    if (true)//Card legal to be played
                    {
                        //Apply special rules (Wild cark check, discard and played card)
                        //Move card from hand to discard pile
                        
                        phase++;
                        print("  "+currPlayer+" end phase");
                    }
                    else
                    {
                        print("Illegal card");
                    }
                }
                break;
                */

            case 2:
            //  End
            //      +Send signal to turnOrderManager
            //      +Reset tracking variables
                cardDrawn = false;
                break;

            default:
                break;
        }
    }
}
