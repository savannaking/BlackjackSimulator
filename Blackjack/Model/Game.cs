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
        public int CurrentCount { get; set; }

        public Game(int numberOfDecks, int numberOfPlayers)
        {
            Deck = new Deck(numberOfDecks);
            CurrentCount = 0;

            Players = new List<Player>() { };
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Players.Add(new Player());
            }

            //add the dealer as a player w/ flag
            Players.Add(new Player()
            {
                IsDealer = true
            });

            Players[0].IsPlayerOne = true;
        }

        public void Deal()
        {
            GameFunctions.Deal(this);
        }
    }
}
