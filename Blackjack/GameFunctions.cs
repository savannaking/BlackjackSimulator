using Blackjack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public static class GameFunctions
    {
        public static void Shuffle(this Deck deck)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = deck.Cards.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                Card card = deck.Cards[k];
                deck.Cards[k] = deck.Cards[n];
                deck.Cards[n] = card;
            }
        }

        public static void Deal(this Game game)
        {
            //recreate the deck if there is less than a quarter of the whole original remaining
            //or if there aren't enough cards for all the players
            if (game.Deck.remainingCards >= game.Deck.cardTotal / 4 && game.Deck.remainingCards > game.Players.Count * 2) 
            {
                //needs to be in order - dealer is last in the list
                //give each player a card from the top of the deck, twice
                for (int i = 0; i < game.Players.Count; i++)
                {
                    DealToPlayer(game.Players[i], game, 1);
                }
                for (int i = 0; i < game.Players.Count; i++)
                {
                    DealToPlayer(game.Players[i], game, 2);
                }
            }
            else
            {
                game.Deck = new Deck(game.Deck.NumberOfDecks);
                GameFunctions.Shuffle(game.Deck);
                GameFunctions.Deal(game);
            }
        }

        private static void DealToPlayer(Player player, Game game, int cardNumber)
        {
            player.Hand.Add(game.Deck.Cards[game.Deck.remainingCards - 1]);
            game.Deck.Cards.RemoveAt(game.Deck.remainingCards - 1);

            if (cardNumber == 1 || (cardNumber == 2 && !player.IsDealer))
            {
                GameFunctions.UpdateCount(game, player.Hand.Last());
            }
        }

        private static void UpdateCount(Game game, Card lastCardDealt)
        {
            switch(lastCardDealt.FaceValue)
            {
                //2 - 6 is +1
                case FaceValue.Two:
                case FaceValue.Three:
                case FaceValue.Four:
                case FaceValue.Five:
                case FaceValue.Six:
                    game.CurrentCount++;
                    break;
                //7,8,9 is 0
                //don't need to do anything here for these
                //10 thru ace is -1
                case FaceValue.Ten:
                case FaceValue.Jack:
                case FaceValue.Queen:
                case FaceValue.King:
                case FaceValue.Ace:
                    game.CurrentCount--;
                    break;
            }
        }
    }
}
