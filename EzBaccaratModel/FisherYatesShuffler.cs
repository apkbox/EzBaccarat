using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace EzBaccarat.Model
{
    public class FisherYatesShuffler : Shuffler
    {
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public FisherYatesShuffler()
        {
        }

        public void Shuffle(List<Card> list)
        {
            for (var i = 0; i < 1; i++)
            {
                ShuffleRound(list);
            }
        }

        private void ShuffleRound(List<Card> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var j = Next(i, list.Count - 1);
                var t = list[i];
                list[i] = list[j];
                list[j] = t;
            }
        }


        private Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
            {
                return minValue;
            }

            Int64 diff = maxValue - minValue;
            byte[] uint32Buffer = new byte[4];

            while (true)
            {
                rng.GetBytes(uint32Buffer);
                UInt32 rand = BitConverter.ToUInt32(uint32Buffer, 0);

                Int64 max = (1 + (Int64)UInt32.MaxValue);
                Int64 remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (Int32)(minValue + (rand % diff));
                }
            }
        }
    }
}