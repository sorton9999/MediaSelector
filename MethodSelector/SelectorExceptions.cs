using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelector
{
    public class IllegalOperationException : Exception
    {
        public IllegalOperationException()
        {
        }

        public IllegalOperationException(string message)
            : base(message)
        {
        }

        public IllegalOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException()
        {
        }

        public InsufficientFundsException(string message)
            : base(message)
        {
        }

        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class UndefinedActionException : Exception
    {
        public UndefinedActionException()
        {
        }

        public UndefinedActionException(string message)
            : base(message)
        {
        }

        public UndefinedActionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message)
            : base(message)
        {
        }

        public AccountNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class BankingException : Exception
    {
        public BankingException()
        {
        }

        public BankingException(string message)
            : base(message)
        {
        }

        public BankingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
