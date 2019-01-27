using EzBaccarat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccaratTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new EzBaccaratTable();

            for (var z = 0; z < 5; ++z)
            {
                table.InitializeGame();

                EzBaccaratPlayerBet bet = new EzBaccaratPlayerBet();
                bet.Player = new Player();
                bet.Player.Put(4000);

                bool betLost = false;
                int lastBet = 25;
                int maxBet = 0;
                int rounds = 0;

                int bankerWins = 0;
                int playerWins = 0;
                int ties = 0;

                while (true)
                {
                    if (betLost)
                    {
                        lastBet *= 2;
                        betLost = false;
                    }

                    lastBet = bet.Player.TryGet(lastBet);
                    maxBet = Math.Max(maxBet, lastBet);
                    bet.BankerBet = lastBet;
                    bet.Dragon7Bet = bet.Player.TryGet(5);
                    bet.Panda8Bet = bet.Player.TryGet(5);
                    table.AddBet(bet);

                    if (!table.PlayRound())
                        break;

                    ++rounds;

                    string outcome = "?????";
                    if (table.Dealer.IsTie)
                    {
                        outcome = " TIE ";
                        ++ties;
                    }

                    if (table.Dealer.IsPlayerWin)
                    {
                        outcome = "<==  ";
                        ++playerWins;
                    }

                    if (table.Dealer.IsBankerWin)
                    {
                        outcome = "  ==>";
                        ++bankerWins;
                    }
                    else
                    {
                        betLost = true;
                    }

                    if (table.Dealer.IsDragon7)
                    {
                        outcome = "  =>7";
                    }

                    if (table.Dealer.IsPanda8)
                    {
                        outcome = "8<=  ";
                    }

                    Console.WriteLine("Player: {0} ({1})    {2}    Banker: {3} ({4})", PrintHand(table.Dealer.PlayerHand), table.Dealer.PlayerPoints,
                        outcome, PrintHand(table.Dealer.BankerHand), table.Dealer.BankerPoints);

                    /*
                    if (bet.Player.Money <= 0)
                    {
                        Console.WriteLine("BANKRUPT!!!!!");
                        break;
                    }
                    */
                }

                table.FinalizeGame();

                Console.WriteLine("Rounds: {0}", rounds);
                Console.WriteLine("Player: {0} ({1:0.0%}), Banker: {2} ({3:0.0%}), Tie: {4} ({5:0.0%})", playerWins, (double)playerWins / rounds,
                    bankerWins, (double)bankerWins / rounds, ties, (double)ties / rounds);
                Console.WriteLine("Gamble money: {0}, Max bet: {1}", bet.Player.Money, maxBet);
            }
        }

        private static string PrintHand(List<Card> cards)
        {
            string s = string.Empty;

            if (cards.Count == 0)
                return s;

            var i = 0;
            for (; i < cards.Count - 1; i++)
            {
                s += cards[i].ToString() + " ";
            }

            s += cards[i].ToString();

            if (i < 2)
            {
                s += "    ";
            }

            return s;
        }
    }
}
