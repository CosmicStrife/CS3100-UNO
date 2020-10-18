using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class UNO : MonoBehaviour
{
    // Sprite array to hold the png card images
    public Sprite[] cardFaces;
    // Game objects for cardPrefab, CPU and Player1 hand place holders 
    public GameObject cardPrefab;
    public GameObject CPUPos;
    public GameObject Player1Pos;

    // string lists ans string arrays used for building the deck and holding first card hands.
    public List<string> deck;
    public static string[] color = new string[] { "Red", "Blue", "Yellow", "Green" };
    public static string[] values = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " Draw Two", " Skip", " Reverse" };
    public static string[] wilds = new string[] { " Wild", " Wild Draw Four" };
    public List<string> CPU1 = new List<string>();
    public List<string> Player1 = new List<string>();


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

        //test the cards in the deck:
        //foreach (string card in deck)
        //{
        //    print(card);
        //}

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

        /*
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
    void UNODraw(int num, List<string> list)
    {
        for (int i = 0; i < num; i++)
        {
            list.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
        }
    }

    // Move cards from CPU1 and Player1 lists and display them using 
    // placeholders CPUPos and Player1Pos
    void UNOInitialDeal(int num)
    {
        print("Decksize before draw" + deck.Count());
        UNODraw(num, CPU1);
        print("Decksize after draw" + deck.Count());
        for(int i = 0; i < CPU1.Count(); i++)
        {
            print(CPU1[i]);
        }
        //for (int i = 0; i < num; i++)
        //{
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
        //}

        UNODraw(num, Player1);
        //for (int i = 0; i < num; i++)
        //{
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
                xOffset = xOffset + 0.8f;
                zOffset = zOffset + 0.05f;
            }
        //}
    }

    // Commented test code for position of CPU1 and Player1 hands 
    //float xOffset = 0;
    //float yOffset = 0.03f;
    //float zOffset = 0.03f;
    //        foreach (string card in CPU1)
    //        {
    //            GameObject newCard = Instantiate(cardPrefab, new Vector3(CPUPos.transform.position.x, CPUPos.transform.position.y - yOffset, CPUPos.transform.position.z - zOffset), Quaternion.identity, CPUPos.transform);
    //newCard.name = card;
    //            newCard.GetComponent<Selectable>().faceUp = true;                
    //            xOffset = xOffset + 0.5f;
    //            zOffset = zOffset + 0.05f;


    // UnoDeal initial function used for testing and displaying all cards from the middle down
    //void UNODeal()
    //{
    //    float yOffset = 0;
    //    float zOffset = 0.03f;
    //    foreach (string card in deck)
    //    {
    //        GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z - zOffset), Quaternion.identity);
    //        newCard.name = card;
    //        newCard.GetComponent<Selectable>().faceUp = true;
    //        yOffset = yOffset + 0.5f;
    //        zOffset = zOffset + 0.03f;
    //    }
    //}
}
