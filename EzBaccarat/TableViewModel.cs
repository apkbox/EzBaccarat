using EzBaccarat.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public string PlayerHand1 { get { return table.Dealer.PlayerHand.Count > 0 ? table.Dealer.PlayerHand[0].CardCode : string.Empty; } }
        public string PlayerHand2 { get { return table.Dealer.PlayerHand.Count > 0 ? table.Dealer.PlayerHand[1].CardCode : string.Empty; } }
        public string PlayerHand3 { get { return table.Dealer.PlayerHand.Count > 2 ? table.Dealer.PlayerHand[2].CardCode : string.Empty; } }

        public string BankerHand1 { get { return table.Dealer.BankerHand.Count > 0 ? table.Dealer.BankerHand[0].CardCode : string.Empty; } }
        public string BankerHand2 { get { return table.Dealer.BankerHand.Count > 0 ? table.Dealer.BankerHand[1].CardCode : string.Empty; } }
        public string BankerHand3 { get { return table.Dealer.BankerHand.Count > 2 ? table.Dealer.BankerHand[2].CardCode : string.Empty; } }

        public bool PlayerWin { get { return table.Dealer.IsPlayerWin || table.Dealer.IsTie; } }

        public bool BankerWin { get { return table.Dealer.IsBankerWin || table.Dealer.IsTie || table.Dealer.IsBankerPush; } }

        public int PlayerBankRoll { get { return player.Money; } }

        public string PandaBetChips { get { return GetChipImage(this.PandaBet); } }
        public string TieBetChips { get { return GetChipImage(this.TieBet); } }
        public string DragonBetChips { get { return GetChipImage(this.DragonBet); } }

        public string BankerBetChips { get { return GetChipImage(this.BankerBet); } }
        public string PlayerBetChips { get { return GetChipImage(this.PlayerBet); } }

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

            foreach(var payout in this.table.Payouts)
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
