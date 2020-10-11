using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private UNO uno;

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = UNO.GenerateDeck();

        // FindObjectOfType returns the object that matches the type
        // and null if no object natches the type.
        // uno variable is used to access the sprites
        uno = FindObjectOfType<UNO>();

        // Loop to match the name of the card with the stings in the deck
        int i = 0;        
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = uno.cardFaces[i];
                break;
            }
            i++;
        }
        // GetComponent returns the component of type if the game object 
        // has one attached, null if it doesn't
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }
    

    // Update is called once per frame
    void Update()
    {
        // If the card is faced up in the selectable script
        // then show the cards face otherwise show the card's back
        if (selectable.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }
}
