namespace EzBaccarat.Model
{
    public struct Card
    {
        public static readonly Card CutCard = new Card(CardSuit.CutCard, CardRank.CutCard);

        public Card(CardSuit suit, CardRank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        private CardSuit suit;
        private CardRank rank;

        public CardSuit Suit { get => suit; }
        public CardRank Rank { get => rank; }

        public override string ToString()
        {
            string rankChar = "  ";
            switch(rank)
            {
                case CardRank.Ace:
                    rankChar = " A";
                    break;
                case CardRank.Two:
                    rankChar = " 2";
                    break;
                case CardRank.Three:
                    rankChar = " 3";
                    break;
                case CardRank.Four:
                    rankChar = " 4";
                    break;
                case CardRank.Five:
                    rankChar = " 5";
                    break;
                case CardRank.Six:
                    rankChar = " 6";
                    break;
                case CardRank.Seventh:
                    rankChar = " 7";
                    break;
                case CardRank.Eight:
                    rankChar = " 8";
                    break;
                case CardRank.Nine:
                    rankChar = " 9";
                    break;
                case CardRank.Ten:
                    rankChar = "10";
                    break;
                case CardRank.Jack:
                    rankChar = " J";
                    break;
                case CardRank.Queen:
                    rankChar = " Q";
                    break;
                case CardRank.King:
                    rankChar = " K";
                    break;
            }

            string suitChar = " ";
            switch (suit)
            {
                case CardSuit.Clubs:
                    suitChar = "\u2663";
                    break;
                case CardSuit.Diamonds:
                    suitChar = "\u2666";
                    break;
                case CardSuit.Hearts:
                    suitChar = "\u2665";
                    break;
                case CardSuit.Spades:
                    suitChar = "\u2660";
                    break;
            }

            return rankChar + "" + suitChar;
        }
    }
}