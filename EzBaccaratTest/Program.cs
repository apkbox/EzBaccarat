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

            var gambler = new Gambler(true);
            gambler.Put(1000);

            for (var z = 0; z < 1000; ++z)
            {
                Debug.Assert(table.CurrentState == EzBaccaratTableState.NotReady);

                table.GoNextState();
                Debug.Assert(table.CurrentState == EzBaccaratTableState.WaitingForBets);

                EzBaccaratBet bet = new EzBaccaratBet();
                bet.Gambler = gambler;
                // bet.Gambler = new Gambler();
                // bet.Gambler.Put(1000);

                int state = 0;
                bool betLost = true;
                int lastBet = 25;
                int maxBet = 0;
                int rounds = 0;

                int bankerWins = 0;
                int playerWins = 0;
                int ties = 0;
                bool bankerSide = false;

                while (true)
                {
                    Debug.Assert(table.CurrentState == EzBaccaratTableState.WaitingForBets);

                    if (betLost)
                    {
                        state = 0;
                        lastBet = 25;
                        bankerSide = !bankerSide;
                    }
                    else
                    {
                        switch(state)
                        {
                            case 0:
                                lastBet = 75;
                                state = 1;
                                break;
                            case 1:
                                lastBet = 50;
                                state = 2;
                                break;
                            case 2:
                                lastBet = 100;
                                state = 0;
                                break;
                        }
                    }

                    betLost = false;

                    lastBet = bet.Gambler.TryGet(lastBet);
                    maxBet = Math.Max(maxBet, lastBet);
                    if (bankerSide)
                    {
                        bet.Banker = lastBet;
                        bet.Player = 0;
                    }
                    else
                    {
                        bet.Banker = 0;
                        bet.Player = lastBet;
                    }

                    table.Bets.Add(bet);

                    table.GoNextState();

                    ++rounds;

                    string outcome = "?????";
                    if (table.Dealer.IsPlayerWin)
                    {
                        outcome = "<==  ";
                        ++playerWins;
                    }
                    else
                    {
                        if (!bankerSide)
                            betLost = true;
                    }

                    if (table.Dealer.IsBankerWin)
                    {
                        outcome = "  ==>";
                        ++bankerWins;
                    }
                    else
                    {
                        if (bankerSide)
                            betLost = true;
                    }

                    if (table.Dealer.IsTie)
                    {
                        outcome = " TIE ";
                        ++ties;

                        betLost = false;
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

                    if (true)
                    {
                        Console.WriteLine("Player: {0} ({1})    {2}    Banker: {3} ({4})     Balance: {5} ({6} - {7})",
                            PrintHand(table.Dealer.PlayerHand), table.Dealer.PlayerPoints,
                            outcome, PrintHand(table.Dealer.BankerHand), table.Dealer.BankerPoints, bet.Gambler.Money, lastBet, bankerSide ? "B" : "P");
                    }

                    table.GoNextState();
                    if (table.CurrentState == EzBaccaratTableState.GameFinished)
                        break;

                    if (bet.Gambler.Money <= 0 && false)
                    {
                        Console.WriteLine("BANKRUPT!!!!!");
                        // goto end;
                    }
                }

                table.GoNextState();

                Console.WriteLine("Rounds: {0}", rounds);
                Console.WriteLine("Player: {0} ({1:0.0%}), Banker: {2} ({3:0.0%}), Tie: {4} ({5:0.0%})", playerWins, (double)playerWins / rounds,
                    bankerWins, (double)bankerWins / rounds, ties, (double)ties / rounds);
                Console.WriteLine("Gamble money: {0}, Max bet: {1}", bet.Gambler.Money, maxBet);
            }
        end:;
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
