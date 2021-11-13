using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses;

namespace MethodSelectorConsole
{
    public class BankConsole : DisplayBase
    {
        
        public BankConsole(string[] args)
            : base(args)
        {

        }

        public override int Display(Bank bank)
        {
            if (bank == null)
            {
                return 1;
            }

            string entry = String.Empty;
            string name = String.Empty;
            string acctId = Bank.InitialID;
            float amt = 0F;
            float balance = 0F;
            AccountType acctType = AccountType.UNINIT;
            Console.Write("Enter a name: ");
            entry = Console.ReadLine();
            name = entry;
            while ((String.Compare(entry, "quit", true) != 0) && (String.Compare(entry, "q", true) != 0))
            {
                Console.WriteLine("Account for {0}. Balance: [{1}]" + Environment.NewLine, name, balance);
                Console.WriteLine("What would you like to do? Type:");
                Console.WriteLine("[o]pen - Open/Create a new account.");
                Console.WriteLine("[d]eposit - Deposit entered amount into an Account.");
                Console.WriteLine("[w]ithdraw - Withdraw entered amount from an Account.");
                Console.WriteLine("[b]alance - Print out the balance of an Account.");
                Console.WriteLine("[a]ccrue -- Accrue interest on the Account.");
                Console.WriteLine("[s]witch - Switch to a named Account.");
                Console.WriteLine("[p]rint - Print out details of an Account.");
                Console.WriteLine("[du]mp -- Dump details of all Accounts");
                Console.WriteLine("[q]uit - Quit the program.");
                Console.Write("====> ");
                entry = Console.ReadLine();
                try
                {
                    switch (entry)
                    {
                        case "open":
                        case "o":
                            Console.WriteLine("Opening an Account. What Type? [s]imple, [i]nterest or [sa]vings");
                            string type = Console.ReadLine();
                            Console.Write("Initial Deposit? ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
                            acctId = bank.GetNewAcctId();
                            string cmd = "uninit";
                            if (type == "s")
                            {
                                acctType = AccountType.SIMPLE_CHECKING;
                                cmd = "deposit";
                            }
                            else if (type == "i")
                            {
                                acctType = AccountType.INTEREST_CHECKING;
                                cmd = "accrue";
                            }
                            else if (type == "sa")
                            {
                                acctType = AccountType.SAVINGS;
                                cmd = "accrue";
                            }
                            else
                            {
                                Console.WriteLine("Undefined action: " + type + ". Type one of [s] or [i].");
                            }
                            try
                            {
                                balance = bank.PerformAction(acctId, name, cmd, amt, acctType, true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "deposit":
                        case "d":
                            Console.Write("How much do you want to deposit? ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
                            try
                            {
                                balance = bank.PerformAction(acctId, name, "deposit", amt);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "withdraw":
                        case "w":
                            Console.Write("How much do you want to withdraw? Balance: [" + balance + "] ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
                            try
                            {
                                balance = bank.PerformAction(acctId, name, "withdraw", amt);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "balance":
                        case "b":
                            try
                            {
                                Console.WriteLine("Balance is: " + bank.PerformAction(acctId, name, "balance", amt));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "accrue":
                        case "a":
                            AccountDetailsViewModel details = bank.AccountDetailsByAccountId(acctId);
                            float defaultInterest = ((details.Type == AccountType.INTEREST_CHECKING) ? bank.CheckingInterest : bank.SavingsInterest);
                            Console.WriteLine("Accruing interest on Account: Default is {0}%", (defaultInterest * 100.0F));
                            Console.WriteLine("Do you want to change it? [y] or [n]");
                            float interest = defaultInterest;
                            entry = Console.ReadLine();
                            if (entry == "y")
                            {
                                Console.Write("Enter a percent value: ");
                                entry = Console.ReadLine();
                                interest = (float)(Convert.ToDouble(entry) / 100.0D);
                                Console.WriteLine("Interest changed to: [{0}]", (interest * 100.0F));
                            }
                            try
                            {
                                balance = bank.PerformAction(acctId, name, "accrue", interest);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "switch":
                        case "s":
                            Console.Write("Enter the Account Name to Switch to: ");
                            name = Console.ReadLine();
                            amt = 0;
                            try
                            {
                                AccountDetailsViewModel[] vm = bank.GetDetailsByName(name);
                                string choice = String.Empty;
                                if (vm.Count() > 1)
                                {
                                    Console.Write("Which Acct? ");
                                    foreach (var v in vm.Select((x, i) => new { x, i }))
                                    {
                                        Console.Write("[{0}]: {1} - {2} ; ", v.i, v.x.AccountId, v.x.Type.ToString());
                                    }
                                    Console.Write(Environment.NewLine + "Choose One: ==> ");
                                    choice = Console.ReadLine();
                                }
                                int chosenIdx = 0;
                                if (!String.IsNullOrEmpty(choice))
                                {
                                    chosenIdx = Convert.ToInt32(choice);
                                }
                                string id = "0";
                                if (vm != null)
                                {
                                    id = vm[chosenIdx].AccountId;
                                }
                                acctId = id;
                                balance = bank.PerformAction(acctId, name, "balance", 0);
                            }
                            catch (Exception e)
                            {
                                balance = 0;
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "print":
                        case "p":
                            Console.WriteLine(bank.PerformAction(acctId, name, "print", amt));
                            break;
                        case "dump":
                        case "du":
                            bank.DumpAccounts();
                            break;
                        case "quit":
                        case "q":
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.WriteLine("Undefined action: " + entry + ". Type one of [balance], [withdraw] or [deposit].");
                            break;
                    }
                }
                catch (MethodSelector.AccountNotFoundException ae)
                {
                    Console.WriteLine(ae.Message);
                    balance = 0;
                }
                catch (MethodSelector.BankingException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Open an Account by Typing \'open\'");
                }
            }
            Console.Write("...Goodbye. Hit ENTER to Exit.");
            Console.ReadLine();

            return 0;
        }
    }
}
