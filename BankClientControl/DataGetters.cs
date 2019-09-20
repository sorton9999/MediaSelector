using CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLib;

namespace BankClientControl
{
    public class OpenAcctDataGetter : IDataGetter
    {
        private string firstName = String.Empty;
        private string lastName = String.Empty;
        private float deposit = 0;
        const int MsgType = MessageTypes.OpenAcctMsgType;

        public OpenAcctDataGetter(string first, string last, string dep)
        {
            firstName = first;
            lastName = last;
            deposit = (float)Convert.ToDouble(dep);
        }

        public MessageData GetData()
        {
            MessageData data = new MessageData();
            Transaction tx = new Transaction();
            tx.acctFirstName = firstName;
            tx.acctLastName = lastName;
            tx.txOperation = "open";
            tx.txAmount = deposit;
            data.id = 0;
            data.name = "openacct";
            data.message = tx;
            data.id = MsgType;
            return data;
        }

        public MessageData GetData(long handle)
        {
            MessageData data = new MessageData();
            Transaction tx = new Transaction();
            tx.acctFirstName = firstName;
            tx.acctLastName = lastName;
            tx.txOperation = "open";
            tx.txAmount = deposit;
            data.id = 0;
            data.name = "openacct";
            data.message = tx;
            data.id = MsgType;
            data.handle = handle;
            return data;
        }

        public void SetData(object data)
        {

        }
    }

    public class TxDataGetter : IDataGetter
    {
        AccountDetailsModel details;
        const int MsgType = MessageTypes.TxMsgType;

        public TxDataGetter()
        {
        }

        public AccountDetailsModel AccountDetails
        {
            get { return details; }
            set { details = value; }
        }

        public Transaction TransactionDetails
        {
            get;
            set;
        }

        public MessageData GetData()
        {
            MessageData data = new MessageData();
            if (TransactionDetails != null)
            {
                Transaction tx = new Transaction();
                tx.acctFirstName = TransactionDetails.acctFirstName;
                tx.acctId = TransactionDetails.acctId;
                tx.acctLastName = TransactionDetails.acctLastName;
                tx.acctType = TransactionDetails.acctType;
                tx.balance = TransactionDetails.balance;
                tx.txAmount = TransactionDetails.txAmount;
                tx.txOperation = TransactionDetails.txOperation;

                data.message = tx;
            }
            data.id = MsgType;
            data.name = "tx";
            // To prevent data ringing
            TransactionDetails = null;
            return data;
        }

        public MessageData GetData(long handle)
        {
            MessageData data = new MessageData();
            if (TransactionDetails != null)
            {
                Transaction tx = new Transaction();
                tx.acctFirstName = TransactionDetails.acctFirstName;
                tx.acctId = TransactionDetails.acctId;
                tx.acctLastName = TransactionDetails.acctLastName;
                tx.acctType = TransactionDetails.acctType;
                tx.balance = TransactionDetails.balance;
                tx.txAmount = TransactionDetails.txAmount;
                tx.txOperation = TransactionDetails.txOperation;

                data.message = tx;
            }
            data.handle = handle;
            data.id = MsgType;
            data.name = "tx";

            // Prevent data ring
            TransactionDetails = null;

            return data;
        }

        public void SetData(object data)
        {
            Transaction tx = data as Transaction;
            if (tx != null)
            {
                TransactionDetails = tx;
            }
        }
    }

}
