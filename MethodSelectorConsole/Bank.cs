using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MethodSelector;

namespace MethodSelectorConsole
{
    public class Bank : ViewModelBase
    {
        private readonly Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        private AccountDetailsListViewModel accountDetails = new AccountDetailsListViewModel();
        private string newId = "12345";

        public Bank()
        {

        }

        public Dictionary<string, Account> Accounts
        {
            get;
            private set;
        }

        public AccountDetailsListViewModel AccountDetailsList
        {
            get { return accountDetails; }
            private set
            {
                accountDetails = value;
            }
        }

        public int PerformAction(string name, string action, int amount, bool openAcct = false)
        {
            Account account = null;
            int balance = 0;
            try
            {
                account = FindAccount(name);
            }
            catch (MethodSelector.AccountNotFoundException e)
            {
                Console.WriteLine(e.Message);
                if (openAcct)
                {
                    long id = Convert.ToInt64(newId);
                    newId = (++id).ToString();
                    account = MakeAccount(name, newId, amount);
                }
                else
                {
                    Console.WriteLine("<<< Do you need to open an account for [{0}]? >>>", name);
                    throw;
                }
            }
            catch (Exception e)
            {
                throw new BankingException(e.Message, e.InnerException);
            }
            if (account != null)
            {
                if (openAcct)
                {
                    return account.AccountBalance();
                }
                try
                {
                    balance = AccountActions(account, action, amount);
                }
                catch (Exception)
                {
                    throw;
                }
                UpdateDetails(account, balance);
            }
            return balance;
        }

        private int AccountActions(Account account, string action, int amount)
        {
            try
            {
                if (action == "print")
                {
                    account.PrintAccountDetails(0);
                    return account.AccountBalance();
                }
                else
                {
                    return (account.AccountAction(action, amount));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
                //return account.AccountBalance();
            }
        }

        private void UpdateDetails(Account account, int balance)
        {
            var item = accountDetails.AccountDetailsList.ToLookup(x => x.AccountName == account.AccountName);
            foreach (var p in item[true])
            {
                p.Balance = balance;
            }
            
        }

        private Account MakeAccount(string name, string id, int deposit)
        {
            Console.WriteLine("<<< Making an account for: [{0}] with initial deposit: [{1}] >>>", name, deposit);
            Account account = new Account(name, id, deposit);
            try
            {
                accounts.Add(name, account);
                accountDetails.AccountDetailsList.Add(account.AccountDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                account = null;
            }
            return account;
        }

        private Account FindAccount(string name)
        {
            Account account = null;
            try
            {
                var acct = accounts.ToLookup(x => x.Key == name);
                foreach (var p in acct[true])
                {
                    account = p.Value;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("FindAccount: " + e.Message);
                throw new AccountNotFoundException("Account Not Found: [" + name + "]");
            }
            if (account == null)
            {
                throw new AccountNotFoundException("Account Not Found: [" + name + "]");
            }
            return account;
        }

        public void DumpAccounts()
        {
            foreach (var acct in accounts.Select((x, i) => new { Value = x, Index = i }))
            {
                acct.Value.Value.PrintAccountDetails(acct.Index);
            }
        }

    }
}
