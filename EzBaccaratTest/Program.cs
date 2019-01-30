using EzBaccarat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Debug.Assert(table.CurrentState == EzBaccaratTableState.NotReady);

                table.GoNextState();
                Debug.Assert(table.CurrentState == EzBaccaratTableState.WaitingForBets);

                EzBaccaratBet bet = new EzBaccaratBet();
                bet.Gambler = new Gambler();
                bet.Gambler.Put(4000);

                bool betLost = false;
                int lastBet = 25;
                int maxBet = 0;
                int rounds = 0;

                int bankerWins = 0;
                int playerWins = 0;
                int ties = 0;

                while (true)
                {
                    Debug.Assert(table.CurrentState == EzBaccaratTableState.WaitingForBets);

                    if (betLost)
                    {
                        lastBet *= 2;
                        betLost = false;
                    }

                    lastBet = bet.Gambler.TryGet(lastBet);
                    maxBet = Math.Max(maxBet, lastBet);
                    bet.Banker = lastBet;
                    bet.Dragon = bet.Gambler.TryGet(5);
                    bet.Panda = bet.Gambler.TryGet(5);
                    table.Bets.Add(bet);

                    table.GoNextState();
                    if (table.CurrentState == EzBaccaratTableState.GameFinished)
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

                    if (table.Dealer.IsDragon)
                    {
                        outcome = "  =>7";
                    }

                    if (table.Dealer.IsPanda)
                    {
                        outcome = "8<=  ";
                    }

                    foreach(var payout in table.Payouts)
                    {
                        payout.Bet.Gambler.Put(payout.TotalWin);
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

                table.GoNextState();

                Console.WriteLine("Rounds: {0}", rounds);
                Console.WriteLine("Player: {0} ({1:0.0%}), Banker: {2} ({3:0.0%}), Tie: {4} ({5:0.0%})", playerWins, (double)playerWins / rounds,
                    bankerWins, (double)bankerWins / rounds, ties, (double)ties / rounds);
                Console.WriteLine("Gamble money: {0}, Max bet: {1}", bet.Gambler.Money, maxBet);
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
