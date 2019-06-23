using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodSelector;

namespace MethodSelectorConsole
{
    public enum AccountType { UNINIT = -99, INTEREST_CHECKING = 0, SIMPLE_CHECKING, SAVINGS, OTHER }

    public sealed class AccountDetailsViewModel : ViewModelBase
    {
        private string accountName = "nobody";
        private AccountType accountType = AccountType.UNINIT;
        private string accountId = "0";
        private int accountBalance = 0;

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
        public int Balance
        {
            get { return accountBalance; }
            set
            {
                accountBalance = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class Account
    {
        private readonly MethodSelectorClass selector;
        private string accountName;
        private AccountDetailsViewModel details = new AccountDetailsViewModel();
        
        public Account(string name, string id, int balance)
        {
            selector = new MethodSelectorClass(balance);
            accountName = name;
            details.AccountName = name;
            // Default this for now
            details.Type = AccountType.SIMPLE_CHECKING;
            details.AccountId = id;
            details.Balance = balance;
        }

        public string AccountName
        {
            get { return accountName; }
            private set { accountName = value; }
        }

        public AccountDetailsViewModel AccountDetails
        {
            get { return details; }
            private set { details = value; }
        }

        public int AccountDeposit(int amt)
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

        public int AccountWithdraw(int amt)
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

        public int AccountBalance()
        {
            int balance = AccountDetails.Balance;
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

        public int AccountAction(string action, int amt)
        {
            try
            {
                Func<string, int, int> perform = (transaction, amount) =>
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

        public void PrintAccountDetails(int idx)
        {
            AccountDetailsViewModel ad = AccountDetails;
            Console.WriteLine("=== [{0}] Account Details ===", (idx + 1));
            Console.WriteLine("Account Name: " + AccountName);
            Console.WriteLine("Acct Type: " + ad.Type.ToString());
            Console.WriteLine("Acct ID:   " + ad.AccountId);
            Console.WriteLine("Balance: " + ad.Balance);
        }
    }
}
