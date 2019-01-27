namespace EzBaccarat.Model
{
    internal class French52Deck
    {
        private readonly Card[] cards = {
            new Card(CardSuit.Clubs, CardRank.Ace),
            new Card(CardSuit.Clubs, CardRank.Two),
            new Card(CardSuit.Clubs, CardRank.Three),
            new Card(CardSuit.Clubs, CardRank.Four),
            new Card(CardSuit.Clubs, CardRank.Five),
            new Card(CardSuit.Clubs, CardRank.Six),
            new Card(CardSuit.Clubs, CardRank.Seventh),
            new Card(CardSuit.Clubs, CardRank.Eight),
            new Card(CardSuit.Clubs, CardRank.Nine),
            new Card(CardSuit.Clubs, CardRank.Ten),
            new Card(CardSuit.Clubs, CardRank.Jack),
            new Card(CardSuit.Clubs, CardRank.Queen),
            new Card(CardSuit.Clubs, CardRank.King),

            new Card(CardSuit.Diamonds, CardRank.Ace),
            new Card(CardSuit.Diamonds, CardRank.Two),
            new Card(CardSuit.Diamonds, CardRank.Three),
            new Card(CardSuit.Diamonds, CardRank.Four),
            new Card(CardSuit.Diamonds, CardRank.Five),
            new Card(CardSuit.Diamonds, CardRank.Six),
            new Card(CardSuit.Diamonds, CardRank.Seventh),
            new Card(CardSuit.Diamonds, CardRank.Eight),
            new Card(CardSuit.Diamonds, CardRank.Nine),
            new Card(CardSuit.Diamonds, CardRank.Ten),
            new Card(CardSuit.Diamonds, CardRank.Jack),
            new Card(CardSuit.Diamonds, CardRank.Queen),
            new Card(CardSuit.Diamonds, CardRank.King),

            new Card(CardSuit.Hearts, CardRank.Ace),
            new Card(CardSuit.Hearts, CardRank.Two),
            new Card(CardSuit.Hearts, CardRank.Three),
            new Card(CardSuit.Hearts, CardRank.Four),
            new Card(CardSuit.Hearts, CardRank.Five),
            new Card(CardSuit.Hearts, CardRank.Six),
            new Card(CardSuit.Hearts, CardRank.Seventh),
            new Card(CardSuit.Hearts, CardRank.Eight),
            new Card(CardSuit.Hearts, CardRank.Nine),
            new Card(CardSuit.Hearts, CardRank.Ten),
            new Card(CardSuit.Hearts, CardRank.Jack),
            new Card(CardSuit.Hearts, CardRank.Queen),
            new Card(CardSuit.Hearts, CardRank.King),

            new Card(CardSuit.Spades, CardRank.Ace),
            new Card(CardSuit.Spades, CardRank.Two),
            new Card(CardSuit.Spades, CardRank.Three),
            new Card(CardSuit.Spades, CardRank.Four),
            new Card(CardSuit.Spades, CardRank.Five),
            new Card(CardSuit.Spades, CardRank.Six),
            new Card(CardSuit.Spades, CardRank.Seventh),
            new Card(CardSuit.Spades, CardRank.Eight),
            new Card(CardSuit.Spades, CardRank.Nine),
            new Card(CardSuit.Spades, CardRank.Ten),
            new Card(CardSuit.Spades, CardRank.Jack),
            new Card(CardSuit.Spades, CardRank.Queen),
            new Card(CardSuit.Spades, CardRank.King),
        };

        public Card[] GetCards() { return cards; }
    }
}