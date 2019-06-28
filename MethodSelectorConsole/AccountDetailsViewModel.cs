using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class AccountDetailsViewModel : ViewModelBase
    {
        private string accountName = "nobody";
        private AccountType accountType = AccountType.UNINIT;
        private string accountId = "0";
        private float accountBalance = 0F;

        public string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                NotifyPropertyChanged();
            }
        }
        public AccountType Type
        {
            get { return accountType; }
            set
            {
                accountType = value;
                NotifyPropertyChanged();
            }
        }
        public string AccountId
        {
            get { return accountId; }
            set
            {
                accountId = value;
                NotifyPropertyChanged();
            }
        }
        public float Balance
        {
            get { return accountBalance; }
            set
            {
                accountBalance = value;
                NotifyPropertyChanged();
            }
        }
    }
}
