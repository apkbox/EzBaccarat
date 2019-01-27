using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat.Model
{
    public class Shoe
    {
        public List<Card> cards = new List<Card>();

        public void Load(IEnumerable<Card> cards, int deckCount = 1)
        {
            for (var i = 0; i < deckCount; i++)
            {
                this.cards.AddRange(cards);
            }
        }

        public Card Draw()
        {
            Card card = this.cards[0];
            this.cards.RemoveAt(0);
            return card;
        }
    }
}
