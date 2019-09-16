using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    public enum AccountType { UNINIT = -99, INTEREST_CHECKING = 0, SIMPLE_CHECKING, SAVINGS, OTHER }

    [Serializable]
    public class AccountDetailsModel
    {
        public string accountName = "nobody";
        public AccountType accountType = AccountType.UNINIT;
        public string accountId = "0";
        public float accountBalance = 0F;
        public bool addBtn = false;
    }

    [Serializable]
    public class AccountNameModel
    {
        public string name = null;
        public string errorString = "";
    }

    public class AccountDetailsClass
    {
        public static List<AccountDetailsModel> accountDetailsList = new List<AccountDetailsModel>();

        public static List<AccountDetailsModel> AccountDetailsList
        {
            get { return accountDetailsList; }
            set { accountDetailsList = value; }
        }
    }

    public static class MessageTypes
    {
        // Add message types here to coordinate send/recv of data
        public const int ClientIdMsgType = 10;
        public const int AccountDetailsMsgType = 20;
        public const int AccountListMsgType = 30;
        public const int OpenAcctMsgType = 40;
        public const int TxMsgType = 50;
    }
}
