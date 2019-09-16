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

        public ListDataGetter(Bank bank)
        {
            this.bank = bank;
        }

        public MessageData GetData(int msgType)
        {
            MessageData data = new MessageData();
            List<AccountDetailsModel> list = new List<AccountDetailsModel>();

            switch (msgType)
            {
                case MessageTypes.AccountListMsgType:
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
                    data.id = msgType;
                    data.name = "AccountList";
                    data.message = list;
                    break;
                case MessageTypes.AccountDetailsMsgType:
                case MessageTypes.ClientIdMsgType:
                default:
                    System.Diagnostics.Debug.WriteLine("Unsupported message type: " + msgType);
                    break;
        }
            return data;
        }
    }

    public class ClientIdDataGetter : IDataGetter
    {
        int clientId = 0;

        public ClientIdDataGetter(int id)
        {
            clientId = id;
        }

        public MessageData GetData(int msgType)
        {
            MessageData data = new MessageData();
            if (msgType == MessageTypes.ClientIdMsgType)
            {
                data.id = msgType;
                data.name = "ClientId";
                data.message = clientId;
            }
            return data;
        }
    }

    public class AccountDetailsDataGetter : IDataGetter
    {
        string accountId = String.Empty;
        Bank _bank;

        public AccountDetailsDataGetter(string id, Bank bank)
        {
            accountId = id;
            _bank = bank;
        }

        public MessageData GetData(int msgType)
        {
            MessageData data = new MessageData();
            if (msgType == MessageTypes.AccountDetailsMsgType)
            {
                AccountDetailsViewModel vm = _bank.AccountDetailsByAccountId(accountId);
                if (vm != null)
                {
                    AccountDetailsModel model = new AccountDetailsModel();
                    model.accountBalance = vm.Balance;
                    model.accountId = vm.AccountId;
                    model.accountName = vm.AccountName;
                    model.accountType = vm.Type;
                    model.addBtn = vm.AddBtn;
                    data.id = msgType;
                    data.name = "AccountDetails";
                    data.message = model;
                }
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
