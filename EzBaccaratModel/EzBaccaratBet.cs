namespace EzBaccarat.Model
{
    public class EzBaccaratBet
    {
        public EzBaccaratBet()
        {
        }

        public EzBaccaratBet(EzBaccaratBet other)
        {
            this.Gambler = other.Gambler;

            this.Panda = other.Panda;
            this.Dragon = other.Dragon;
            this.Tie = other.Tie;
            this.Banker = other.Banker;
            this.Player = other.Player;
        }

        public Gambler Gambler { get; set; }

        public int Panda { get; set; }
        public int Dragon { get; set; }
        public int Tie { get; set; }
        public int Banker { get; set; }
        public int Player { get; set; }
    }
}