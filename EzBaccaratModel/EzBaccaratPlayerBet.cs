namespace EzBaccarat.Model
{
    public class EzBaccaratPlayerBet
    {
        public Player Player { get; set; }

        public int Panda8Bet { get; set; }
        public int Dragon7Bet { get; set; }
        public int TieBet { get; set; }
        public int BankerBet { get; set; }
        public int PlayerBet { get; set; }
    }
}