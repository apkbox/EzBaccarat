using System;

namespace EzBaccarat.Model
{
    public class Gambler
    {
        private bool allowNegativeBalance = false;

        public Gambler()
        {
        }

        public Gambler(bool allowNegativeBalance)
        {
            this.allowNegativeBalance = allowNegativeBalance;
        }

        public int Money { get; private set; }

        public int TryGet(int amount)
        {
            if (this.Money < amount && !this.allowNegativeBalance)
                return 0;

            this.Money -= amount;
            return amount;
        }

        public void Put(int amount)
        {
            this.Money += amount;
        }
    }
}