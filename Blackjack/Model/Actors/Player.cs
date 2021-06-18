using Blackjack.Model;
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
        public Game Game { get; set; }
        public bool TakeInsuranceBet { get; set; }
        public int WinCount { get; set; }
        public decimal CurrentWinnings { get; set; }
        public decimal TotalChipsValue { get; set; }
        public decimal Bet { get; set; }

        public bool Busted { 
            get
            {
                return CurrentHandScore > 21;
            }
        }

        public virtual int CurrentHandScore
        {
            get
            {
                if (!Hand.Any(x => x.FaceValue == FaceValue.Ace)) {
                    return Hand.Sum(x => x.CardScore);
                }
                else
                {
                    if (CurrentHandScore > 21)
                        return Hand.Sum(x => x.CardScore) - 10; //ace card acts as a one instead
                    else 
                        return Hand.Sum(x => x.CardScore);
                }
            }
        }

        public bool HasBlackjack
        {
            get
            {
                return Hand.Any(x => x.FaceValue == FaceValue.Ace)
                    && Hand.Any(x => x.FaceValue != FaceValue.Ace && (int)x.FaceValue == 10);
            }
        }

        public Player(Game game, decimal bet)
        {
            Hand = new List<Card>();
            Bet = bet;
            TotalChipsValue = 500;
            Game = game;
        }

        public virtual void Play(int i)
        {
            switch (CurrentHandScore)
            {
                case int score when score < 10:
                    Hit(i);
                    break;
                case int score when score == 10 || score == 11:
                    if (Game.Dealer.VisibleHand.Any(x => x.FaceValue == FaceValue.Ace)) //dealer's upturned card is Ace
                        Stay(i); //everyone has to stay - move this, probably 
                    else
                        //DoubleDown(); //unless dealer has an ace
                        Hit(i); //for now
                    break;
                case int score when score >= 12 && score <= 16:
                    if (Game.Dealer.CurrentHandScore >= 2 && Game.Dealer.CurrentHandScore <= 6)
                        Stay(i);
                    else if (Game.Dealer.CurrentHandScore >= 7)
                        Hit(i);
                    break;
                case int score when score >= 17:
                    Stay(i);
                    break;
            }
        }

        public void DoubleDown()
        {
            //dealer deals one more card
            //double your bet
        }

        public virtual void Hit(int i)
        {
            Console.WriteLine(String.Format("Player {0} hit", i));
            Game.Dealer.DealToPlayer(this, i);
            Play(i);
        }

        public virtual void Stay(int i)
        {
            Console.WriteLine(String.Format("Player {0} stays", i));
        }

        public void Reset()
        {
            Hand = new List<Card>();
            TakeInsuranceBet = false;
            CurrentWinnings = 0;
        }
    }
}
