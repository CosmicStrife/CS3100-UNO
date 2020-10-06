using System;

namespace Daniel_Xiang_Test
{
    //Class containing data and functions that a card can need
    class card
    {
        //Card Qualities
        private string m_color;
        private string m_symbol;

        //Contains characters meant to signify special rules that should be applied
        //EX: "d2" -> next person draws two cards
        private string m_rules;

        //Tells if a card has a matching quality with another (or if one is a wild card)
        public bool matches(card target)
        {
            if ((m_color == target.m_color) || (m_symbol == target.m_symbol) || (m_color == "Black") || (target.m_symbol == "Black"))// Wild symbol should be handled by rules? || m_symbol == "Wild" || m_s)
            {
                return true;
            }

            return false;
        }

        //Gets the rules  string
        public string getRules()
        {
            return m_rules;
        }



        //Constructors
        //  Default
        //  Parameterized
        //  Copy
        //  Assignment

        //Destructor
    }

    //Meant to store the number of cards in each area (Deck, Hands, played stack) and use them to decide what card to play.
    class memMatrix
    {
        byte[,] quantity = new byte[5,15];

        string[] colors = {"Red", "Blue", "Yellow", "Green", "Black"};
        string[] symbols = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Skip", "Draw Two", "Reverse", "Wild", "Wild Draw 4"};
        //Can "Wild" and "Wild draw 4" use the same symbol?
        
    }

    class DX_Main_Class
    {
        //Build deck
        //  Constructing card array to hold deck
        //  Constructing integer to hold number of cards in deck
        card[] deck = new card[5,15];
        byte deckSize = 108;

        string[] colors = new string[5] {"Red", "Blue", "Yellow", "Green", "Black"};

        for (byte i=0; i<100; i++)
        {
            //  Iterating through RBYG
            for (byte j=0; j<4; i++)
            {
                for (byte k=0; k<=9; k++)
                {
                    
                    //.ToString()
                }
            }
        }
    }
}