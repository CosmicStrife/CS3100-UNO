using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class UNO : MonoBehaviour
{
    private UserInput input;
    private turnActionManager actionManager;


    // Sprite array to hold the png card images
    public Sprite[] cardFaces;
    // Game objects for cardPrefab, CPU and Player1 hand place holders 
    public GameObject cardPrefab;
    public GameObject CPUPos;
    public GameObject Player1Pos;
    public GameObject DiscardPos;

    // string lists ans string arrays used for building the deck and holding first card hands.
    public List<string> deck;
    public static string[] color = new string[] { "R", "B", "Y", "G" }; //Red, Blue, Yellow, Green
    public static string[] values = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " Draw Two", " Skip", " Reverse" };
    public static string[] wilds = new string[] { " Wild", " Wild Draw Four" };
    public List<string> CPU1 = new List<string>();
    public List<string> Player1 = new List<string>();
    public List<string> Discard = new List<string>();

    public char curColor;

    // Start is called before the first frame update
    void Start()
    {

        //List<string> CPUCards = CPU1;
        //List<string> Player1Cards = Player1;
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        Shuffle(deck);
        UNOInitialDeal(7);

        /*
        test the cards in the deck:
        foreach (string card in deck)
        {
            print(card);
        }
        
        print("The cards in CPU1: ");
        foreach (string card in CPU1)
        {
            print(card);
        }

        print("The cards in Player1: ");
        foreach (string card in Player1)
        {
            print(card);
        }

        
        print("The cards in DECK ");
        foreach (string card in deck)
        {
            print(card);
        }
        */
    }

    // Build the deck by concatenating strings
    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string c in color)
        {
            foreach (string v in values)
            {
                newDeck.Add(c + v);
                if (v != "0")
                {
                    newDeck.Add(c + v);
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            newDeck.Add(wilds[0]);
            newDeck.Add(wilds[1]);
        }
        return newDeck;
    }

    // Shuffle the deck with a simple algorithm
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

    // Take cards from shuffled deck and put them in CPU1 and Player1 list
    public void UNODraw(int num, List<string> list)
    {
        for (int i = 0; i < num; i++)
        {
            list.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
        }
    }

    // Move cards from CPU1 and Player1 lists and display them using 
    // placeholders CPUPos and Player1Pos
    // Also place a card to start in the discard pile
    void UNOInitialDeal(int num)
    {
        /* Deal cards to the CPU */
        UNODraw(num, CPU1);
        float xOffset = 0.03f;
        float yOffset = 0.03f;
        float zOffset = 0.03f;
        foreach (string card in CPU1)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(CPUPos.transform.position.x - xOffset, CPUPos.transform.position.y - yOffset, CPUPos.transform.position.z - zOffset), Quaternion.identity, CPUPos.transform);
            newCard.name = card;
            // GetComponent returns the component of type if the game object has one attached, null if it doesn't.
            newCard.GetComponent<Selectable>().faceUp = false;
            newCard.GetComponent<Selectable>().playerCard = false;
            xOffset = xOffset + 0.8f;
            zOffset = zOffset + 0.05f;
        }
        /* Print cards in CPU's hand
        for(int i = 0; i < CPU1.Count(); i++)
        {
            print(CPU1[i]);
        }
        */

        /* Deal cards to Player1 */
        UNODraw(num, Player1);
        xOffset = 0.03f;
        yOffset = 0.03f;
        zOffset = 0.03f;
        foreach (string card in Player1)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(Player1Pos.transform.position.x + xOffset, Player1Pos.transform.position.y - yOffset, Player1Pos.transform.position.z - zOffset), Quaternion.identity, Player1Pos.transform);
            newCard.name = card;
            // GetComponent returns the component of type if the game object has one attached, null if it doesn't.
            newCard.GetComponent<Selectable>().faceUp = true;
            newCard.GetComponent<Selectable>().playerCard = true;
            xOffset = xOffset + 1.0f;
            zOffset = zOffset + 0.05f;
        }

        /* Deal a card to the discard pile */
        int index = 0;
        //Make sure card added is not a wild card by checking first letter for ' '
        //Otherwise, make sure the ending letter isn't o, p, or e for "Draw Two", "Skip", or "Reverse"
        for (int i = deck.Count - 1; i >= 0; i--)
        {
            //Check first letter for ' ' 
            if (deck[i][0] != ' ' && deck[i][deck[i].Length - 1] != 'o' && deck[i][deck[i].Length - 1] != 'p' && deck[i][deck[i].Length - 1] != 'e')
            {
                index = i;
                i = -1;
            }
        }
        Discard.Add(deck[index]);
        deck.RemoveAt(index);
        zOffset = 0.03f;
        GameObject Dcard = Instantiate(cardPrefab, new Vector3(DiscardPos.transform.position.x, DiscardPos.transform.position.y, DiscardPos.transform.position.z - zOffset), Quaternion.identity, DiscardPos.transform);
        Dcard.name = Discard[0];
        curColor = Dcard.name[0]; //Set the current color to the top card on discard pile
        Dcard.GetComponent<Selectable>().faceUp = true;
        Dcard.GetComponent<Selectable>().playerCard = false;
        //print to make sure a card is removed
        //print(deck.Count());
    }

    public void turnDraw(byte numCards, List<string> player)
    {
        for(byte i=0; i<numCards; i++)
        {
            if(player == Player1)
            {
                //
                UNODraw(1, player);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, Player1Pos.transform);//Player1Pos.transform.position.x + xOffset, Player1Pos.transform.position.y - yOffset, Player1Pos.transform.position.z - zOffset), Quaternion.identity, Player1Pos.transform);
                newCard.name = player[player.Count-1];
                // GetComponent returns the component of type if the game object has one attached, null if it doesn't.
                newCard.GetComponent<Selectable>().faceUp = true;
                newCard.GetComponent<Selectable>().playerCard = true;
                FindObjectOfType<UserInput>().allAudio[2].Play();

            }
            else
            {
                UNODraw(1, player);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, CPUPos.transform);//Player1Pos.transform.position.x + xOffset, Player1Pos.transform.position.y - yOffset, Player1Pos.transform.position.z - zOffset), Quaternion.identity, Player1Pos.transform);
                newCard.name = player[player.Count-1];
                // GetComponent returns the component of type if the game object has one attached, null if it doesn't.
                newCard.GetComponent<Selectable>().faceUp = false;
                newCard.GetComponent<Selectable>().playerCard = false;
                FindObjectOfType<UserInput>().allAudio[2].Play();
            }
        }

        return;
    }

    //INCOMPLETE: Current playing code relies on hits from user input. Not sure how to make it work with AI actions.
    //Moves a card from the hand to the discard pile
    //Assuming passed cardName is legal to play
    public void playFromHand(GameObject handPos, List<string> hand, string cardName)
    {
        if(cardName == "NULL")
        {
            print("  >Not playing a card");
            return;
        }
        print("  >Playing "+cardName);
        
        /*Place the card onto the discard pile and remove from hand*/
        //GameObject temp = GameObject.Find("UNOGame").GetComponent<UNO>().DiscardPos;
        //hit.transform.parent = temp.transform;
        
        //Move card onto top of discard pile
        //hit.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z - .03f * (GameObject.Find("UNOGame").GetComponent<UNO>().Discard.Count + 1));
        
        //Add to the discard pile and remove from player hand
        hand.Remove(cardName);
        Discard.Add(cardName);

        //hit.transform.GetComponent<Selectable>().playerCard = false;

        //Moving to function
        input.refresh_hand_display(handPos);

        curColor = cardName[0];
        actionManager.getRules(cardName);
        actionManager.playDone = true;

        return;
    }
}
