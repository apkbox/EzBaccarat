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

        private List<EzBaccaratBet> bets = new List<EzBaccaratBet>();
        private List<EzBaccaratPayout> payouts = new List<EzBaccaratPayout>();

        public EzBaccaratTableState CurrentState { get; private set; } = EzBaccaratTableState.NotReady;

        public EzBaccaratDealer Dealer { get; private set; }

        public IList<EzBaccaratBet> Bets { get { return bets; } }
        public IList<EzBaccaratPayout> Payouts { get { return payouts; } }


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

        public void GoNextState()
        {
            if (this.CurrentState == EzBaccaratTableState.NotReady)
            {
                this.InitializeGame();
                this.CurrentState = EzBaccaratTableState.WaitingForBets;
            }
            else if (CurrentState == EzBaccaratTableState.WaitingForBets)
            {
                if (!this.Deal())
                    this.CurrentState = EzBaccaratTableState.GameFinished;
                else
                    this.CurrentState = EzBaccaratTableState.PayoutReady;
            }
            else if (CurrentState == EzBaccaratTableState.PayoutReady)
            {
                this.EndRound();
                if (this.Dealer.LastDeal)
                    this.CurrentState = EzBaccaratTableState.GameFinished;
                else
                    this.CurrentState = EzBaccaratTableState.WaitingForBets;
            }
            else if (CurrentState == EzBaccaratTableState.GameFinished)
            {
                this.FinalizeGame();
                this.CurrentState = EzBaccaratTableState.NotReady;
            }
        }

        private void InitializeGame()
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

            this.bets.Clear();
            this.payouts.Clear();
        }

        /// <summary>
        /// With bets in place, draws and calculates payout based on results.
        /// </summary>
        /// <returns></returns>
        private bool Deal()
        {
            Debug.Assert(this.payouts.Count == 0);

            if (!this.Dealer.Deal())
                return false;

            foreach (var bet in bets)
            {
                var payout = new EzBaccaratPayout(bet);
                payouts.Add(payout);

                if (this.Dealer.IsTie)
                {
                    System.Diagnostics.Debug.Assert(!this.Dealer.IsBankerWin && !this.Dealer.IsPlayerWin);
                    payout.Bet.Tie = bet.Tie;
                    payout.Tie = bet.Tie * 8;

                    payout.Bet.Banker = bet.Banker;
                    payout.Bet.Player = bet.Player;
                }
                else
                {
                    if (this.Dealer.IsBankerWin)
                    {
                        System.Diagnostics.Debug.Assert(!this.Dealer.IsPlayerWin);
                        payout.Bet.Banker = bet.Banker;
                        payout.Banker = bet.Banker;
                    }

                    if (this.Dealer.IsPlayerWin)
                    {
                        System.Diagnostics.Debug.Assert(!this.Dealer.IsBankerWin);
                        payout.Bet.Player = bet.Player;
                        payout.Player = bet.Player;
                    }
                }

                if (this.Dealer.IsBankerPush)
                {
                    payout.Bet.Banker = bet.Banker;
                }

                if (this.Dealer.IsDragon)
                {
                    payout.Bet.Dragon = bet.Dragon;
                    payout.Dragon = bet.Dragon * 40;
                }

                if (this.Dealer.IsPanda)
                {
                    payout.Bet.Panda = bet.Panda;
                    payout.Panda = bet.Panda * 25;
                }
            }

            return true;
        }

        private void EndRound()
        {
            bets.Clear();
            payouts.Clear();

            discardTray.AddRange(this.Dealer.PlayerHand);
            discardTray.AddRange(this.Dealer.BankerHand);
        }

        private void FinalizeGame()
        {
            shuffler.Shuffle(cardSets[currentCardSet]);
            currentCardSet = (currentCardSet + 1) % cardSets.Length;
            discardTray.Clear();
            this.Dealer = null;
        }
    }
}
