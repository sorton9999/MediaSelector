using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelector
{

     public class MethodSelectorExtensionClass : MethodSelectorClass
    {
        public MethodSelectorExtensionClass(float balance)
            : base (balance)
        {
        }

        public void ExtendSelection(string action, float inter, float amt = 0)
        {
            Func<Func<string, Func<float, float>>> makeAccount =
                ((Func<Func<string, Func<float, float>>>)(() =>
                {
                    MethodSelector("balance", amt);
                    float balance = Balance;
                    return (string transaction) =>
                    {
                        switch (transaction)
                        {
                            case "accrue":
                                return (float interest) =>
                                {
                                    balance += (balance * interest);
                                    return balance;
                                };
                            case "balance":
                            case "deposit":
                            case "withdraw":
                                return (float amount) =>
                                {
                                    var acc = Account(transaction);
                                    return (acc(amount));
                                };
                            default:
                                throw new IllegalOperationException(@"Illegal Operation: " + transaction);
                        }
                    };
                }
                ));
            var a = makeAccount();
            Balance = a(action)(inter);
        }
    }

    /// <summary>
    /// Provides 3 different anonymous method selector methods that takes
    /// a banking action (Balance, Deposit, Withdraw) and performs that
    /// action on the balance.  The balance is available after the action
    /// is performed.  Each of the methods follow a flavor of the
    /// Envoy pattern language as discussed at length in Ted Neward's blog.
    /// This is a functional programming technique that employs a closure
    /// that encapsulates the data and provides functions that act on the
    /// data.  In this case, the data is an account balance and the functions
    /// or actions are the banking operations.
    /// Ted Neward's pattern blogs can be found at 
    /// http://blogs.tedneward.com/patterns/catalog/
    /// </summary>
    public class MethodSelectorClass
    {
        public MethodSelectorClass(float balance)
        {
            Balance = balance;
        }

        public float Balance
        {
            get;
            protected set;
        }

        public Func<string, Func<float, float>> Account
        {
            get;
            private set;
        }

        public void MethodSelector(string action, float tAmt)
        {
            Func<float, Func<string, Func<float, float>>> makeAccount =
                ((Func<float, Func<string, Func<float, float>>>)((bal) =>
                {
                    var balance = bal;
                    return (string transaction) =>
                    {
                        switch (transaction)
                        {
                            case "withdraw":
                                return (float amount) =>
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
                                return (float amount) =>
                                {
                                    balance += amount;
                                    return balance;
                                };
                            case "balance":
                                return (float unused) =>
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
            Account = acct;
            Balance = acct(action)(tAmt);
        }

        public void MethodSelector2(string action, float tAmt)
        {
            Func<float, dynamic> makeAccount = (float bal) =>
            {
                var balance = bal;
                dynamic result = new System.Dynamic.ExpandoObject();
                result.withdraw = (Func<float, float>)((amount) =>
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
                result.deposit = (Func<float, float>)((amount) =>
                {
                    balance += amount;
                    return balance;
                });
                result.balance = (Func<float>)(() => balance);
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

        public void MethodSelector3(string action, float tAmt)
        {
            Func<float, Dictionary<string, Func<float, float>>> makeAccount =
                (float bal) =>
                {
                    var balance = bal;
                    var result = new Dictionary<string, Func<float, float>>();
                    result["withdraw"] = (Func<float, float>)((amount) =>
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
                    result["deposit"] = (Func<float, float>)((amount) =>
                    {
                        balance += amount;
                        return balance;
                    });
                    result["balance"] = (Func<float, float>)((unused) => balance);
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
