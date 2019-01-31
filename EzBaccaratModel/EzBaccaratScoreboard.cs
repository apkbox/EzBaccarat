using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat.Model
{
    public enum EzBaccaratWinningHand
    {
        None,
        Player,
        Banker
    }

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

    public class EzBaccaratScoreboardBigRoad
    {
        private const int Columns = 38;
        private const int Rows = 5;

        private int currentColumn = 0;
        private int x = 0;
        private int y = 0;
        private int unattributedTies = 0;

        private EzBaccaratScoreItem[,] plate = new EzBaccaratScoreItem[Columns, Rows];

        public EzBaccaratScoreboardBigRoad()
        {
            plate.Initialize();

            for (var i = 0; i < plate.Length; i++)
            {
                Items.Add(null);
            }
        }

        public IList<EzBaccaratScoreItem> Items { get; } = new List<EzBaccaratScoreItem>();

        // TODO: Update Items directly using method instead of 2d array.
        // TODO: Add update event.
        public void AddScore(EzBaccaratScoreItem item)
        {
            if (item.IsTie)
            {
                if (plate[x, y] == null)
                {
                    ++unattributedTies;
                }
                else
                {
                    plate[x, y].IsTie = true;
                    unattributedTies = 0;
                }
            }
            else
            {
                if (plate[x, y] == null)
                {
                    // If there is nothing yet - place an item
                    plate[x, y] = item;
                }
                else {
                    // There is a previous item - move next, bend or start new column
                    if (plate[x, y].WinningHand == item.WinningHand)
                    {
                        // Try next position down, unless we out of rows, the previous tail is there or we are already growing a tail.
                        if ((y + 1) >= Rows || plate[x, y + 1] != null || x > currentColumn)
                        {
                            // Cannot move down - move right
                            ++x;
                        }
                        else
                        {
                            // Otherwise move down, unless we are already growing a tail.
                            ++y;
                        }
                    }
                    else
                    {
                        // Move next column
                        x = ++currentColumn;
                        y = 0;
                    }

                    plate[x, y] = item;

                    if (unattributedTies > 0)
                    {
                        plate[x, y].IsTie = true;
                        unattributedTies = 0;
                    }
                }
            }

            this.Items[y * Columns + x] = this.plate[x, y];
        }
    }
}
