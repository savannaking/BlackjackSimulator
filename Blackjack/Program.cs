using Blackjack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            //instantiate x# decks / up to six players
            Console.WriteLine("Enter the number of decks you are playing with.");
            var numberOfDecks = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the number of players, up to six.");
            var numberOfPlayers = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the bet amount.");
            var bet = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the # of hands to simulate.");
            var numberOfHands = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("All players start with $500");

            Game game = new Game(numberOfDecks, numberOfPlayers, bet);
            int handNumber = 0;
            while (handNumber < numberOfHands)
            {
                handNumber++;
                Console.WriteLine(String.Format("Hand {0}: ", handNumber));
                game.Play();
                game.CalculatePayouts();

                for (int i = 0; i < game.Players.Count; i++)
                {
                    Console.WriteLine(String.Format("Player {0} has won {1} of {2} games and has ${3} remaining",
                        i, game.Players[i].WinCount, handNumber, game.Players[i].TotalChipsValue));
                }

                Console.WriteLine(String.Format("Dealer has won {0} of {1} games",
                        game.Dealer.WinCount, handNumber));
                game.Reset();
            }
            Console.ReadKey();
        }
    }
}
