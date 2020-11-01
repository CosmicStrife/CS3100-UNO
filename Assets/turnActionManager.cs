using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class turnActionManager : MonoBehaviour
{
    /*---Variables---*/
    //Storees current player
    string currPlayer;

    //Stores special rules to process
    //  +Reverse ("r")
    //  +Draw ("d2", "d4")
    //  +Skip
    public List<string> rules;

    //Stores current phase of turn
    //  +Start: 0
    //  +Draw/Play: 1
    //  +End: 2
    public byte phase;

    //Keeps track of if UNO is called at an appropriate time
    public bool UNOcalled;

    //Keeps track of if a card was drawn this turn outside of special rules
    public bool cardDrawn;


    //AI Variables; keeps track of what an AI is doing
    public bool AIdraw;
    public bool AIplay;
    public string AIselectedCard;


    //Stores what actions are allowed; used in UserInput.cs
    public bool playAllowed;
    public bool playDone;


    //GameObject UNOobject;
    private UNO UNOsystem;

    private UserInput input;
    private turnOrderManager orderManager;

    //Input stuff
    Vector3 mousePosition;// = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
    RaycastHit2D hit;// = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    // Game objects for cardPrefab, CPU and Player1 hand place holders 
    //public GameObject cardPrefab;
    //public GameObject CPUPos;
    //public GameObject Player1Pos;
    //public GameObject DiscardPos;


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
        if(player == "Player1")
        {
            int i=0;
            while(i<numCards)
            {
                //
                if(input.deckClick)
                {
                    print("Card drawn by" + player);
                    UNOsystem.turnDraw(1, UNOsystem.Player1);
                    input.refresh_hand_display();
                    i++;
                }
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

        playAllowed = false;
        playDone = false;

        AIdraw = false;
        AIplay = false;
        AIselectedCard = "NULL";

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
        //  +Get data from code taken from userInput    
        if (Input.GetMouseButtonDown(0))
        {
            //clickCount++;

            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }

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
                print("UNO! called");//Temporary; change to display an image and noise
                UNOcalled = true;
            }
        }


        //Once per turn:
        //  Get currPlayer from turnOrderManager
        //  Maybe just have turnOrderManager change according to phase stored here
        
        //If (matching phase) AND (player action detected):
        //  Special Actions
        //      +Use rules list tpo detect
        //      +Send signals to UI to change display and interfaceswitch (phase)
        //  Playing cards:
        //      +Phase->determining whether or not allowed to play
        //      +Use variables to allow or block playing cards
        switch (phase)
        {
            case 0:
            //  Start
            //      +Use cardManager to change data
            //      +Send signals to UI to change display and interface
            //      +Process special actions
            //      +Reset tracking variables

                //Display for testing
                if(currPlayer == "Player1")
                {
                    consoleDisplay(UNOsystem.Player1);
                }
                else
                    consoleDisplay(UNOsystem.CPU1);


                //Reseting tracking variables
                cardDrawn = false;

                //Special Actions
                if (rules.Contains("s"))
                {
                    phase = 2;
                    rules.Remove("s");
                }
                else
                {
                    foreach (string rule in rules)
                    {
                        if(rule[0] == 'd')
                        {
                            //Draw
                            turnDraw(currPlayer, (byte)Char.GetNumericValue(rule[1]));
                            //input.refresh_hand_display();
                        }
                        if(rule[0] == 's' || rule[0] == 'r')
                        {
                            phase = 2;
                        }
                    }
                    rules.Clear();
                    //Insert other checks here

                    if(phase != 2)
                    {
                        phase++;
                        print("  "+currPlayer+" draw/play phase");
                    }
                }
                break;
            case 1:
            //  Draw/Play
            //      +Check for matching cards in hand.
            //          -If none, require draw.

                if(!playDone)
                {
                    playAllowed = true;

                    if(currPlayer == "Player1")
                    {
                        if (input.deckClick && !cardDrawn)
                        {
                            cardDrawn = true;
                            
                            //Move card from deck to hand
                            UNOsystem.turnDraw(1, UNOsystem.Player1);

                            //Refresh/rebuild hand display
                            input.refresh_hand_display();

                            print("  Drew: "+UNOsystem.Player1[UNOsystem.Player1.Count-1]);
                        
                            if(!input.valid(UNOsystem.Player1[UNOsystem.Player1.Count-1]))
                            {
                                print("  No possible moves");
                                phase = 2;
                            }
                        }
                    }
                    else if(currPlayer == "CPU1")
                    {
                        print("  CPU turn; skipping until an AI is made.");

                        //NON-FUNCTIONAL; based off code that relies on UserInput for changing the game.
                        if (AIdraw && !cardDrawn)
                        {
                            cardDrawn = true;
                            
                            //Move card from deck to hand
                            UNOsystem.turnDraw(1, UNOsystem.CPU1);

                            //Refresh/rebuild hand display
                            //  +Remove current sprites
                            //  +Make new sprites
                            input.refresh_hand_display();

                            print("  Drew: "+UNOsystem.CPU1[UNOsystem.CPU1.Count-1]);
                        
                            if(!input.valid(UNOsystem.CPU1[UNOsystem.CPU1.Count-1]))
                            {
                                print("  No possible moves");
                                phase = 2;
                            }
                        }
                        
                        /*
                        //NON-FUNCTIONAL; based off code that relies on UserInput for changing the game.
                        if (input.valid(AIselectedCard) && ((!(cardDrawn)) || (AIselectedCard == UNOsystem.CPU1[UNOsystem.CPU1.Count-1])))//Connecting to turnActionManager.cs; Determining whether allowed to play and which cards to play
                        {
                            print("  >Playing "+hit.transform.name);
                            //Place the card onto the discard pile and remove from hand
                            GameObject temp = GameObject.Find("UNOGame").GetComponent<UNO>().DiscardPos;
                            //
                            hit.transform.parent = temp.transform;
                            //Move card onto top of discard pile
                            hit.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z - .03f * (GameObject.Find("UNOGame").GetComponent<UNO>().Discard.Count + 1));
                            //Add to the discard pile and remove from player hand
                            GameObject.Find("UNOGame").GetComponent<UNO>().Player1.Remove(hit.transform.name);
                            GameObject.Find("UNOGame").GetComponent<UNO>().Discard.Add(hit.transform.name);
                            hit.transform.GetComponent<Selectable>().playerCard = false;
                            //

                            // ------ the card play logic goes here ------
                            //UNOsystem.curColor = hit.transform.name[0];
                            //playDone = true;
                            phase = 2;

                            print("This is valid");
                        }
                        */
                        else //otherwise, ignore the card click
                            print("Invalid, select another.");

                        //TEMPORARY
                        phase = 2;
                    }
                }
                else
                {
                    print("  Finishing turn");
                    phase = 2;
                }
                    /*
                    //Not drawing a card
                    if (!cardDrawn && input.cardClick)
                    {
                        //Attempting to play a card
                        if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == true))
                        {
                            if(input.valid(hit.transform.name))
                            {
                                //play(hit.transform.name, UNOsystem.Player1);

                                phase++;
                                print("  "+currPlayer+" end phase");
                            }
                            else
                            {
                                print("Illegal card");
                            }
                        }
                    }
                    //Drawing a card
                    else if(cardDrawn)
                    {
                        //Check if card that was drawn is playable.
                        //  +If so, play card. (?)
                        //  +If not, end turn.
                        
                        if ((hit.collider.CompareTag("Card")) && (hit.transform.GetComponent<Selectable>().playerCard == true))
                        {
                            if(input.valid(UNOsystem.Player1[UNOsystem.Player1.Count-1]))
                            {
                                if(hit.transform.name == UNOsystem.Player1[UNOsystem.Player1.Count-1])
                                {
                                    //play(hit.transform.name, UNOsystem.Player1);

                                    print("  "+currPlayer+" plays "+hit.transform.name);
                                
                                    phase++;
                                    print("  "+currPlayer+" end phase");
                                }
                            }
                            else
                            {
                                print("  "+UNOsystem.Player1[UNOsystem.Player1.Count-1]+" is not legal to play");
                                
                                phase++;
                                print("  "+currPlayer+" end phase");
                            }
                            
                        }
                    }
                    */
                    /*
                    else
                    {
                        print("No legal moves for "+currPlayer);
                        phase++;
                        print("  "+currPlayer+" end phase");
                    }
                    */
                
                break;

            case 2:
            //  End
            //      +Send signal to turnOrderManager
                print("END PHASE");

                playAllowed = false;
                playDone = false;

                cardDrawn = false;

                AIdraw = false;
                AIplay = false;
                AIselectedCard = "NULL";

                phase++;//Enable turn change
                break;

            default:
                break;
        }
    }


    
    //void play(RaycastHit2D hit, List<string> hand)

    //Move card from hand to discard pile; update display
    void getRules(string cardName)
    {
        //Store special rules of the played card, if any
        switch (cardName[cardName.Length-1])
        {
            case 'o'://Draw two
                rules.Add("d2");
                break;
            case 'p'://Skip
                rules.Add("s");
                break;
            case 'e'://Reverse
                //orderManager.turnDirection *= (-1);
                rules.Add("s");//Only two players
                break;
            /*
            case 'd'://Wild
                rules.Add("w");
                break;
            */
            case 'r'://Wild draw four
                //Wild card rules handled in input.valid()
                rules.Add("d4");
                break;        
            default:
                break;
        }
/*
        //Alter hand data

        //Alter discard pile

        //Store special rules of the played card, if any
        switch (cardName[cardName.Length-1])
        {
            case 'o'://Draw two
                rules.Add("d2");
                break;
            case 'p'://Skip
                rules.Add("s");
                break;
            case 'e'://Reverse
                //orderManager.turnDirection *= (-1);
                rules.Add("s");//Only two players
                break;
            
            //case 'd'://Wild
            //    rules.Add("w");
            //    break;
            case 'r'://Wild draw four
                //Wild card rules handled in input.valid()
                rules.Add("d4");
                break;        
            default:
                break;
        }

        //Rebuild/refresh hand display
*/
        return;
    }


    void consoleDisplay(List<string> hand)
    {
        print(">Cards in hand:");
        foreach (string card in hand)
        {
            print("  +"+card);
        }
        return;
    }    
}
