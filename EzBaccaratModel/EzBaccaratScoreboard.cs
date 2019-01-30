using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccaratModel
{
    public enum EzBaccaratScoreType
    {
        Empty,
        Player,
        Banker,
    }

    public class EzBaccaratScoreItem
    {
        public EzBaccaratScoreType Type { get; set; }

        public bool IsPlayerNatural { get; set; }
        public bool IsBankerNatural { get; set; }
        public bool IsTie { get; set; }
        public bool IsDragon { get; set; }
        public bool IsPanda { get; set; }
    }

    public class EzBaccaratScoreboardBeadPlate
    {

    }

    public class EzBaccaratScoreboardBigRoad
    {
        private const int Columns = 38;
        private const int Rows = 5;

        private int x = 0;
        private int y = 0;

        private List<EzBaccaratScoreItem> items = new List<EzBaccaratScoreItem>();

        private EzBaccaratScoreItem[,] plate = new EzBaccaratScoreItem[Columns, Rows];

        public EzBaccaratScoreboardBigRoad()
        {
        }

        public void AddScore(EzBaccaratScoreItem item)
        {
            items.Add(item);
        }

        private void Transform()
        {

        }
    }

    public class EzBaccaratScoreboard
    {
    }
}
