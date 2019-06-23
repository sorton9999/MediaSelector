using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class AccountNameViewModel : ViewModelBase
    {
        private string name = null;
        private string errorString = "";
        public string ActiveAccountName
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string ErrorString
        {
            get { return errorString; }
            set
            {
                errorString = value;
                NotifyPropertyChanged();
            }
        }
    }

}
