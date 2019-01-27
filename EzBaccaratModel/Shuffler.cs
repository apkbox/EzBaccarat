using System.Collections.Generic;

namespace EzBaccarat.Model
{
    public interface Shuffler
    {
        void Shuffle(List<Card> list);
    }
}