namespace EzBaccarat.Model
{
    public class EzBaccaratPayout
    {
        public EzBaccaratPayout(EzBaccaratBet originalBet)
        {
            this.OriginalBet = originalBet;

            this.Bet = new EzBaccaratBet();
            this.Bet.Gambler = this.OriginalBet.Gambler;
        }

        /// <summary>
        /// Original bet.
        /// </summary>
        public EzBaccaratBet OriginalBet { get; }

        /// <summary>
        /// Bet after game round. Winning, push and tie bets remain, while lost bets are zeroed.
        /// </summary>
        public EzBaccaratBet Bet { get; private set; }

        public int Panda { get; set; }
        public int TotalPanda { get { return this.Bet.Panda + this.Panda; } }

        public int Dragon { get; set; }
        public int TotalDragon { get { return this.Bet.Dragon + this.Dragon; } }

        public int Tie { get; set; }
        public int TotalTie { get { return this.Bet.Tie + this.Tie; } }

        public int Banker { get; set; }
        public int TotalBanker { get { return this.Bet.Banker + this.Banker; } }

        public int Player { get; set; }
        public int TotalPlayer { get { return this.Bet.Player + this.Player; } }

        public int TotalWin
        {
            get
            {
                return this.TotalPanda + this.TotalDragon + this.TotalTie + this.TotalBanker + this.TotalPlayer;
            }
        }
    }
}