using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EzBaccarat
{
    class TableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string PlayerHand1 { get { return "AC"; } }
        public string PlayerHand2 { get { return "AC"; } }
        public string PlayerHand3 { get { return "AC"; } }

        public string BankerHand1 { get { return "AC"; } }
        public string BankerHand2 { get { return "AC"; } }
        public string BankerHand3 { get { return "ACx"; } }

        public bool PlayerWin { get { return true; } }

        public bool BankerWin { get { return false; } }

        public void InvokePropertyChanged([CallerMemberName] string name = "")
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
