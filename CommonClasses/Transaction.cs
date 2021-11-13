using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    [Serializable]
    public class Transaction
    {
        public int acctId = 0;
        public string acctFirstName = String.Empty;
        public string acctLastName = String.Empty;
        public string txOperation = String.Empty;
        public CommonClasses.AccountType acctType = AccountType.UNINIT;
        public float txAmount = 0F;
        public float balance = 0F;
        public bool response = false;
    }
}
