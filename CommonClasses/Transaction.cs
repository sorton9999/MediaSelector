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
        public int acctId;
        public string acctFirstName;
        public string acctLastName;
        public string txOperation;
        public CommonClasses.AccountType acctType;
        public float txAmount;
        public float balance;
    }
}
