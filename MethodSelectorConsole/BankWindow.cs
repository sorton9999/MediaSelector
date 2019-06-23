using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class BankWindow : DisplayBase
    {
        BankControlForm _form = null;
        public BankWindow(string[] args)
            : base(args)
        {

        }

        public override int Display(Bank bank)
        {
            Thread windowThread = new Thread(new ThreadStart(() =>
            {
                string name;
                Console.Write("Enter a Name: ");
                name = Console.ReadLine();
                _form = new BankControlForm(name, bank);
                _form.Show();

                System.Windows.Threading.Dispatcher.Run();
            }
            ));
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.IsBackground = false;
            windowThread.Start();
            return 0;
        }
    }
}
