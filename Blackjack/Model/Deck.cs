using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        internal int cardTotal
        {
            get
            {
                return NumberOfDecks * 52;
            }
        }

        internal int remainingCards
        {
            get
            {
                return this.Cards.Count;
            }
        }

        public List<Card> Cards { get; set; }
        public int NumberOfDecks { get; set; }

        public Deck(int numberOfDecks)
        {
            this.NumberOfDecks = numberOfDecks;
            List<Card> cards = new List<Card>();

            //for each deck being used, create one of each suit
            for (int i = 0; i < numberOfDecks; i++)
            {
                //iterate over face values
                for (int faceValue = 0; faceValue <= 12; faceValue++)
                {
                    //iterate over suits
                    for (int suit = 0; suit <= 3; suit++) {
                        Card card = new Card()
                        {
                            Suit = (Suit)suit,
                            FaceValue = (FaceValue)faceValue
                        };
                        cards.Add(card);
                    }
                }
            }

            this.Cards = cards;
        }
    }
}
