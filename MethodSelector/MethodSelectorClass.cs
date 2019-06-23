using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelector
{
    /// <summary>
    /// Provides 3 different anonymous method selector methods that takes
    /// a banking action (Balance, Deposit, Withdraw) and performs that
    /// action on the balance.  The balance is available after the action
    /// is performed.  Each of the methods follow a flavor of the
    /// Envoy pattern as discussed at length in Ted Neward's blog.  This
    /// is a functional programming technique that employs a closure that
    /// encapsulates the data and provides functions that act on the data.
    /// In this case, the data is an account balance and the functions or
    /// actions are the banking operations.
    /// Ted Neward's pattern blogs can be found at 
    /// http://http://blogs.tedneward.com/patterns/catalog/
    /// </summary>
    public class MethodSelectorClass
    {
        public MethodSelectorClass(int balance)
        {
            Balance = balance;
        }

        public int Balance
        {
            get;
            private set;
        }

        public void MethodSelector(string action, int tAmt)
        {
            Func<int, Func<string, Func<int, int>>> makeAccount =
                ((Func<int, Func<string, Func<int, int>>>)((bal) =>
                {
                    var balance = bal;
                    return (string transaction) =>
                    {
                        switch (transaction)
                        {
                            case "withdraw":
                                return (int amount) =>
                                {
                                    if (balance >= amount)
                                    {
                                        balance -= amount;
                                        return balance;
                                    }
                                    else
                                    {
                                        throw new InsufficientFundsException(@"Insufficient Funds. Balance: " + Balance);
                                    }
                                };
                            case "deposit":
                                return (int amount) =>
                                {
                                    balance += amount;
                                    return balance;
                                };
                            case "balance":
                                return (int unused) =>
                                {
                                    return balance;
                                };
                            default:
                                throw new IllegalOperationException(@"Illegal Operation: " + transaction);
                        }
                    };
                }
                ));
            var acct = makeAccount(Balance);
            Balance = acct(action)(tAmt);
        }

        public void MethodSelector2(string action, int tAmt)
        {
            Func<int, dynamic> makeAccount = (int bal) =>
            {
                var balance = bal;
                dynamic result = new System.Dynamic.ExpandoObject();
                result.withdraw = (Func<int, int>)((amount) =>
                {
                    if (balance >= amount)
                    {
                        balance -= amount;
                        return balance;
                    }
                    else
                    {
                        throw new InsufficientFundsException(@"Insufficient funds. Balance: " + Balance);
                    }
                });
                result.deposit = (Func<int, int>)((amount) =>
                {
                    balance += amount;
                    return balance;
                });
                result.balance = (Func<int>)(() => balance);
                return result;
            };
            var acct = makeAccount(Balance);
            switch (action)
            {
                case "withdraw":
                    Balance = acct.withdraw(tAmt);
                    break;
                case "deposit":
                    Balance = acct.deposit(tAmt);
                    break;
                case "balance":
                    Balance = acct.balance();
                    break;
                default:
                    throw new UndefinedActionException(@"Undefined Action: " + action);
            }
        }

        public void MethodSelector3(string action, int tAmt)
        {
            Func<int, Dictionary<string, Func<int, int>>> makeAccount =
                (int bal) =>
                {
                    var balance = bal;
                    var result = new Dictionary<string, Func<int, int>>();
                    result["withdraw"] = (Func<int, int>)((amount) =>
                    {
                        if (balance >= amount)
                        {
                            balance -= amount;
                            return balance;
                        }
                        else
                        {
                            throw new InsufficientFundsException(@"Insufficient Funds! Balance: " + Balance);
                        }
                    });
                    result["deposit"] = (Func<int, int>)((amount) =>
                    {
                        balance += amount;
                        return balance;
                    });
                    result["balance"] = (Func<int, int>)((unused) => balance);
                    return result;
                };
            var acct = makeAccount(Balance);
            try
            {
                Balance = acct[action](tAmt);
            }
            catch (Exception)
            {
                throw new UndefinedActionException(@"Undefined Action: " + action);
            }
        }
    }
}
