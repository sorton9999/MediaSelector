using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLib;
using CommonClasses;

namespace MethodSelectorConsole
{
    public class ListDataGetter : IDataGetter
    {
        Bank bank = null;
        const int MsgType = MessageTypes.AccountListMsgType;

        public ListDataGetter(Bank bank)
        {
            this.bank = bank;
        }

        public MessageData GetData()
        {
            MessageData data = new MessageData();
            List<AccountDetailsModel> list = new List<AccountDetailsModel>();

            foreach (var item in bank.AccountDetailsList.AccountDetailsList)
            {
                AccountDetailsModel model = new AccountDetailsModel();
                model.accountBalance = item.Balance;
                model.accountId = item.AccountId;
                model.accountName = item.AccountName;
                model.accountType = item.Type;
                model.addBtn = item.AddBtn;
                list.Add(model);
            }
            data.id = MsgType;
            data.name = "AccountList";
            data.message = list;

            return data;
        }

        public MessageData GetData(long handle)
        {
            MessageData data = new MessageData();
            List<AccountDetailsModel> list = new List<AccountDetailsModel>();

            foreach (var item in bank.AccountDetailsList.AccountDetailsList)
            {
                AccountDetailsModel model = new AccountDetailsModel();
                model.accountBalance = item.Balance;
                model.accountId = item.AccountId;
                model.accountName = item.AccountName;
                model.accountType = item.Type;
                model.addBtn = item.AddBtn;
                list.Add(model);
            }
            data.id = MsgType;
            data.handle = handle;
            data.name = "AccountList";
            data.message = list;

            return data;
        }

        public void SetData(object data)
        {

        }
    }

    public class ClientIdDataGetter : IDataGetter
    {
        int clientId = 0;
        const int MsgType = MessageTypes.ClientIdMsgType;

        public ClientIdDataGetter(int id)
        {
            clientId = id;
        }

        public MessageData GetData()
        {
            MessageData data = new MessageData();
            data.id = MsgType;
            data.name = "ClientId";
            data.message = clientId;
            return data;
        }

        public MessageData GetData(long handle)
        {
            MessageData data = new MessageData();
            data.id = MsgType;
            data.handle = handle;
            data.name = "ClientId";
            data.message = clientId;
            return data;
        }

        public void SetData(object data)
        {

        }
    }

    public class AccountDetailsDataGetter : IDataGetter
    {
        string accountId = String.Empty;
        Bank _bank;
        const int MsgType = MessageTypes.AccountDetailsMsgType;

        public AccountDetailsDataGetter(string id, Bank bank)
        {
            accountId = id;
            _bank = bank;
        }

        public MessageData GetData()
        {
            MessageData data = new MessageData();
            AccountDetailsViewModel vm = _bank.AccountDetailsByAccountId(accountId);
            if (vm != null)
            {
                AccountDetailsModel model = new AccountDetailsModel();
                model.accountBalance = vm.Balance;
                model.accountId = vm.AccountId;
                model.accountName = vm.AccountName;
                model.accountType = vm.Type;
                model.addBtn = vm.AddBtn;
                data.id = MsgType;
                data.name = "AccountDetails";
                data.message = model;
            }
            return data;
        }

        public MessageData GetData(long handle)
        {
            MessageData data = new MessageData();
            AccountDetailsViewModel vm = _bank.AccountDetailsByAccountId(accountId);
            if (vm != null)
            {
                AccountDetailsModel model = new AccountDetailsModel();
                model.accountBalance = vm.Balance;
                model.accountId = vm.AccountId;
                model.accountName = vm.AccountName;
                model.accountType = vm.Type;
                model.addBtn = vm.AddBtn;
                data.id = MsgType;
                data.handle = handle;
                data.name = "AccountDetails";
                data.message = model;
            }
            return data;
        }

        public void SetData(object data)
        {
            string acctId = data as string;
            if (!String.IsNullOrEmpty(acctId))
            {
                accountId = acctId;
            }
        }
    }

    public class TxDataGetter : IDataGetter
    {
        const int MsgType = MessageTypes.TxMsgType;

        public TxDataGetter()
        {
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
            // To prevent data ringing
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
