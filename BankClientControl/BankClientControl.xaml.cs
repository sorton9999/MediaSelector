using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonClasses;
using TcpLib;
using CliServLib;

namespace BankClientControl
{
    /// <summary>
    /// Interaction logic for BankClientControl.xaml
    /// </summary>
    public partial class BankClientControl : UserControl
    {
        AccountDetailsViewModel Vm = new AccountDetailsViewModel();
        ObservableCollection<AccountDetailsViewModel> acctList = new ObservableCollection<AccountDetailsViewModel>();
        public static readonly DependencyProperty BankProperty =
            DependencyProperty.Register(
            "BankClient", typeof(BankClient), typeof(BankClientControl), null);

        public static Action<MessageData> dataAction;

        public BankClientControl()
        {
            InitializeComponent();

            // Test Vm
            //AccountDetailsViewModel details = new AccountDetailsViewModel();
            //details.AccountId = "6666";
            //details.AccountName = "Dr. Doolittle";
            //details.Balance = 100.00F;
            //details.Type = AccountType.SAVINGS;

            //Vm.AccountId = details.AccountId;
            //Vm.AccountName = details.AccountName;
            //Vm.Balance = details.Balance;
            //Vm.Type = details.Type;

            //acctList.Add(details);

            DetailsStack.DataContext = Vm;
            listView.ItemsSource = acctList;
            acctTypeCB.ItemsSource = AccountDetailsClass.LoadAccountTypes();

            dataAction = new Action<MessageData>(AddData);
        }

        public BankClient BankClient
        {
            get { return (BankClient)GetValue(BankProperty); }
            set { SetValue(BankProperty, value); }
        }

        public string ReceiveMsgs
        {
            get { return Vm.StrOut; }
            set { Vm.StrOut = value; }
        }

        private void AddData(MessageData data)
        { 
            System.Diagnostics.Debug.WriteLine("Processing Message Type: {0}", data.id);
            if ((data == null) || (data.id <= 0) || (data.message == null))
            {
                return;
            }
            switch (data.id)
            {
                case MessageTypes.AccountDetailsMsgType:
                    break;
                case MessageTypes.AccountListMsgType:
                    break;
                case MessageTypes.ClientIdMsgType:
                    break;
                case MessageTypes.OpenAcctMsgType:
                    break;
                case MessageTypes.TxMsgType:
                    {
                        Transaction tx = data.message as Transaction;
                        if (tx != null)
                        {
                            switch (tx.txOperation)
                            {
                                case "open-response":
                                    {
                                        AccountDetailsViewModel details = new AccountDetailsViewModel();
                                        details.AccountId = tx.acctId.ToString();
                                        details.AccountName = tx.acctLastName;
                                        details.Balance = tx.balance;
                                        details.Type = tx.acctType;

                                        acctList.Add(details);
                                    }
                                    break;
                                case "deposit-response":
                                    {
                                        AccountDetailsViewModel details = FindAccount(tx.acctId.ToString());
                                        if (details != default(AccountDetailsViewModel))
                                        {
                                            details.Balance = tx.balance;
                                        }
                                    }
                                    break;
                                case "withdraw-response":
                                    {
                                        AccountDetailsViewModel details = FindAccount(tx.acctId.ToString());
                                        if (details != default(AccountDetailsViewModel))
                                        {
                                            details.Balance = tx.balance;
                                        }
                                    }
                                    break;
                                default:
                                    System.Diagnostics.Debug.WriteLine("Unsupported Transaction Type: {0}", tx.txOperation);
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Unsupported Msg Type {0}", data.id);
                    break;
            }
        }

        private AccountDetailsViewModel FindAccount(string acctId)
        {
            AccountDetailsViewModel details = default(AccountDetailsViewModel);
            var item = acctList.ToLookup( s => s.AccountId == acctId );
            foreach (var i in item[true])
            {
                details = i;
            }
            return details;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            if (list != null)
            {
                int idx = list.SelectedIndex;
                AccountDetailsViewModel details = acctList[idx];
                Vm.AccountId = details.AccountId;
                Vm.AccountName = details.AccountName;
                Vm.Balance = details.Balance;
                Vm.Type = details.Type;
            }
        }

        private void OpenAcctBtn_Click(object sender, RoutedEventArgs e)
        {
            string first = firstNameTextBox.Text;
            string last = lastNameTextBox.Text;
            string deposit = depositTextBox.Text;

            // We need this dispatcher to update the ObserverCollection from the BankClient.
            // The transactions are off the main thread.
            if (BankClient.Dispatcher == null)
            {
                BankClient.Dispatcher = this.Dispatcher;
            }

            OpenAcctDataGetter getter = new OpenAcctDataGetter();
            OpenAcctDataGetter.OpenAcctData data = new OpenAcctDataGetter.OpenAcctData();
            AccountType typeVal = AccountType.UNINIT;
            if (!Enum.TryParse(acctTypeCB.SelectedIndex.ToString(), out typeVal))
            {
                typeVal = AccountType.OTHER;
            }
            data.acctType = typeVal;
            data.deposit = (float)Convert.ToDouble(deposit);
            data.firstName = first;
            data.lastName = last;

            getter.SetData(data);


            // The data should send across in the library sender class
            BankClient.TcpClient().SetData(data, getter);
        }

        private void WithdrawBtn_Click(object sender, RoutedEventArgs e)
        {
            int idx = listView.SelectedIndex;
            if (idx >= 0)
            {
                AccountDetailsViewModel details = acctList[idx];
                TxDataGetter getter = new TxDataGetter();
                Transaction tx = new Transaction();
                tx.acctLastName = details.AccountName;
                tx.acctId = Convert.ToInt32(details.AccountId);
                tx.acctType = details.Type;
                tx.balance = details.Balance;
                tx.txAmount = (float)Convert.ToDouble(depositTextBox.Text);
                tx.txOperation = "withdraw";

                BankClient.TcpClient().SetData(tx, getter);
            }
        }

        private void DepositBtn_Click(object sender, RoutedEventArgs e)
        {
            int idx = listView.SelectedIndex;
            if (idx >= 0)
            {
                AccountDetailsViewModel details = acctList[idx];
                TxDataGetter getter = new TxDataGetter();
                Transaction tx = new Transaction();
                tx.acctLastName = details.AccountName;
                tx.acctId = Convert.ToInt32(details.AccountId);
                tx.acctType = details.Type;
                tx.balance = details.Balance;
                tx.txAmount = (float)Convert.ToDouble(depositTextBox.Text);
                tx.txOperation = "deposit";

                BankClient.TcpClient().SetData(tx, getter);
            }
        }
    }
}
