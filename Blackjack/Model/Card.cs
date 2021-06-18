using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public enum Suit
    {
        Hearts,
        Diamonds, 
        Spades,
        Clubs
    }

    public enum FaceValue        
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card
    {
        public Suit Suit { get; set; }
        public FaceValue FaceValue { get; set; }

        public string Face()
        {
            switch (this.FaceValue)
            {
                case FaceValue.Two:
                    return "Two";
                case FaceValue.Three:
                    return "Three";
                case FaceValue.Four:
                    return "Four";
                case FaceValue.Five:
                    return "Five";
                case FaceValue.Six:
                    return "Six";
                case FaceValue.Seven:
                    return "Seven";
                case FaceValue.Eight:
                    return "Eight";
                case FaceValue.Nine:
                    return "Nine";
                case FaceValue.Ten:
                    return "Ten";
                case FaceValue.Jack:
                    return "Jack";
                case FaceValue.Queen:
                    return "Queen";
                case FaceValue.King:
                    return "King";
                case FaceValue.Ace:
                    return "Ace";
                default:
                    return null;
            }
        }

        public int CardScore
        {
            get {
                switch (this.FaceValue)
                {
                    case (FaceValue.Two):
                        return 2;
                    case (FaceValue.Three):
                        return 3;
                    case (FaceValue.Four):
                        return 4;
                    case (FaceValue.Five):
                        return 5;
                    case (FaceValue.Six):
                        return 6;
                    case (FaceValue.Seven):
                        return 7;
                    case (FaceValue.Eight):
                        return 8;
                    case (FaceValue.Nine):
                        return 9;
                    case (FaceValue.Ace):
                        return 11;
                    default:
                        return 10;
                }
            }
        }
    }
}
