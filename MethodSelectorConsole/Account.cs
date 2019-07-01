using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodSelector;

namespace MethodSelectorConsole
{
    public enum AccountType { UNINIT = -99, INTEREST_CHECKING = 0, SIMPLE_CHECKING, SAVINGS, OTHER }

    public abstract class AccountBase
    {
        protected string accountName = String.Empty;
        protected AccountDetailsViewModel details = new AccountDetailsViewModel();

        //public string AccountName
        //{
        //    get { return details.AccountName; }
        //    protected set { details.AccountName = value; }
        //}
        public string AccountName
        {
            get { return accountName; }
            private set { accountName = value; }
        }


        public AccountDetailsViewModel AccountDetails
        {
            get { return details; }
            protected set { details = value; }
        }

        public abstract float AccountAction(string action, float amount);

        public abstract void PrintAccountDetails(int idx);

        public abstract float AccountBalance();
    }

    public class Account : AccountBase
    {
        private readonly MethodSelectorClass selector;
        
        public Account(string name, string id, float balance)
        {
            selector = new MethodSelectorClass(balance);
            accountName = name;
            // Default this for now
            details.Type = AccountType.OTHER;
            details.AccountId = id;
        }

        public static string[] LoadAccountTypes()
        {
            string[] items = Enum.GetNames(typeof(AccountType));
            return items;
        }

        public float AccountDeposit(float amt)
        {
            try
            {
                AccountDetails.Balance = AccountAction("deposit", amt);
            }
            catch (Exception)
            {
                throw;
            }
            return AccountDetails.Balance;
        }

        public float AccountWithdraw(float amt)
        {
            try
            {
                AccountDetails.Balance = AccountAction("withdraw", amt);
            }
            catch (Exception)
            {
                throw;
            }
            return AccountDetails.Balance;
        }

        public override float AccountBalance()
        {
            float balance = AccountDetails.Balance;
            try
            {
                balance = AccountAction("balance", 0);
            }
            catch (Exception)
            {
                throw;
            }
            return balance;
        }

        public override float AccountAction(string action, float amt)
        {
            try
            {
                Func<string, float, float> perform = (transaction, amount) =>
                {
                    selector.MethodSelector(transaction, amount);
                    return selector.Balance;
                };
                return (perform(action, amt));
            }
            catch (Exception e)
            {
                throw new IllegalOperationException(("Exception: " + e.Message + " for Action: " + action + " on Account: " + AccountName), e.InnerException);
            }
        }

        public override void PrintAccountDetails(int idx)
        {
            AccountDetailsViewModel ad = AccountDetails;
            Console.WriteLine("=== [{0}] Account Details ===", (idx + 1));
            Console.WriteLine("Account Name: " + AccountName);
            Console.WriteLine("Acct Type: " + ad.Type.ToString());
            Console.WriteLine("Acct ID:   " + ad.AccountId);
            Console.WriteLine("Balance: " + AccountBalance());
        }
    }

    public class SimpleAccount : Account
    {
        private readonly MethodSelectorExtensionClass selector = null;

        public SimpleAccount(string name, string id, float balance)
            : base(name, id, balance)
        {
            selector = new MethodSelectorExtensionClass(balance);
            AccountDetails.Type = AccountType.SIMPLE_CHECKING;
            AccountDetails.AccountName = name;
            AccountDetails.AccountId = id;
            AccountDetails.Balance = balance;
            AccountDetails.AddBtn = false;
        }

        public override float AccountAction(string action, float amt)
        {
            return base.AccountAction(action, amt);
        }

        public override float AccountBalance()
        {
            return base.AccountBalance();
        }

        public override void PrintAccountDetails(int idx)
        {
            base.PrintAccountDetails(idx);
        }
    }

    public class InterestAccount : Account
    {
        private readonly MethodSelectorExtensionClass selector = null;

        public InterestAccount(string name, string id, float balance)
            : base(name, id, balance)
        {
            selector = new MethodSelectorExtensionClass(balance);
            AccountDetails.Type = AccountType.INTEREST_CHECKING;
            AccountDetails.AccountName = name;
            AccountDetails.AccountId = id;
            AccountDetails.Balance = balance;
            AccountDetails.AddBtn = true;
        }

        public override float AccountAction(string action, float amt)
        {
            try
            {
                Func<string, float, float> perform = 
                    ((Func<string, float, float>)((transaction, amount) =>
                {
                    selector.ExtendSelection(transaction, amount);
                    return selector.Balance;
                }
                ));
                return (perform(action, amt));
            }
            catch (Exception e)
            {
                throw new IllegalOperationException(("Exception: " + e.Message + " for Action: " + action + " on Account: " + AccountName), e.InnerException);
            }
        }

        public override float AccountBalance()
        {
            return base.AccountBalance();
        }

        public override void PrintAccountDetails(int idx)
        {
            base.PrintAccountDetails(idx);
        }

        public float AccountAccrue(float interest)
        {
            return (AccountAction("accrue", interest));
        }
    }
}
