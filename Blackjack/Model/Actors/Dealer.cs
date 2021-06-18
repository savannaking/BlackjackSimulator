using Blackjack.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Model
{
    public class Dealer : Player
    {
        public Dealer(Game game, decimal bet = 0) : base(game, bet)
        {

        }

        public Card UpCard
        {
            get
            {
                return Hand.Last();
            }
        }

        public List<Card> VisibleHand
        {
            get
            {
                return Hand.GetRange(1, Hand.Count-1);
            }
        }

        public override int CurrentHandScore
        {
            get
            {
                return VisibleHand.Sum(x => x.CardScore) + 10;
            }
        }

        public override void Play(int i = 0)
        {
            //lower than 17, dealer hits until they hit 17
            if (CurrentHandScore < 17)
                Hit();
            else
                if (!Busted)
                    Stay();
        }

        public void Deal()
        {
            int i = 0;
            //needs to be in order - dealer is last in the list
            //give each player a card from the top of the deck, twice
            while (i < 2)
            {
                for (int j = 0; j < Game.Players.Count; j++)
                {
                    DealToPlayer(Game.Players[j], j);
                }
                DealToDealer();
                i++;
            }
        }

        public void HandleInsuranceBets()
        {
            //if your score is 20 you should take the insurance bet
            for (int i = 0; i < Game.Players.Count; i++)
            {
                if (Game.Players[i].GetType() == typeof(Counter))
                {
                    //add logic for counting strategy here later
                }
                else if (Game.Players[i].CurrentHandScore == 20)
                {
                    Game.Players[i].TakeInsuranceBet = true;
                    Game.Players[i].TotalChipsValue -= Game.Players[i].Bet / 2;
                    Console.WriteLine(String.Format("Player {0} bet ${1} for insurance", i, Game.Players[i].Bet / 2));
                }
            }
        }

        public void DealToPlayer(Player player, int i)
        {
            player.Hand.Add(Game.Deck.Cards[Game.Deck.remainingCards - 1]);
            Game.Deck.Cards.RemoveAt(Game.Deck.remainingCards - 1);

            Console.WriteLine(String.Format("Player {0} was dealt a {1} of {2}",
                i, Game.Players[i].Hand.Last().Face(), Game.Players[i].Hand.Last().Suit));

            Game.UpdateCount(player.Hand.Last());
        }

        private void DealToDealer()
        {
            Hand.Add(Game.Deck.Cards[Game.Deck.remainingCards - 1]);
            Game.Deck.Cards.RemoveAt(Game.Deck.remainingCards - 1);

            if (Hand.Count > 1) 
            {
                if (Hand.Count == 2)
                {
                    Console.WriteLine(String.Format("Dealer was dealt a downturned card",
                        Hand.Last().Face(), Hand.Last().Suit));
                }

                Game.UpdateCount(UpCard);
                if (VisibleHand.Any(x => x.FaceValue == FaceValue.Ace))
                {
                    Console.WriteLine("Dealer has an ace. Taking insurance bets.");
                    HandleInsuranceBets();
                }
            }
            else
            {
                Console.WriteLine(String.Format("Dealer was dealt a {0} of {1}",
                     Hand.Last().Face(), Hand.Last().Suit));
            }
        }

        public override void Hit(int i = 0)
        {
            DealToDealer();
            Console.WriteLine(String.Format("Dealer hit and was dealt a {0} of {1}", Hand.Last().FaceValue, Hand.Last().Suit));
            Play();
        }

        public override void Stay(int i = 0)
        {
            Console.WriteLine("Dealer stays");
        }

        public void Shuffle()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = Game.Deck.Cards.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                Card card = Game.Deck.Cards[k];
                Game.Deck.Cards[k] = Game.Deck.Cards[n];
                Game.Deck.Cards[n] = card;
            }
        }

    }
}
