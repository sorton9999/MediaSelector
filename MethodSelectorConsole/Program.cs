using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class Program
    {
        static bool use_graphical = false;

        static void Main(string[] args)
        {
            ParseArgs(args);
            DisplayBase display = null;
            Bank bank = new Bank();

            if (use_graphical)
            {
                display = new BankWindow(args);
            }
            else
            {
                display = new BankConsole(args);
            }
            GuiReactor<DisplayBase>.Instance(display, args, bank).Start();
        }

        private static void ParseArgs(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg == "-G")
                {
                    Console.WriteLine("Using graphical interface.");
                    use_graphical = true;
                }
            }
        }
    }
}
