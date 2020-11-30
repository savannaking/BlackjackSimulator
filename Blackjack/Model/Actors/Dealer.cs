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
        public Dealer(Game game) : base(game)
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
                return Hand.GetRange(1, Hand.Count);
            }
        }

        public override int CurrentHandScore
        {
            get
            {
                return VisibleHand.Sum(x => x.CardScore) + 10;
            }
        }

        public override void Play()
        {
            //lower than 17, dealer hits until they hit 17
            if (CurrentHandScore < 17)
                Hit();
            else
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
                    DealToPlayer(Game.Players[j]);
                }
                DealToDealer();
                i++;
            }
        }

        public void HandleInsuranceBets()
        {
            //if your score is 20 you should take the insurance bet
            foreach (Player player in Game.Players)
            {
                if (player.GetType() == typeof(Counter))
                {
                    //add logic for counting strategy here later
                }
                else if (player.CurrentHandScore == 20)
                {
                    player.TakeInsuranceBet = true;
                }
            }
        }

        public void DealToPlayer(Player player)
        {
            player.Hand.Add(Game.Deck.Cards[Game.Deck.remainingCards - 1]);
            Game.Deck.Cards.RemoveAt(Game.Deck.remainingCards - 1);

            Game.UpdateCount(player.Hand.Last());
        }

        private void DealToDealer()
        {
            Hand.Add(Game.Deck.Cards[Game.Deck.remainingCards - 1]);
            Game.Deck.Cards.RemoveAt(Game.Deck.remainingCards - 1);

            if (Hand.Count > 1) //first card is not shown 
            {
                Game.UpdateCount(UpCard);
            }

            if (UpCard.FaceValue == FaceValue.Ace)
            {
                HandleInsuranceBets();
            }
        }

        public override void Hit()
        {
            DealToDealer();
            Play();
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
