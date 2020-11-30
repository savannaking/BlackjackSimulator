using Blackjack.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Model
{
    public class Game
    {
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public Dealer Dealer { get; set; }
        public int CurrentCount { get; set; }
        public decimal Bet { get; set; }


        public Game(int numberOfDecks, int numberOfPlayers, decimal bet)
        {
            Deck = new Deck(numberOfDecks);
            CurrentCount = 0;

            Players = new List<Player>() { };

            //one player is testing the counting strategy, the rest are using a basic strategy - may add ability to change this later
            Players.Add(new Counter(this));

            for (int i = 1; i < numberOfPlayers; i++)
            {
                Players.Add(new Player(this)); //uses basic strategy
            }

            Dealer dealer = new Dealer(this);

            Bet = bet;
        }

        public void Play(int numberOfHands)
        {
            //the dealer shuffles the deck 
            Dealer.Shuffle();

            //recreate the deck if there is less than a quarter of the whole original remaining
            //or if there aren't enough cards for all the players
            if (Deck.remainingCards >= Deck.cardTotal / 4 && Deck.remainingCards > Players.Count * 2)
            {
                Dealer.Deal();

                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].Play();
                }

                Dealer.Play();

                CalculatePayouts();
            }
            else
            {
                Deck = new Deck(Deck.NumberOfDecks);
                Dealer.Shuffle();
                Dealer.Deal();
            }

            //reset     
        }

        public void CalculatePayouts()
        {
            if (!Dealer.Busted)
            {
                if (Dealer.HasBlackjack)
                {
                    foreach (Player player in Players)
                    {
                        if (player.TakeInsuranceBet)
                        {
                            //they break even
                            player.CurrentWinnings += Bet;
                        }
                        else
                        {
                            player.CurrentWinnings = 0;
                        }
                        Dealer.WinCount++;
                    }
                }
                else
                {
                    foreach (Player player in Players)
                    {
                        if (player.TakeInsuranceBet)
                        {
                            player.CurrentWinnings = 0;
                        }
                        if (player.Busted)
                        {
                            player.CurrentWinnings = 0;
                        }
                        else
                        {
                            if (player.HasBlackjack)
                            {
                                player.CurrentWinnings = Bet * 1.5m;
                                player.TotalWinnings += player.CurrentWinnings;
                                player.WinCount++;
                            }
                            else if (player.CurrentHandScore > Dealer.CurrentHandScore)
                            {
                                player.CurrentWinnings = Bet * 2;
                                player.TotalWinnings += player.CurrentWinnings;
                                player.WinCount++;
                            }
                            else
                            {
                                player.CurrentWinnings = Bet * -1;
                            }
                        }

                    }
                }
            }
            else //dealer busted
            {

            }
        }

            //if dealer doesn't have blackjack and you take insurance you lose your insurance bet which is half your original, plus original
            //if he does, you come out even 2-1 on your 1/2 insurance bet
        }

        public void UpdateCount(Card lastCardDealt)
        {
            switch (lastCardDealt.FaceValue)
            {
                //2 - 6 is +1
                case FaceValue.Two:
                case FaceValue.Three:
                case FaceValue.Four:
                case FaceValue.Five:
                case FaceValue.Six:
                    this.CurrentCount++;
                    break;
                //7,8,9 is 0
                //don't need to do anything here for these
                //10 thru ace is -1
                case FaceValue.Ten:
                case FaceValue.Jack:
                case FaceValue.Queen:
                case FaceValue.King:
                case FaceValue.Ace:
                    this.CurrentCount--;
                    break;
            }
        }

    }
}
