using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClientControl
{
    //public enum AccountType { UNINIT = -99, INTEREST_CHECKING = 0, SIMPLE_CHECKING, SAVINGS, OTHER }

    public class AccountDetailsViewModel : ViewModelBase
    {
        //private string accountName = "nobody";
        //private AccountType accountType = AccountType.UNINIT;
        //private string accountId = "0";
        //private float accountBalance = 0F;
        //private bool addBtn = false;

        CommonClasses.AccountDetailsModel model = new CommonClasses.AccountDetailsModel();

        private string strOut = String.Empty;

        public string AccountName
        {
            get { return model.accountName; }
            set
            {
                model.accountName = value;
                NotifyPropertyChanged();
            }
        }
        public CommonClasses.AccountType Type
        {
            get { return model.accountType; }
            set
            {
                model.accountType = value;
                NotifyPropertyChanged();
            }
        }
        public string AccountId
        {
            get { return model.accountId; }
            set
            {
                model.accountId = value;
                NotifyPropertyChanged();
            }
        }
        public float Balance
        {
            get { return model.accountBalance; }
            set
            {
                model.accountBalance = value;
                NotifyPropertyChanged();
            }
        }

        public bool AddBtn
        {
            get { return model.addBtn; }
            set
            {
                model.addBtn = value;
                NotifyPropertyChanged();
            }
        }

        public string StrOut
        {
            get { return strOut; }
            set
            {
                strOut = value;
                NotifyPropertyChanged();
            }
        }
    }
}
