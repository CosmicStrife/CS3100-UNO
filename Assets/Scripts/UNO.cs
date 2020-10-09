using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNO : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public static string[] color = new string[] { "Red", "Blue", "Yellow", "Green" };
    public static string[] values = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " Draw Two", " Skip", " Reverse"};
    public static string[] wilds = new string[] { " Wild", " Wild Draw Four" };

    public List<string> deck;
    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayCards()
    {
        //foreach (List<string> list in bottoms)
        //{
        //    list.Clear();
        //}

        deck = GenerateDeck();
        Shuffle(deck);
        UNODeal();

        //test the cards in the deck:
        foreach (string card in deck)
        {
            print(card);
        }
        

    }


    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in color)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
                if (v != "0")
                {
                    newDeck.Add(s + v);
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

    void UNODeal()
    {
        float yOffset = 0;
        float zOffset = 0.03f;
        foreach (string card in deck)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z - zOffset), Quaternion.identity);
            newCard.name = card;
            newCard.GetComponent<Selectable>().faceUp = true;
            yOffset = yOffset + 0.5f;
            zOffset = zOffset + 0.03f;
        }
    }



}
