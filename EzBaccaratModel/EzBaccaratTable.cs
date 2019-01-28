using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat.Model
{
    public class EzBaccaratTable
    {
        private readonly Shuffler shuffler = new FisherYatesShuffler();
        private Shoe shoe = new Shoe();

        private int currentCardSet = 0;

        private List<Card>[] cardSets = { new List<Card>(), new List<Card>() };
        private List<Card> discardTray = new List<Card>();

        private List<EzBaccaratPlayerBet> bets = new List<EzBaccaratPlayerBet>();

        public EzBaccaratDealer Dealer { get; private set; }

        public EzBaccaratTable()
        {
            // Create an initial deck (8x52 decks)
            var frenchDeck = new French52Deck();

            for (var i = 0; i < 8; i++)
            {
                this.cardSets[0].AddRange(frenchDeck.GetCards());
                this.cardSets[1].AddRange(frenchDeck.GetCards());
            }

            for (var i = 0; i < cardSets.Length; i++)
            {
                shuffler.Shuffle(this.cardSets[i]);
            }

            if (false)
            {
                foreach (var z in this.cardSets[0])
                {
                    Console.WriteLine(z.ToString());
                }
            }

            currentCardSet = 0;
        }

        public void AddBet(EzBaccaratPlayerBet bet)
        {
            bets.Add(bet);
        }

        public void InitializeGame()
        {
            this.discardTray.Clear();

            // TODO: Check that discard tray contains correct number of cards
            var deck = new List<Card>(cardSets[currentCardSet]);

            // Cut
            // TODO: Move index +/-10 cards randomly
            var index = deck.Count / 2;
            var half = deck.GetRange(0, index);
            deck.RemoveRange(0, index);
            deck.AddRange(half);

            // TODO: Player cut (random > 52)
            deck.Insert(deck.Count - 55, Card.CutCard);

            // Load shoe
            shoe.Load(deck);

            // Burn cards (TODO: There are different rules. Check with casino).
            var card = shoe.Draw();
            discardTray.Add(card);

            if (card.Rank > CardRank.Ace)
            {
                var burnCount = (((int)card.Rank) - 1) % 10;

                for (var i = 0; i < burnCount; i++)
                {
                    discardTray.Add(shoe.Draw());
                }
            }

            // Create a dealer
            this.Dealer = new EzBaccaratDealer(shoe);
        }

        public void FinalizeGame()
        {
            shuffler.Shuffle(cardSets[currentCardSet]);
            currentCardSet = (currentCardSet + 1) % cardSets.Length;
            discardTray.Clear();
            this.Dealer = null;
        }

        public bool PlayRound()
        {
            if (!this.Dealer.Deal())
                return false;

            foreach (var bet in bets)
            {
                if (this.Dealer.IsTie)
                {
                    System.Diagnostics.Debug.Assert(!this.Dealer.IsBankerWin && !this.Dealer.IsPlayerWin);

                    bet.Player.Put(bet.TieBet * 8);
                    bet.Player.Put(bet.BankerBet);
                    bet.Player.Put(bet.PlayerBet);
                }
                else
                {
                    if (this.Dealer.IsBankerWin)
                    {
                        System.Diagnostics.Debug.Assert(!this.Dealer.IsPlayerWin);
                        bet.Player.Put(bet.BankerBet * 2);
                    }

                    if (this.Dealer.IsPlayerWin)
                    {
                        System.Diagnostics.Debug.Assert(!this.Dealer.IsBankerWin);
                        bet.Player.Put(bet.PlayerBet * 2);
                    }
                }

                if (this.Dealer.IsPush)
                {
                    bet.Player.Put(bet.BankerBet);
                }

                if (this.Dealer.IsDragon7)
                {
                    bet.Player.Put(bet.Dragon7Bet * 40);
                }

                if (this.Dealer.IsPanda8)
                {
                    bet.Player.Put(bet.Dragon7Bet * 25);
                }
            }

            discardTray.AddRange(this.Dealer.PlayerHand);
            discardTray.AddRange(this.Dealer.BankerHand);

            bets.Clear();

            return true;
        }
    }
}
