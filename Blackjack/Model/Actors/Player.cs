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
        //will add this when adding the ability to give players different bets, for now
        //all players make the same bet for every round
        //public int CurrentBet { get; set; }
        public int WinCount { get; set; }
        public decimal CurrentWinnings { get; set; }
        public decimal TotalWinnings { get; set; }

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

        public Player(Game game)
        {
            Hand = new List<Card>();
        }

        public virtual void Play()
        {
            switch (CurrentHandScore)
            {
                case int score when score < 10:
                    Hit();
                    break;
                case int score when score == 10 || score == 11:
                    if (Game.Dealer.VisibleHand.Any(x => x.FaceValue == FaceValue.Ace)) //dealer's upturned card is Ace
                        Stay(); //everyone has to stay - move this, probably 
                    else
                        DoubleDown(); //unless dealer has an ace
                    break;
                case int score when score >= 12 && score <= 16:
                    if (Game.Dealer.CurrentHandScore >= 2 && Game.Dealer.CurrentHandScore <= 6)
                        Stay();
                    else if (Game.Dealer.CurrentHandScore >= 7)
                        Hit();
                    break;
                case int score when score >= 17:
                    Stay();
                    break;
            }
        }

        public void DoubleDown()
        {
            //dealer deals one more card
            //double your bet
        }

        public virtual void Hit()
        {
            Game.Dealer.DealToPlayer(this);
            Play();
        }

        public void Stay()
        {
            //method should stay for terminology/readability's sake, but the action here is to do nothing
        }
    }
}
