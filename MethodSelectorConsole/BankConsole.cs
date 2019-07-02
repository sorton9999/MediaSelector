using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            float amt = 0F;
            float balance = 0F;
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
                            Console.WriteLine("Opening an Account. What Type? [s]imple or [i]nterest");
                            string type = Console.ReadLine();
                            Console.Write("Initial Deposit? ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
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
=======
                            if (type == "s")
                            {
                                balance = bank.PerformAction(name, "deposit", amt, true);
                            }
                            break;
                        case "deposit":
                        case "d":
                            Console.Write("How much do you want to deposit? ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
                            balance = bank.PerformAction(name, "deposit", amt, false);
                            break;
                        case "withdraw":
                        case "w":
                            Console.Write("How much do you want to withdraw? Balance: [" + balance + "] ");
                            entry = Console.ReadLine();
                            amt = (float)Convert.ToDouble(entry);
                            try
                            {
                                balance = bank.PerformAction(name, "withdraw", amt, false);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case "balance":
                        case "b":
                            Console.WriteLine("Balance is: " + bank.PerformAction(name, "balance", amt, false));
                            break;
                        case "accrue":
                        case "a":
                            Console.WriteLine("Accruing interest on Account: Default is 03%");
                            Console.WriteLine("Do you want to change it? [y] or [n]");
                            float interest = 0.03F;
                            entry = Console.ReadLine();
                            if (entry == "y")
                            {
                                entry = Console.ReadLine();
                                interest = (float)Convert.ToDouble(entry);
                                Console.WriteLine("Interest changed to: [{0}]", interest);
                            }
                            balance = bank.PerformAction(name, "accrue", interest, true);
                            break;
                        case "switch":
                        case "s":
                            Console.Write("Enter the Account Name to Switch to: ");
                            name = Console.ReadLine();
                            amt = 0;
                            balance = bank.PerformAction(name, "balance", 0, false);
                            break;
                        case "print":
                        case "p":
                            Console.WriteLine(bank.PerformAction(name, "print", amt, false));
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
