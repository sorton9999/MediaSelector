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
using System.Windows.Threading;

namespace MethodSelectorConsole
{
    public class TransactionServer
    {
        Bank _bank;
        ClientStore clients;
        TxDataGetter tx = new TxDataGetter();
        ThreadedListener listenerThread;
        private const int MaxEmptyRcv = 100;
        private int currentNumEmptyRcv = 0;

        bool done = false;

        public TransactionServer(Bank bank, Dispatcher dispatcher)
        {
            _bank = bank;
            _bank.Dispatcher = dispatcher;
            clients = new ClientStore();
            ThreadedReceiver.ServerDataReceived += ThreadedReceiver_ServerDataReceived;
            listenerThread = new ThreadedListener(tx);
            listenerThread.Run(clients);
        }

        private void ThreadedReceiver_ServerDataReceived(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ReceiveData data = e.UserState as ReceiveData;
                if (data != null)
                {
                    MessageData msg = data.clientData;
                    Client client = ClientStore.FindClient(data.clientHandle);
                    if ((msg != null) && (msg.id > 0))
                    {
                        System.Diagnostics.Debug.WriteLine("Received Message Type: {0}", msg.id);
                        if (client != null)
                        {
                            System.Diagnostics.Debug.WriteLine("   From Client: {0}", data.clientHandle);

                            try
                            {
                                switch (msg.id)
                                {
                                    case MessageTypes.OpenAcctMsgType:
                                        ProcessOpenAccount(msg, client);
                                        break;
                                    case MessageTypes.AccountDetailsMsgType:
                                        ProcessAccountDetails(msg, client);
                                        break;
                                    case MessageTypes.AccountListMsgType:
                                        ProcessAccountList(msg, client);
                                        break;
                                    case MessageTypes.ClientIdMsgType:
                                        ProcessAccountId(msg, client);
                                        break;
                                    case MessageTypes.TxMsgType:
                                        ProcessTransaction(msg, client);
                                        break;
                                    default:
                                        System.Diagnostics.Debug.WriteLine("Received unsupported msg type: {0} from client: {1}", msg.id, client.ClientHandle);
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
                    else
                    {
                        ++currentNumEmptyRcv;
                        if (currentNumEmptyRcv == MaxEmptyRcv)
                        {
                            client.Stop();
                        }
                    }
                }
            }
        }

        private void ProcessTransaction(MessageData data, Client client)
        {
            System.Diagnostics.Debug.WriteLine("Processing Transaction message for client {0}", client.ClientHandle);
            if (data.id != MessageTypes.TxMsgType)
            {
                throw new BankingException("Invalid Transaction Type: " + data.id + " from Client: " + client.ClientHandle);
            }
            Transaction tx = data.message as Transaction;
            if (tx != null)
            {
                switch (tx.txOperation)
                {
                    case "open":
                        {
                            string id = tx.acctId.ToString();
                            if (String.IsNullOrEmpty(id) || (String.Compare(id, "0") == 0))
                            {
                                id = BankControlForm.Bank.GetNewAcctId();
                            }
                            try
                            {
                                float balance = _bank.PerformAction(id, tx.acctLastName, tx.txOperation, tx.txAmount, tx.acctType, true);
                                if (balance >= 0)
                                {
                                    // Send response of Tx back to client
                                    Transaction txBack = new Transaction();
                                    txBack.acctFirstName = tx.acctFirstName;
                                    txBack.acctLastName = tx.acctLastName;
                                    txBack.txAmount = (float)Convert.ToDouble(tx.txAmount);
                                    txBack.acctId = Convert.ToInt32(id);
                                    txBack.acctType = tx.acctType;
                                    txBack.txOperation = "open-response";
                                    txBack.balance = balance;
                                    txBack.response = true;

                                    // The data should send across in the library sender class
                                    client.SetData(txBack);

                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("Open Tx Exception: " + e.Message);
                            }
                        }
                        break;
                    case "deposit":
                        {
                            try
                            {
                                float balance = _bank.PerformAction(tx.acctId.ToString(), tx.acctLastName, tx.txOperation, tx.txAmount, tx.acctType);
                                if (balance >= 0)
                                {
                                    Transaction txBack = new Transaction();
                                    txBack.acctFirstName = tx.acctFirstName;
                                    txBack.acctId = tx.acctId;
                                    txBack.acctLastName = tx.acctLastName;
                                    txBack.acctType = tx.acctType;
                                    txBack.balance = balance;
                                    txBack.txAmount = tx.txAmount;
                                    txBack.txOperation = "deposit-response";
                                    txBack.response = true;

                                    client.SetData(txBack);
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("Deposit Tx Exception: " + e.Message);
                            }
                        }
                        break;
                    case "withdraw":
                        {
                            try
                            {
                                float balance = _bank.PerformAction(tx.acctId.ToString(), tx.acctLastName, tx.txOperation, tx.txAmount, tx.acctType);
                                if (balance >= 0)
                                {
                                    Transaction txBack = new Transaction();
                                    txBack.acctFirstName = tx.acctFirstName;
                                    txBack.acctId = tx.acctId;
                                    txBack.acctLastName = tx.acctLastName;
                                    txBack.acctType = tx.acctType;
                                    txBack.balance = balance;
                                    txBack.txAmount = tx.txAmount;
                                    txBack.txOperation = "withdraw-response";
                                    txBack.response = true;

                                    client.SetData(txBack);
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("Withdraw Tx Exception: " + e.Message);
                            }
                        }
                        break;
                    default:
                        throw new BankingException("Invalid Transaction Name: " + data.name + " from Client: " + client.ClientHandle);
                        break;
                }
            }
        }

        private void ProcessAccountId(MessageData data, Client client)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account ID message");
        }

        private void ProcessAccountList(MessageData data, Client client)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account List message");
        }

        private void ProcessAccountDetails(MessageData data, Client client)
        {
            System.Diagnostics.Debug.WriteLine("Processing Account Details message");
        }

        private void ProcessOpenAccount(MessageData data, Client client)
        {
            System.Diagnostics.Debug.WriteLine("Processing Open Account message");
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

        public void CleanUp()
        {
            RemoveAllClients();
            listenerThread.StopLoopAction();
        }

        public void SendTransaction(Transaction details)
        {
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
