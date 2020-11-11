using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class turnActionManager : MonoBehaviour
{
    public GameObject signalBlue;
    public GameObject signalGreen;
    public GameObject signalRed;
    public GameObject signalYellow;

    /*---Variables---*/
    //Storees current player
    public string currPlayer;

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

    //GameObject WildMEnu;
    private WildMenu WMsystem;

    private UserInput input;
    private turnOrderManager orderManager;

    //public DumbAI DumbAI;

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
    public string getCurrPlayer()
    {
        //return orderManager.turnOrder[0];
        //return orderManager.turnOrder.First();
        //print(orderManager.turnOrder[0]);
        return orderManager.turnOrder[0];
        //return "temp";
    }

    void turnDrawAM(string player, byte numCards)
    {
        if(player == "Player1")
        {
            int i=0;
            while(i<numCards)
            {
                //For some reason it does not work within the if statement
                //if(input.deckClick)
                //{
                    print("Card drawn by " + player);
                    UNOsystem.turnDraw(1, UNOsystem.Player1);//
                    input.refresh_hand_display(UNOsystem.Player1Pos);
                    i++;
                //}
            }
        }
        else
        {
            print(numCards+" cards drawn by " + player);
            UNOsystem.turnDraw(numCards, UNOsystem.CPU1);//
            input.refresh_hand_display(UNOsystem.CPUPos);
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

        //Connect to Wild Menu
        WMsystem = FindObjectOfType<WildMenu>();


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


                
                /*
                if(currPlayer == "Player1")
                {
                    consoleDisplay(UNOsystem.Player1, "Player1");
                }
                else
                    consoleDisplay(UNOsystem.CPU1, "CPU1");
                */

                //Special Actions
                consoleDisplay(rules, "current rules");
                if (rules.Contains("s") || rules.Contains("r"))
                {
                    print("  Skipping");
                    phase = 2;
                    rules.Remove("s");
                    rules.Remove("r");
                }
                else
                {
                    foreach (string rule in rules)
                    {
                        if(rule[0] == 'd')
                        {
                            print("  Drawing "+rule[1]);
                            //Draw
                            turnDrawAM(currPlayer, (byte)Char.GetNumericValue(rule[1]));
                            //input.refresh_hand_display();
                        }
                    }
                    //Insert other checks here

                    rules.Clear();
                }

                if(phase != 2)
                {
                    phase++;
                    print("  "+currPlayer+" draw/play phase");
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
                            input.refresh_hand_display(UNOsystem.Player1Pos);

                            print("  Drew: "+UNOsystem.Player1[UNOsystem.Player1.Count-1]);
                        
                            if(!input.valid(UNOsystem.Player1[UNOsystem.Player1.Count-1]))
                            {
                                print("  No possible moves");
                                phase = 2;
                            }
                        }
                    }
                    else if(currPlayer == "CPU1" && !WMsystem.WildMenuIsActive)
                    {

                        //print("  CPU turn; skipping until an AI is made.");


                        StartCoroutine(delay());  //NEEDS WORK: Attempt to delay CPU
                        dumbAILogic();
                        input.refresh_hand_display(UNOsystem.CPUPos);


                        /*
                        //NON-FUNCTIONAL; based off code that relies on UserInput for changing the game.
                        if (AIdraw && !cardDrawn)
                        {
                            cardDrawn = true;
                            
                            //Move card from deck to hand
                            UNOsystem.turnDraw(1, UNOsystem.CPU1);

                            //Refresh/rebuild hand display
                            //  +Remove current sprites
                            //  +Make new sprites
                            input.refresh_hand_display(UNOsystem.CPUPos);

                            print("  Drew: "+UNOsystem.CPU1[UNOsystem.CPU1.Count-1]);

                            if(input.valid(UNOsystem.CPU1[UNOsystem.CPU1.Count-1]))
                            {
                                //AI_play(AIhand, cardName==cardDrawn);
                                UNOsystem.playFromHand(UNOsystem.CPUPos, UNOsystem.CPU1, UNOsystem.CPU1[UNOsystem.CPU1.Count-1]);
                                phase = 2;
                            }
                            else
                            {
                                print("  No possible moves");
                                phase = 2;
                            }
                        }

                        //When an AI plays a card:
                        //  1. Sends name of card played
                        //      >AIselectedCard = card played
                        //  2. Sets AIPlay to true to enable playing the card
                        if(AIplay && !cardDrawn)
                        {
                            //Make sure to select card before enabling AIplay
                            //AI_play(AIhand, AISelectedCard);
                            UNOsystem.playFromHand(UNOsystem.CPUPos, UNOsystem.CPU1, AIselectedCard);
                            phase = 2;
                        }

                        */

                        /*-----TEMPORARY: Skips AI's turn-----*/
                        phase = 2;
                    }
                }
                else
                {
                    print("  Finishing turn");
                    phase = 2;
                }
                
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
    public void getRules(string cardName)
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
           
            case 'd'://Wild
                rules.Add("w");
                if (currPlayer == "Player1")
                    WMsystem.WildMenuIsActive = true;

                break;
           
            case 'r'://Wild draw four
                //Wild card rules handled in input.valid()
                rules.Add("d4");

                if (currPlayer == "Player1")
                    WMsystem.WildMenuIsActive = true;
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


    void consoleDisplay(List<string> hand, string varName)
    {
        print(">Contents of "+varName+":");
        foreach (string card in hand)
        {
            print("  +"+card);
        }
        return;
    }    

    void dumbAILogic()
    {
        List<string> CPUhand = UNOsystem.CPU1;
        bool valid = true;

        print("CPUHAND IN DUMBAILOGIC");
        foreach (string card in CPUhand)
        {
            print("checking if " + card + " is valid");
            print(CPUhand.Count);
            valid = input.valid(card);
            if(valid)
            {
                //print("Made it in if statement");
                AIplay = true;
                AIselectedCard = card;
                AIPlay(card);
                if(card[0] == ' ')
                {
                    UNOsystem.curColor = 'Y';
                    StartCoroutine(colorYellow());

                }
                break;
            }
        }
        if(valid == false)
        {
            UNOsystem.turnDraw(1, UNOsystem.CPU1);
        }
        print("valid is ");
        print(valid);
    }

    void AIPlay(string card)
    {
        print("CPU >Playing "+card);
        /*Place the card onto the discard pile and remove from hand*/
        GameObject discardpile = UNOsystem.DiscardPos;
        string path = "/CPU1/CPU1/" + card;
        GameObject cardInHand = GameObject.Find(path);
        cardInHand.transform.parent = discardpile.transform;
        cardInHand.transform.GetComponent<Selectable>().faceUp = true;
        cardInHand.transform.GetComponent<Selectable>().playerCard = false;
        print("test" + cardInHand.transform.name);

        //Move card onto top of discard pile
        cardInHand.transform.position = new Vector3(discardpile.transform.position.x, discardpile.transform.position.y, discardpile.transform.position.z - .03f * (UNOsystem.Discard.Count + 1));
        input.allAudio[1].Play();
        //Add to the discard pile and remove from player hand
        UNOsystem.CPU1.Remove(card);
        UNOsystem.Discard.Add(card);

        getRules(card);

        input.refresh_hand_display(UNOsystem.CPUPos);
        UNOsystem.curColor=card[0]; //Might need to be changed for the smart AI
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(10);
    }

    IEnumerator colorBlue()
    {
        signalBlue.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(3);
        signalBlue.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator colorGreen()
    {
        signalGreen.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(3);
        signalGreen.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator colorRed()
    {
        signalRed.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(3);
        signalRed.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator colorYellow()
    {
        signalYellow.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(3);
        signalYellow.GetComponent<SpriteRenderer>().enabled = false;
    }
}
