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
            //OpenAcctDataGetter getter = new OpenAcctDataGetter(first, last, deposit);
            TxDataGetter getter = new TxDataGetter();
            //AccountDetailsModel details = new AccountDetailsModel();
            //details.accountBalance = (float)Convert.ToDouble(deposit);
            //details.accountName = first;
            //getter.AccountDetails = details;

            Transaction tx = new Transaction();
            tx.acctFirstName = first;
            tx.acctLastName = last;
            tx.txAmount = (float)Convert.ToDouble(deposit);
            tx.txOperation = "open";

            getter.TransactionDetails = tx;

            // The data should send across in the library sender class
            BankClient.TcpClient().SetData(tx);

            /*
            var eventData = GetDataAsync.GetMessageDataAsync(getter, MessageTypes.OpenAcctMsgType);
            if (eventData != null)
            {
                try
                {
                    eventData.Result.name = "openacct";

                    var sendResult = SendMessageAsync.SendMsgAsync(BankClient.ClientSocket, eventData.Result);

                    if (sendResult.Result.Failure)
                    {
                        System.Diagnostics.Debug.WriteLine("There was a problem sending acct message to the server.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Send Exception: " + ex.Message);
                }
            }
            */

        }
    }
}
