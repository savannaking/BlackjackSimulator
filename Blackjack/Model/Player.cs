using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Player
    {
        public List<Card> Hand { get; set; }
        public bool IsDealer { get; set; }
        public bool IsPlayerOne { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }

        public void Play()
        {
            //first try strategy without taking count into consideration
            if (!IsPlayerOne && !IsDealer)
            {
                PlayBasicStrategy();
            }
            else if (IsPlayerOne)
            {
                PlayCountingStrategy();
            }
            else //dealer
            {
                PlayDealerStrategy();
            }
        }

        private void PlayDealerStrategy()
        {
            //dealer must take card if they have less than 17, otherwise they don't 
            //if everyone busts, don't take a card
        }

        private void PlayCountingStrategy()
        {
            throw new NotImplementedException();
        }

        private void PlayBasicStrategy()
        {
            //don't go over 21
            //2-9 are face value, rest are ten
            //you can take multiple cards if you are still below twelve after taking the first

            //if you have under ten, take a card
            //if you have 10 or 11, take a card and double down unless dealer has an Ace
            //if you have 12-16 and dealer has 2-6, don't take a card
            //if you have 12-16 and dealer has 7 to ace, take a card
            //if you have 17+, you stay (don't take a card)      
        }

            public void DoubleDown()
        {
            //dealer deals one more card
            //double your bet
        }
    }
}
