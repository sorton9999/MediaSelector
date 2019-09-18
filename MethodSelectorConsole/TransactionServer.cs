using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliServLib;
using TcpLib;
using CommonClasses;
using MethodSelector;

namespace MethodSelectorConsole
{
    public class TransactionServer
    {
        Bank _bank;

        public static event AsyncCompletedEventHandler TransactionReceived;

        ClientStore clients;
        TxDataGetter tx = new TxDataGetter();
        //Action<MessageData> msgAction;
        ThreadedListener listenerThread;

        bool done = false;

        public TransactionServer(Bank bank)
        {
            _bank = bank;
            //msgAction = ReceiveData;
            clients = new ClientStore();
            ClientConnectAsync.OnConnect += ClientConnectAsync_OnConnect;
            ThreadedReceiver.DataReceived += ThreadedReceiver_DataReceived;
            listenerThread = new ThreadedListener(tx);
            listenerThread.Run(clients);
        }

        private void ThreadedReceiver_DataReceived(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ReceiveData data = e.UserState as ReceiveData;
                if (data != null)
                {
                    MessageData msg = data.clientData;
                    if ((msg != null) && (msg.id > 0))
                    {
                        System.Diagnostics.Debug.WriteLine("Received Message Type: {0}", msg.id);
                        Client client = ClientStore.FindClient(data.clientHandle);
                        if (client != null)
                        {
                            System.Diagnostics.Debug.WriteLine("   From Client: {0}", data.clientHandle);

                            try
                            {
                                switch (msg.id)
                                {
                                    case MessageTypes.OpenAcctMsgType:
                                        ProcessOpenAccount(msg);
                                        break;
                                    case MessageTypes.AccountDetailsMsgType:
                                        ProcessAccountDetails(msg);
                                        break;
                                    case MessageTypes.AccountListMsgType:
                                        ProcessAccountList(msg);
                                        break;
                                    case MessageTypes.ClientIdMsgType:
                                        ProcessAccountId(msg);
                                        break;
                                    case MessageTypes.TxMsgType:
                                        ProcessTransaction(msg);
                                        break;
                                    default:
                                        System.Diagnostics.Debug.WriteLine("Received unsupported msg type: {0}", msg.id);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("Transaction Exception: " + ex.Message);
                            }


                            client.ClearData();
                        }
                    }
                }
            }
        }

        private void ProcessTransaction(MessageData data)
        {
            System.Diagnostics.Debug.WriteLine("Processing Transaction message");
            if (data.id != MessageTypes.TxMsgType)
            {
                throw new BankingException("Invalid Transaction Type: " + data.id);
            }
            Transaction tx = data.message as Transaction;
            if (tx != null)
            {
                switch (tx.txOperation)
                {
                    case "open":
                        _bank.PerformAction(tx.acctId.ToString(), tx.acctLastName, tx.txOperation, tx.txAmount, tx.acctType, true);
                        break;
                    default:
                        throw new BankingException("Invalid Transaction Name: " + data.name);
                        break;
                }
            }
        }

        private void ProcessAccountId(MessageData data)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account ID message");
        }

        private void ProcessAccountList(MessageData data)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account List message");
        }

        private void ProcessAccountDetails(MessageData data)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account Details message");
        }

        private void ProcessOpenAccount(MessageData data)
        {
            System.Diagnostics.Debug.WriteLine("Processing Open Account message");
        }

        private void ClientConnectAsync_OnConnect(System.Net.Sockets.Socket socket)
        {
            ListDataGetter getter = new ListDataGetter(_bank);
            var message = GetDataAsync.GetMessageDataAsync(getter, CommonClasses.MessageTypes.AccountListMsgType);
            var sendResult = SendMessageAsync.SendMsgAsync(socket, message);

            if (sendResult.Result.Failure)
            {
                System.Diagnostics.Debug.WriteLine("Send Failure from Tx Server");
            }
        }

        //public void ReceiveData(MessageData data)
        //{
        //    Console.WriteLine("<<<<< Received Message of Type: {0} >>>>>", data.id);
        //}

        public bool ServerIsDone
        {
            get { return done; }
            set
            {
                done = value;
                if (done)
                {
                    listenerThread.StopLoopAction.Invoke();
                }
            }
        }

        public void SendTransaction(AccountDetailsViewModel details)
        {
            AccountDetailsModel model = new AccountDetailsModel();
            model.accountBalance = details.Balance;
            model.accountId = details.AccountId;
            model.accountName = details.AccountName;
            model.accountType = details.Type;
            model.addBtn = details.AddBtn;
            tx.AccountDetails = model;
        }

        public static void ClientReceiveTransaction(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageData message = e.UserState as MessageData;
                if (message != null)
                {
                    int id = message.id;
                    Console.WriteLine("Received Message for ID: {0}", id);
                }
            }
        }

        public bool ClientsAllDone()
        {
            return ClientStore.ClientsAllDone();
        }

        public void RemoveAllClients()
        {
            ClientStore.RemoveAllClients();
        }
    }
}
