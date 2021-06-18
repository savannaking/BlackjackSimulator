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


        public Game(int numberOfDecks, int numberOfPlayers, decimal bet)
        {
            Deck = new Deck(numberOfDecks);
            CurrentCount = 0;

            Players = new List<Player>() { };

            //one player is testing the counting strategy, the rest are using a basic strategy - may add ability to change this later
            //Players.Add(new Counter(this, bet));

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                Players.Add(new Player(this, bet)); //uses basic strategy
            }

            Dealer = new Dealer(this);
        }

        public void Play()
        {
            foreach (var player in Players)
            {
                player.TotalChipsValue -= player.Bet;
            }

            //the dealer shuffles the deck 
            Dealer.Shuffle();

            //recreate the deck if there is less than a quarter of the whole original remaining
            //or if there aren't enough cards for all the players
            if (Deck.remainingCards >= Deck.cardTotal / 4 && Deck.remainingCards > Players.Count * 2)
            {
                Dealer.Deal();

                for(int i = 0; i < Players.Count; i++)
                {
                    Players[i].Play(i);
                }
                Console.WriteLine(String.Format("Dealer reveals a {0} of {1}",
                    Dealer.Hand.Last().Face(), Dealer.Hand.Last().Suit));
                Dealer.Play();
            }
            else
            {
                Deck = new Deck(Deck.NumberOfDecks);
                Dealer.Shuffle();
                Dealer.Deal();
            }
        }

        public void CalculatePayouts()
        {
            if (!Dealer.Busted)
            {
                if (Dealer.HasBlackjack)
                {
                    Dealer.WinCount++;

                    Console.WriteLine("Dealer has blackjack. :( ");
                    for(int i = 0; i < Players.Count; i++)
                    {
                        var player = Players[i];
                        if (player.TakeInsuranceBet)
                        {
                            //they break even
                            player.CurrentWinnings = player.Bet;
                            player.TotalChipsValue += player.CurrentWinnings;
                            Console.WriteLine(String.Format("Player {0} wins insurance bet of {1}", i, player.Bet / 2));
                        }
                        else if (player.HasBlackjack)
                        {
                            Console.WriteLine(String.Format("Player {0} has blackjack!! Returning bet to player.", i));
                            player.CurrentWinnings += player.Bet;
                            player.TotalChipsValue += player.CurrentWinnings;
                            player.WinCount++;
                        }
                        else
                        {
                            Console.WriteLine(String.Format("Player {0} lost bet of ${1}", i, player.Bet));
                            player.CurrentWinnings = 0;
                        }                    }
                }
                else
                {
                    for (int i = 0; i < Players.Count; i++)
                    {
                        var player = Players[i];

                        if (player.TakeInsuranceBet)
                        {
                            //they lose this bet
                            Console.WriteLine(String.Format("Player {0} loses insurance bet of {1}", i, player.Bet / 2));
                        }

                        if (player.Busted)
                        {
                            Console.WriteLine(String.Format("Player {0} busted and lost bet of ${1}", i, player.Bet));
                            Dealer.WinCount++;
                        }
                        else
                        {
                            if (player.HasBlackjack)
                            {
                                Console.WriteLine(String.Format("Player {0} has blackjack and won ${1}.", i, player.Bet * 1.5m));
                                player.CurrentWinnings = (player.Bet * 1.5m) + player.Bet;
                                player.TotalChipsValue += player.CurrentWinnings;
                                player.WinCount++;
                            }
                            else if (player.CurrentHandScore > Dealer.CurrentHandScore)
                            {
                                Console.WriteLine(String.Format("Player {0} won ${1}.", i, player.Bet));
                                player.CurrentWinnings = player.Bet * 2;
                                player.TotalChipsValue += player.CurrentWinnings;
                                player.WinCount++;
                            }
                            else
                            {
                                player.CurrentWinnings = 0;
                                Console.WriteLine(String.Format("Player {0} lost bet of ${1}", i, player.Bet));
                                Dealer.WinCount++;
                            }
                        }

                    }
                }
            }
            else //dealer busted
            {
                Console.WriteLine("Dealer busted.");
                for (int i = 0; i < Players.Count; i++)
                {
                    var player = Players[i];
                    if (!player.Busted)
                    {
                        Console.WriteLine(String.Format("Player {0} won ${1}.", i, player.Bet));
                        player.CurrentWinnings = player.Bet * 2;
                        player.TotalChipsValue += player.CurrentWinnings;
                        player.WinCount++;

                    }
                    else
                    {
                        player.CurrentWinnings = 0;
                        Console.WriteLine(String.Format("Player {0} busted and lost bet of ${1}", i, player.Bet));
                    }
                }
            }
        }

        public void Reset()
        {
            foreach(var player in Players)
            {
                player.Reset();
            }
            Dealer.Reset();
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
