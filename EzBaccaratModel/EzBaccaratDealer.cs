using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat.Model
{
    public class EzBaccaratDealer
    {
        private readonly int[] CardPointValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0 };

        private Shoe shoe;

        private bool noMoreDeals = false;
        private bool seenCutCard = false;

        public List<Card> BankerHand { get; } = new List<Card>();

        public List<Card> PlayerHand { get; } = new List<Card>();

        public int BankerPoints { get; private set; }

        public int PlayerPoints { get; private set; }

        public bool IsBankerWin { get; private set; }

        public bool IsPlayerWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsPush { get; private set; }

        public bool IsPanda8 { get; private set; }

        public bool IsDragon7 { get; private set; }

        public bool IsNatural { get; private set; }

        public EzBaccaratDealer(Shoe shoe)
        {
            this.shoe = shoe;
        }

        public bool Deal()
        {
            if (noMoreDeals)
                return false;

            if (seenCutCard)
                noMoreDeals = true;

            PlayerHand.Clear();
            BankerHand.Clear();
            PlayerPoints = 0;
            BankerPoints = 0;

            IsBankerWin = false;
            IsPlayerWin = false;
            IsTie = false;
            IsPush = false;

            IsPanda8 = false;
            IsDragon7 = false;

            IsNatural = false;

            PlayerHand.Add(this.DrawCard());
            // TODO: Check whether we need to declare last round when first drawn card is a cutcard.
            BankerHand.Add(this.DrawCard());
            PlayerHand.Add(this.DrawCard());
            BankerHand.Add(this.DrawCard());

            PlayerPoints += CardPointValues[(int)PlayerHand[0].Rank];
            PlayerPoints += CardPointValues[(int)PlayerHand[1].Rank];
            PlayerPoints %= 10;

            BankerPoints += CardPointValues[(int)BankerHand[0].Rank];
            BankerPoints += CardPointValues[(int)BankerHand[1].Rank];
            BankerPoints %= 10;

            if (PlayerPoints == 8 || PlayerPoints == 9 || BankerPoints == 8 || BankerPoints == 9)
            {
                IsNatural = true;
            }
            else
            {
                bool playerStands = false;
                if (PlayerPoints <= 5)
                {
                    PlayerHand.Add(this.DrawCard());
                    PlayerPoints += CardPointValues[(int)PlayerHand[2].Rank];
                    PlayerPoints %= 10;
                }
                else
                {
                    playerStands = true;
                }

                if (playerStands)
                {
                    if (BankerPoints <= 5)
                    {
                        BankerHand.Add(this.DrawCard());
                        BankerPoints += CardPointValues[(int)BankerHand[2].Rank];
                        BankerPoints %= 10;
                    }
                }
                else
                {
                    // Player drew a third card.
                    int playersThirdCard = CardPointValues[(int)PlayerHand[2].Rank];
                    bool bankerDraws = false;

                    if ((BankerPoints <= 2) || (BankerPoints == 3 && playersThirdCard != 8) ||
                        (BankerPoints == 4 && (playersThirdCard >= 2 && playersThirdCard <= 7)) ||
                        (BankerPoints == 5 && (playersThirdCard >= 4 && playersThirdCard <= 7)) ||
                        (BankerPoints == 6 && (playersThirdCard == 6 || playersThirdCard == 7)))
                    {
                        bankerDraws = true;
                    }

                    if (bankerDraws)
                    {
                        BankerHand.Add(this.DrawCard());
                        BankerPoints += CardPointValues[(int)BankerHand[2].Rank];
                        BankerPoints %= 10;
                    }
                }
            }

            if (BankerPoints > PlayerPoints)
            {
                IsBankerWin = true;
                if (BankerPoints == 7 && BankerHand.Count == 3)
                {
                    this.IsDragon7 = true;

                    this.IsBankerWin = false;
                    this.IsPush = true;
                }
            }
            else if (BankerPoints < PlayerPoints)
            {
                IsPlayerWin = true;
                if (PlayerPoints == 8 && PlayerHand.Count == 3)
                    this.IsPanda8 = true;
            }
            else
            {
                IsTie = true;
            }

            return true;
        }

        private Card DrawCard()
        {
            var card = shoe.Draw();
            if (card.Suit == CardSuit.CutCard)
            {
                this.seenCutCard = true;
                card = shoe.Draw();
            }

            return card;
        }
    }
}
