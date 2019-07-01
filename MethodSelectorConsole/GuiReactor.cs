using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class GuiReactor<T> where T : DisplayBase
    {
        private static GuiReactor<T> _instance = null;
        private T _display;
        private Bank _bank;

        public static GuiReactor<T> Instance(T display, string[] args, Bank bank)
        {
            return (_instance ?? (_instance = new GuiReactor<T>(display, args, bank)));
        }

        public void Start()
        {
            if (_display != null)
            {
                _display.Display(_bank);
            }
        }

        protected GuiReactor(T display, string[] args, Bank bank)
        {
            _display = null;
            _bank = bank;
            _display = display;
        }
    }
}
