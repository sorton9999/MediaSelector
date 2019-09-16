using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliServLib;
using TcpLib;
using CommonClasses;

namespace MethodSelectorConsole
{
    public class TransactionServer
    {
        Bank _bank;

        public static event AsyncCompletedEventHandler TransactionReceived;

        ClientStore clients;
        TxDataGetter tx = new TxDataGetter();
        Action<MessageData> msgAction;
        ThreadedListener listenerThread;

        bool done = false;

        public TransactionServer(Bank bank)
        {
            _bank = bank;
            msgAction = ReceiveData;
            clients = new ClientStore(msgAction);
            ClientConnectAsync.OnConnect += ClientConnectAsync_OnConnect;
            listenerThread = new ThreadedListener(tx);
            listenerThread.Run(clients);
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

        public void ReceiveData(MessageData data)
        {
            Console.WriteLine("<<<<< Received Message of Type: {0} >>>>>", data.id);
        }

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
