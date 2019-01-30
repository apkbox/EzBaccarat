using System;

namespace EzBaccarat.Model
{
    public class Gambler
    {
        public int Money { get; private set; }

        public int TryGet(int amount)
        {
            if (this.Money < amount)
                return 0;

            this.Money -= amount;
            return amount;
        }

        public void Put(int amount)
        {
            this.Money += amount;
        }

        public void Put(object totalWin)
        {
            throw new NotImplementedException();
        }
    }
}