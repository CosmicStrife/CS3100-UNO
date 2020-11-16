using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class HardAI : MonoBehaviour
{
    private turnActionManager actionManager;
    private UNO UNOsystem;

    //GameObject WildMenu;
    private WildMenu WMsystem;

    private UserInput input;
    /*
    public GameObject signalBlue;
    public GameObject signalGreen;
    public GameObject signalRed;
    public GameObject signalYellow;
    */


    List<List<byte>> maxQuants;
    //List<List<byte>> deckModel;
    public List<List<byte>> discardMem;
    public List<List<byte>> handMem;


    //Used in processing function
    //Weights intended to be values found with a genetic algorithm; not enough time to implement, so algorithms used.
    List<double> hand_color_weights;
    List<double> hand_symbol_weights;
    List<double> played_color_weights;
    List<double> played_symbol_weights;

    List<sbyte> hand_color_signs;
    List<sbyte> hand_symbol_signs;
    List<sbyte> played_color_signs;
    List<sbyte> played_symbol_signs;

    //handMem[Player][Color][Symbol]
    /*
    List<List<byte>> AI_handMem;
    List<List<byte>> Player_handMem;
    */

    // Start is called before the first frame update
    void Start()
    {
        UNOsystem = FindObjectOfType<UNO>();
        actionManager = FindObjectOfType<turnActionManager>();
        input = FindObjectOfType<UserInput>();
        //handMem = ;
        maxQuants = new List<List<byte>>()
        {
            new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2,0,0},
            new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2,0,0},
            new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2,0,0},
            new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,4,4}
        };
        handMem = new List<List<byte>>()
        {
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };
        foreach (string card in UNOsystem.CPU1)
        {
            addCard(card, handMem);
        }
        discardMem = new List<List<byte>>()
        {
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        hand_color_weights = new List<double>() {1.5, 1.5, 1.5};
        hand_symbol_weights = new List<double>() {1.5, 1.5, 1.5};
        played_color_weights = new List<double>() {0.9, 0.9, 0.9};
        played_symbol_weights = new List<double>() {0.9, 0.9, 0.9};

        hand_color_signs = new List<sbyte>() {-1, -1, -1};
        hand_symbol_signs = new List<sbyte>() {-1, -1, -1};
        played_color_signs = new List<sbyte>() {-1, -1, -1};
        played_symbol_signs = new List<sbyte>() {-1, -1, -1};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<byte> card_code(string cardName)
    {
        byte rowNum = 5;
        byte colNum = 16;

        switch (cardName[0])
        {
            case 'R'://Red
                rowNum = 0;
                break;
            case 'B'://Blue
                rowNum = 1;
                break;
            case 'Y'://Yellow
                rowNum = 2;
                break;
            case 'G'://Green
                rowNum = 3;
                break;
            case ' '://Wild
                rowNum = 4;
                break;
            default:
                break;
        }

        switch (cardName[cardName.Length - 1])
        {
            case '0':
                colNum = 0;
                break;
            case '1':
                colNum = 1;
                break;
            case '2':
                colNum = 2;
                break;
            case '3':
                colNum = 3;
                break;
            case '4':
                colNum = 4;
                break;
            case '5':
                colNum = 5;
                break;
            case '6':
                colNum = 6;
                break;
            case '7':
                colNum = 7;
                break;
            case '8':
                colNum = 8;
                break;
            case '9':
                colNum = 9;
                break;

            case 'p'://Skip
                colNum = 10;
                break;
            case 'e'://Reverse
                colNum = 11;
                break;
            
            case 'o'://Draw two
                colNum = 12;
                break;

            case 'd'://Wild
                colNum = 13;
                break;
           
            case 'r'://Wild draw four
                colNum = 14;
                break;

            default:
                break;
        }

        return new List<byte>() {rowNum, colNum};
    }
    string card_name(sbyte color, sbyte symbol)
    {
        //print("  Testing: card_name()");
        //print(color);
        //print(symbol);

        string cardName = "";

        switch (color)
        {
            case 0://Red
                cardName = "R";
                break;
            case 1://Blue
                cardName = "B";
                break;
            case 2://Yellow
                cardName = "Y";
                break;
            case 3://Green
                cardName = "G";
                break;
            case 4://Wild
                cardName = " ";
                break;
            default:
                print("ERROR: Color not found");
                break;
        }

        switch (symbol)
        {
            case 0:
                cardName += "0";
                break;
            case 1:
                cardName += "1";
                break;
            case 2:
                cardName += "2";
                break;
            case 3:
                cardName += "3";
                break;
            case 4:
                cardName += "4";
                break;
            case 5:
                cardName += "5";
                break;
            case 6:
                cardName += "6";
                break;
            case 7:
                cardName += "7";
                break;
            case 8:
                cardName += "8";
                break;
            case 9:
                cardName += "9";
                break;
            case 10://Skip
                cardName += " Skip";
                break;
            case 11://Reverse
                cardName += " Reverse";
                break;
            case 12://Draw 2
                cardName += " Draw Two";
                break;
            case 13://Wild
                cardName += "Wild";
                break;
            case 14://Wild draw 4
                cardName += "Wild Draw Four";
                break;
            default:
                print("ERROR: Symbol not found");
                break;
        }

        print("  card_name(): "+cardName);
        return cardName;
    }

    public void addCard(string cardName, List<List<byte>> cardSet)
    {
        List<byte> cardData = card_code(cardName);

        cardSet[cardData[0]][cardData[1]]++;

        return;
    }
    public void removeCard(string cardName, List<List<byte>> cardSet)
    {
        List<byte> cardData = card_code(cardName);

        cardSet[cardData[0]][cardData[1]]--;

        return;
    }

    /*
    byte getQuantity(string cardName, List<List<byte>> cardSet)
    {
        //

    }
    */

    
    List<byte> getMax(List<List<byte>> cardSet)
    {
        byte max = cardSet[0][0];
        byte maxRow = 0;
        byte maxCol = 0;
        
        string cardName;

        for (byte i = 0; i < cardSet.Count; i++)
        {
            for (byte k = 0; k < cardSet[0].Count; k++)
            {
                if(max < cardSet[i][k])
                {
                    max = cardSet[i][k];
                    maxRow = i;
                    maxCol = k;
                }
            }
        }

        return new List<byte>() {maxRow, maxCol};//card_name(maxRow, maxCol);
    }
    List<byte> getMin(List<List<byte>> cardSet)
    {
        byte min = cardSet[0][0];
        byte minRow = 0;
        byte minCol = 0;
        
        string cardName;

        for (byte i = 0; i < cardSet.Count; i++)
        {
            for (byte k = 0; k < cardSet[0].Count; k++)
            {
                if(min > cardSet[i][k])
                {
                    min = cardSet[i][k];
                    minRow = i;
                    minCol = k;
                }
            }
        }

        return new List<byte>() {minRow, minCol};//card_name(minRow, minCol);
    }

    List<sbyte> get_min_in_hand(List<List<double>> cardSet)
    {
        double min = 99999999999;//cardSet[0][0];
        sbyte minRow = -1;
        sbyte minCol = -1;
        
        string cardName;

        for (byte i = 0; i < cardSet.Count; i++)
        {
            for (byte k = 0; k < cardSet[0].Count; k++)
            {
                if((min > cardSet[i][k]) && (handMem[i][k] > 0))
                {
                    if(input.valid(card_name(Convert.ToSByte(i), Convert.ToSByte(k))))
                    {
                        min = cardSet[i][k];
                        minRow = Convert.ToSByte(i);
                        minCol = Convert.ToSByte(k);
                    }
                }
            }
        }

        print("  HardAI Chosen card: "+card_name(minRow, minCol));

        return new List<sbyte>() {minRow, minCol};//card_name(minRow, minCol);
    }

    List<byte> colorQuants(List<List<byte>> cardSet)
    {
        List<byte> sums = new List<byte>() {0, 0, 0, 0, 0};

        for (byte i = 0; i < cardSet.Count; i++)
        {
            for (byte k = 0; k < cardSet[0].Count; k++)
            {
                sums[i] += cardSet[i][k];
            }
        }

        return sums;
    }
    List<byte> symbolQuants(List<List<byte>> cardSet)
    {
        List<byte> sums = new List<byte>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        for (byte k = 0; k < 15; k++)
        {
            for (byte i = 0; i < 5; i++)
            {
                sums[k] += cardSet[i][k];
            }
        }

        return sums;
    }

    //  +IMPORTANT: input_value should be a PROPORTION of the quantity divided by the total cards with the corresponding quality
    double Processing_function_pt1(double input_value, List<sbyte> signs, List<double> weights)
    {
        double score = 0;
    
        for (byte i = 0; i < weights.Count; i++)
        {
            score += (input_value * /*Convert.ToDouble*/(signs[i]) * Math.Abs(Math.Pow(weights[i],(i+1))));
        }

        return score;
    }

    //Represents probability that a card can be played by another (the human player)
    //  +IMPORTANT: Quantity values should be a PROPORTION of the quantity divided by the total cards with the corresponding quality
    double commonness_score(double hand_color_quantity, double hand_symbol_quantity, double played_color_quantity, double played_symbol_quantitiy)
    {
        double score = 0;

        score += Processing_function_pt1(hand_color_quantity, hand_color_signs, hand_color_weights);
        score += Processing_function_pt1(hand_symbol_quantity, hand_symbol_signs, hand_symbol_weights);
        
        score += Processing_function_pt1(played_color_quantity, played_color_signs, played_color_weights);
        score += Processing_function_pt1(played_symbol_quantitiy, played_symbol_signs, played_symbol_weights);
    
        return score;
    }

    public void Smart_AI_Logic()
    {
        print("TESTING - Smart_AI_Logic()");
        bool playWild = false;

        List<List<double>> comm_scores = new List<List<double>>() 
        {
            new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0},
            new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        //Proportion of cards that the opponent may play still possible
        //double hand_color_quantity, 
        //double hand_symbol_quantity, 
        //double played_color_quantity, 
        //double played_symbol_quantitiy

        List<double> hand_color_quantity_proportions = new List<double>() {0, 0, 0, 0};
        for (byte i = 0; i < hand_color_quantity_proportions.Count; i++)
        {
            hand_color_quantity_proportions[i] += colorQuants(maxQuants)[i];
            hand_color_quantity_proportions[i] -= colorQuants(handMem)[i];
            hand_color_quantity_proportions[i] = hand_color_quantity_proportions[i]/(colorQuants(maxQuants)[i]);
        }

        List<double> hand_symbol_quantity_proportions = new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0};
        for (byte i = 0; i < hand_symbol_quantity_proportions.Count; i++)
        {
            hand_symbol_quantity_proportions[i] += symbolQuants(maxQuants)[i];
            hand_symbol_quantity_proportions[i] -= symbolQuants(handMem)[i];
            hand_symbol_quantity_proportions[i] = hand_symbol_quantity_proportions[i]/(symbolQuants(maxQuants)[i]);
        }

        List<double> discard_color_quantity_proportions = new List<double>() {0, 0, 0, 0};
        for (byte i = 0; i < discard_color_quantity_proportions.Count; i++)
        {
            discard_color_quantity_proportions[i] += colorQuants(maxQuants)[i];
            discard_color_quantity_proportions[i] -= colorQuants(discardMem)[i];
            discard_color_quantity_proportions[i] = discard_color_quantity_proportions[i]/(colorQuants(maxQuants)[i]);
        }

        List<double> discard_symbol_quantity_proportions = new List<double>() {0,0,0,0,0,0,0,0,0,0,0,0,0};
        for (byte i = 0; i < discard_symbol_quantity_proportions.Count; i++)
        {
            discard_symbol_quantity_proportions[i] += symbolQuants(maxQuants)[i];
            discard_symbol_quantity_proportions[i] -= symbolQuants(discardMem)[i];
            discard_symbol_quantity_proportions[i] = discard_symbol_quantity_proportions[i]/(symbolQuants(maxQuants)[i]);
        }
        
        for (byte i = 0; i < 4; i++)
        {
            for (byte k = 0; k < 13; k++)
            {
                comm_scores[i][k] = commonness_score(hand_color_quantity_proportions[i], hand_symbol_quantity_proportions[k], discard_color_quantity_proportions[i], discard_symbol_quantity_proportions[k]);
            }
        }

        //Select card in hand with lowest commonness score
        List<sbyte> temp = get_min_in_hand(comm_scores);
        if (temp[0] == -1 && temp[1] == -1)
        {
            //No non-wild cards can be played
            playWild = true;
        }
        //return card_name(temp[0], temp[1]);

        //Special handling for wild cards
        //  +Save them
        //  +Play as last resort
        //  +Select most common non-black color in (discard pile + hand)
        if(playWild)
        {
            if(handMem[4][14] > 0)
            {
                HardAIplay(" Wild Draw Four");
            }
            else if (handMem[4][14] > 0)
            {
                HardAIplay(" Wild");
            }
            else//No cards (including wild) can be payed
            {
                UNOsystem.turnDraw(1, UNOsystem.CPU1);
                if(input.valid(UNOsystem.CPU1[UNOsystem.CPU1.Count-1]))
                {
                    HardAIplay(UNOsystem.CPU1[UNOsystem.CPU1.Count-1]);
                }
            }
        }
        else
        {
            HardAIplay(card_name(temp[0], temp[1]));
        }
        
        return;
    }

    void HardAIplay(string cardName)
    {
        print("  Hard AI playing: "+cardName);

        actionManager.AIplay = true;
        actionManager.AIselectedCard = cardName;
        actionManager.AIPlay(cardName);
        if(cardName[0] == ' ')
        {
            byte minColor;
            byte min;
            //
            List<List<byte>> possible_plays = new List<List<byte>>()
            {
                new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2},
                new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2},
                new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2},
                new List<byte>() {2,4,4,4,4,4,4,4,4,4,2,2,2}
            };
            for (byte i = 0; i < 4; i++)
            {
                for (byte k = 0; k < 13; k++)
                {
                    possible_plays[i][k] -= (handMem[i][k]);
                    possible_plays[i][k] -= (discardMem[i][k]);
                }
            }
            
            List<byte> possible_colors = colorQuants(possible_plays);

            minColor = 0;
            min = possible_colors[0];
            for (byte i = 1; i < 4; i++)
            {
                if(min < possible_colors[i])
                {
                    minColor = i;
                    min = possible_colors[i];
                }
            }

            switch (minColor)
            {
                case 0://R
                    UNOsystem.curColor = 'R';
                    actionManager.StartCoroutine(actionManager.colorRed());
                    break;
                case 1://B
                    UNOsystem.curColor = 'B';
                    actionManager.StartCoroutine(actionManager.colorBlue());
                    break;
                case 2://Y
                    UNOsystem.curColor = 'Y';
                    actionManager.StartCoroutine(actionManager.colorYellow());
                    break;
                case 3://G
                    UNOsystem.curColor = 'G';
                    actionManager.StartCoroutine(actionManager.colorGreen());
                    break;
                default:
                    break;
            }
        }

        removeCard(cardName, handMem);
        addCard(cardName, discardMem);
        return;
    }

}
