using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodSelector;
using CommonClasses;

namespace MethodSelectorConsole
{
    //public enum AccountType { UNINIT = -99, INTEREST_CHECKING = 0, SIMPLE_CHECKING, SAVINGS, OTHER }


    public abstract class Account : IAccount
    {
        private readonly string accountName;
        private readonly AccountDetailsViewModel details = new AccountDetailsViewModel();
        
        protected Account(string name, string id, float balance)
        {
            accountName = name;
            // Default this for now
            details.Type = AccountType.OTHER;
            details.AccountId = id;
        }

        public string AccountName
        {
            get { return accountName; }
        }

        public AccountDetailsViewModel AccountDetails
        {
            get { return details; }
        }

        public static string[] LoadAccountTypes()
        {
            string[] items = Enum.GetNames(typeof(AccountType));
            return items;
        }

        public abstract float AccountAction(string action, float amt);

        public virtual float AccountDeposit(float amt)
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

        public virtual float AccountWithdraw(float amt)
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

        public virtual float AccountBalance()
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

        public void PrintAccountDetails(int idx)
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
        private readonly MethodSelectorClass selector = null;

        public SimpleAccount(string name, string id, float balance)
            : base(name, id, balance)
        {
            selector = new MethodSelectorClass(balance);
            AccountDetails.Type = AccountType.SIMPLE_CHECKING;
            AccountDetails.AccountName = name;
            AccountDetails.AccountId = id;
            AccountDetails.Balance = balance;
            AccountDetails.AddBtn = false;
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

    }

    public class InterestAccount : Account
    {
        private readonly MethodSelectorExtensionClass selector = null;

        public InterestAccount(string name, string id, float balance)
            : base (name, id, balance)
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

        public float AccountAccrue(float interest)
        {
            return (AccountAction("accrue", interest));
        }
    }

    public class SavingsAccount : Account
    {
        private readonly MethodSelectorExtensionClass selector = null;

        public SavingsAccount(string name, string id, float balance)
            : base (name, id, balance)
        {
            selector = new MethodSelectorExtensionClass(balance);
            AccountDetails.Type = AccountType.SAVINGS;
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

        public float AccountAccrue(float interest)
        {
            return (AccountAction("accrue", interest));
        }
    }
}
