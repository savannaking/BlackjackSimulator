using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Model.Actors
{
    public class Counter : Player
    {
        public Counter(Game game, decimal bet) : base(game, bet)
        {
        }

        public override void Play(int i)
        {
        }
    }
}
