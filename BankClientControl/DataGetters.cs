﻿using CommonClasses;
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

        public OpenAcctDataGetter(string first, string last, string dep)
        {
            firstName = first;
            lastName = last;
            deposit = (float)Convert.ToDouble(dep);
        }

        public MessageData GetData(int msgType)
        {
            MessageData data = new MessageData();
            if (msgType == MessageTypes.OpenAcctMsgType)
            {
                Transaction tx = new Transaction();
                tx.acctFirstName = firstName;
                tx.acctLastName = lastName;
                tx.txOperation = "open";
                tx.txAmount = deposit;
                data.id = 0;
                data.name = "openacct";
                data.message = tx;
            }
            return data;
        }
    }

    public class TxDataGetter : IDataGetter
    {
        AccountDetailsModel details;

        public TxDataGetter()
        {
        }

        public AccountDetailsModel AccountDetails
        {
            get { return details; }
            set { details = value; }
        }

        public MessageData GetData(int msgType)
        {
            MessageData data = new MessageData();
            if ((details != null) && (msgType == MessageTypes.TxMsgType))
            {
                Transaction tx = new Transaction();
                tx.acctFirstName = String.Empty;
                tx.acctId = Convert.ToInt32(details.accountId);
                tx.acctLastName = details.accountName;
                tx.acctType = details.accountType;
                tx.balance = details.accountBalance;
                tx.txAmount = 0;
                tx.txOperation = "tx";
            }
            return data;
        }
    }

}