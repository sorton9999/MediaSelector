using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public abstract class DisplayBase
    {
        protected string[] _args;

        public DisplayBase(string[] args)
        {
            _args = args;
        }

        public abstract int Display(Bank bank);
    }
}
