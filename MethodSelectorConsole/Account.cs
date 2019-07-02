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

	[
        //public override float AccountAction(string action, float amt)
        //{
        //    return base.AccountAction(action, amt);
        //}

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
=======
        public override float AccountBalance()
        {
            return base.AccountBalance();
        }

        public override void PrintAccountDetails(int idx)
        {
            base.PrintAccountDetails(idx);
>>>>>>> f3a8c896113e2022969799da6d8db66a67a27b41
        }

        public float AccountAccrue(float interest)
        {
            return (AccountAction("accrue", interest));
        }
    }
}
