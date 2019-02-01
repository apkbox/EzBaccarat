using System.Collections.Generic;

namespace EzBaccarat.Model
{
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
            Clear();
        }

        public void Clear()
        {
            for (var x = 0; x < plate.GetLength(0); x++)
            {
                for (var y = 0; y < plate.GetLength(1); y++)
                {
                    plate[x, y] = null;
                }
            }

            Items.Clear();
            for (var i = 0; i < plate.Length; i++)
            {
                Items.Add(null);
            }

            x = 0;
            y = 0;
            unattributedTies = 0;
            currentColumn = 0;
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

                    // TODO: Check if x is out of bounds and scroll whole plate left.
                    if (x >= Columns)
                    {
                        for (var i = 0; i < this.plate.GetLength(0) - 1; i++)
                        {
                            for (var j = 0; j < this.plate.GetLength(1); j++)
                            {
                                this.plate[i, j] = this.plate[i + 1, j];
                            }
                        }

                        var lastCol = this.plate.GetLength(0) - 1;
                        for (var j = 0; j < this.plate.GetLength(1); j++)
                        {
                            this.plate[lastCol, j] = null;
                        }

                        x -= 1;
                        currentColumn -= 1;
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
