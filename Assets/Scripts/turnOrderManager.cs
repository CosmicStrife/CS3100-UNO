using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOrderManager : MonoBehaviour
{
    /*---Variables---*/    
    //Get player data
    List<string> players;
    public List<string> turnOrder;
        
    //Stores turn direction
    //  +Clockwise = 1
    //  +Counterclockwise = -1
    public sbyte turnDirection;

    //Used for accessing and moving players in turnOrder
    string storedPlayer;

    private UNO UNOsystem;
    private turnActionManager actionManager;

    /*----------------------------------------------------------------------------------------------------------------------*/

    //Get number of players from menu
    //  Total players
    //  # of human players
    //  # of AI players = numTotal - numHumans 
    List<string> getPlayers()
    {
        //Temporary; assuming 1 human, 1 AI
        return new List<string>(){"Player1", "CPU1"};
    }

    //COPIED FROM UNO.cs
    // Shuffle the players with a simple algorithm
    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    /*----------------------------------------------------------------------------------------------------------------------*/

    // Start is called before the first frame update
    void Start()
    {
        //Connect to turnActionManager
        UNOsystem = FindObjectOfType<UNO>();
        //actionManager = Gameobject.Find("turnManager").GetComponent("turnActionManager");
        actionManager = FindObjectOfType<turnActionManager>();

        //Get player data
        players = getPlayers();
        turnOrder = new List<string>(players);

        //Create turn order
        //Shuffle(turnOrder);//? Randomizing function for lists; should work for this, right?
        
        
        foreach (string player in turnOrder)
        {
            print(player);
        }
        

        //Set first player
        //currPlayer = players[0];

        //Set turn direction
        //  +Clockwise = 1
        //  +Counterclockwise = -1
        turnDirection = 1;
    }

    //string test;

    // Update is called once per frame
    void Update()
    {
        //Current turn is first in list
        //  +Change by altering list
        //      -Clockwise: move front to back
        //      -Counterclockwise: move back to front

        //Change direction according to game rules? Maybe should be handled in turnActionManager
        //If (reverse card played) AND (>2 players.size)
        //  turnDirection*=(-1);
        /*
        if ()
        */

        //Change display

        //test = turnOrder[0];
        //print(test);

        //If (turn end)
        //  move to next turn.
        if(actionManager.phase > 2)
        {
            //Change player whose turn it is
            if(turnDirection>0)
            {
                storedPlayer = turnOrder[0];

                turnOrder.Remove(storedPlayer);
                turnOrder.Add(storedPlayer);
            }
            else if(turnDirection<0)
            {
                storedPlayer = turnOrder[turnOrder.Count-1];

                turnOrder.Remove(storedPlayer);
                turnOrder.Insert(0, storedPlayer);
            }
        }
    }
}
