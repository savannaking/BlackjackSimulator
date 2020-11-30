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
