namespace EzBaccarat.Model
{
    public class EzBaccaratScoreItem
    {
        public EzBaccaratWinningHand WinningHand { get; set; }

        public bool IsPlayerNatural { get; set; }
        public bool IsBankerNatural { get; set; }
        public bool IsNatural { get { return IsPlayerNatural || IsBankerNatural; } }
        public bool IsTie { get; set; }

        public bool IsDragon { get; set; }
        public bool IsPanda { get; set; }
    }
}
