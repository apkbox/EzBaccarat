using EzBaccarat.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat
{
    class TableViewModel : INotifyPropertyChanged
    {
        private EzBaccaratTable table = new Model.EzBaccaratTable();
        private Gambler player = new Gambler();

        private EzBaccaratScoreboardBigRoad bigRoadScoreboard = new EzBaccaratScoreboardBigRoad();

        public event PropertyChangedEventHandler PropertyChanged;

        public int PlayerHandScore { get { return table.Dealer.PlayerPoints; } }
        public int BankerHandScore { get { return table.Dealer.BankerPoints; } }

        public string PlayerHand1 { get { return table.Dealer.PlayerHand.Count > 0 ? table.Dealer.PlayerHand[0].CardCode : string.Empty; } }
        public string PlayerHand2 { get { return table.Dealer.PlayerHand.Count > 0 ? table.Dealer.PlayerHand[1].CardCode : string.Empty; } }
        public string PlayerHand3 { get { return table.Dealer.PlayerHand.Count > 2 ? table.Dealer.PlayerHand[2].CardCode : string.Empty; } }

        public string BankerHand1 { get { return table.Dealer.BankerHand.Count > 0 ? table.Dealer.BankerHand[0].CardCode : string.Empty; } }
        public string BankerHand2 { get { return table.Dealer.BankerHand.Count > 0 ? table.Dealer.BankerHand[1].CardCode : string.Empty; } }
        public string BankerHand3 { get { return table.Dealer.BankerHand.Count > 2 ? table.Dealer.BankerHand[2].CardCode : string.Empty; } }

        public bool PlayerWin { get { return table.Dealer.IsPlayerWin || table.Dealer.IsTie; } }
        public bool BankerWin { get { return table.Dealer.IsBankerWin || table.Dealer.IsTie || table.Dealer.IsBankerPush; } }

        public int Shoe { get; private set; }
        public double Dragon7Count1 { get; private set; }
        public int Dragon7Count2 { get; private set; }

        public int Panda8Count { get; private set; }
        public int TieCount { get; private set; }
        public int Dragon7Count { get; private set; }
        public int PlayerCount { get; private set; }
        public int BankerCount { get; private set; }

        public int PlayerBankRoll { get { return player.Money; } }

        public string PandaBetChips { get { return GetChipImage(this.PandaBet); } }
        public string TieBetChips { get { return GetChipImage(this.TieBet); } }
        public string DragonBetChips { get { return GetChipImage(this.DragonBet); } }

        public string BankerBetChips { get { return GetChipImage(this.BankerBet); } }
        public string PlayerBetChips { get { return GetChipImage(this.PlayerBet); } }

        private ObservableCollection<EzBaccaratScoreItem> bigRoadItems = new ObservableCollection<EzBaccaratScoreItem>();

        public IList<EzBaccaratScoreItem> BigRoadScoreboard
        {
            get
            {
                return bigRoadItems;
            }
        }

        private string GetChipImage(int bet)
        {
            if (bet == 0)
                return "";
            else if (bet < 5)
                return "1";
            else if (bet < 25)
                return "5";
            else if (bet < 100)
                return "25";
            else if (bet < 500)
                return "100";
            else if (bet < 1000)
                return "500";
            else
                return "1000";
        }

        public TableViewModel()
        {
            table.GoNextState();

            player.Put(1000);

            this.PandaBetCommand = new DelegateCommand(OnPandaBetCommand);
            this.TieBetCommand = new DelegateCommand(OnTieBetCommand);
            this.DragonBetCommand = new DelegateCommand(OnDragonBetCommand);
            this.BankerBetCommand = new DelegateCommand(OnBankerBetCommand);
            this.PlayerBetCommand = new DelegateCommand(OnPlayerBetCommand);

            this.DealCommand = new DelegateCommand(OnDealCommand);
        }

        public DelegateCommand PandaBetCommand { get; private set; }
        public DelegateCommand TieBetCommand { get; private set; }
        public DelegateCommand DragonBetCommand { get; private set; }
        public DelegateCommand BankerBetCommand { get; private set; }
        public DelegateCommand PlayerBetCommand { get; private set; }

        public DelegateCommand DealCommand { get; private set; }

        public int PandaBet { get; private set; }
        public int TieBet { get; private set; }
        public int DragonBet { get; private set; }
        public int BankerBet { get; private set; }
        public int PlayerBet { get; private set; }

        private void OnPandaBetCommand()
        {
            PandaBet += player.TryGet(25);
            InvokePropertyChanged("PandaBet");
            InvokePropertyChanged("PandaBetChips");
        }

        private void OnTieBetCommand()
        {
            TieBet += player.TryGet(25);
            InvokePropertyChanged("TieBet");
            InvokePropertyChanged("TieBetChips");
        }

        private void OnPlayerBetCommand()
        {
            PlayerBet += player.TryGet(25);
            InvokePropertyChanged("PlayerBet");
            InvokePropertyChanged("PlayerBetChips");
        }

        private void OnBankerBetCommand()
        {
            BankerBet += player.TryGet(25);
            InvokePropertyChanged("BankerBet");
            InvokePropertyChanged("BankerBetChips");
        }

        private void OnDragonBetCommand()
        {
            DragonBet += player.TryGet(25);
            InvokePropertyChanged("DragonBet");
            InvokePropertyChanged("DragonBetChips");
        }

        private void OnDealCommand()
        {
            if (table.CurrentState != EzBaccaratTableState.WaitingForBets)
            {
                return;
            }

            var bet = new EzBaccaratBet();
            bet.Gambler = player;
            bet.Panda = this.PandaBet;
            bet.Tie = this.TieBet;
            bet.Dragon = this.DragonBet;
            bet.Banker = this.BankerBet;
            bet.Player = this.PlayerBet;

            table.Bets.Add(bet);
            table.GoNextState();

            ++this.Shoe;
            if (this.table.Dealer.IsPanda)
                ++this.Panda8Count;

            if (this.table.Dealer.IsTie)
            {
                ++this.TieCount;
                bigRoadScoreboard.AddScore(new EzBaccaratScoreItem() { WinningHand = EzBaccaratWinningHand.None, IsTie = true });
            }

            if (this.table.Dealer.IsDragon)
                ++this.Dragon7Count;

            if (this.table.Dealer.IsPlayerWin)
            {
                ++this.PlayerCount;
                bigRoadScoreboard.AddScore(new EzBaccaratScoreItem() { WinningHand = EzBaccaratWinningHand.Player });
            }

            if (this.table.Dealer.IsBankerWin)
            {
                ++this.BankerCount;
                bigRoadScoreboard.AddScore(new EzBaccaratScoreItem() { WinningHand = EzBaccaratWinningHand.Banker });
            }

            this.bigRoadItems.Clear();
            for (var i = 0; i < bigRoadScoreboard.Items.Count; i++)
            {
                this.bigRoadItems.Add(bigRoadScoreboard.Items[i]);
            }

            UpdateDragon7Count();

            foreach (var payout in this.table.Payouts)
            {
                payout.Bet.Gambler.Put(payout.TotalWin);
            }

            this.PandaBet = 0;
            this.TieBet = 0;
            this.DragonBet = 0;
            this.BankerBet = 0;
            this.PlayerBet = 0;

            table.GoNextState();

            InvokePropertyChanged(string.Empty);
        }

        private void UpdateDragon7Count()
        {
            foreach (var c in table.Dealer.PlayerHand)
            {
                this.Dragon7Count1 += GetDragon7Weight(c);

                if (c.RankValue > 3 && c.RankValue < 8)
                    Dragon7Count2 -= 1;
                else if (c.RankValue == 8 || c.RankValue == 9)
                    Dragon7Count2 += 2;
            }

            foreach (var c in table.Dealer.BankerHand)
            {
                this.Dragon7Count1 += GetDragon7Weight(c);

                if (c.RankValue > 3 && c.RankValue < 8)
                    Dragon7Count2 -= 1;
                else if (c.RankValue == 8 || c.RankValue == 9)
                    Dragon7Count2 += 2;
            }

            InvokePropertyChanged("Dragon7Count1");
            InvokePropertyChanged("Dragon7Count2");
        }

        private double GetDragon7Weight(Card c)
        {
            switch (c.RankValue)
            {
                case 1:
                    return -0.5;
                case 2:
                    return -0.9;
                case 3:
                    return -1.1;
                case 4:
                    return -2.7;
                case 5:
                    return -2.7;
                case 6:
                    return -3.3;
                case 7:
                    return -3.6;
                case 8:
                    return 5.4;
                case 9:
                    return 4.8;
                case 10:
                    return 0.9;
            }

            return 0;
        }

        public void InvokePropertyChanged([CallerMemberName] string name = "")
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
