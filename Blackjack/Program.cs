﻿using Blackjack.Model;
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

            Game game = new Game(numberOfDecks, numberOfPlayers);

            //shuffle the deck 
            game.Deck.Shuffle();

            //deal to each player, including dealer
            game.Deal();

            //reset
        }
    }
}