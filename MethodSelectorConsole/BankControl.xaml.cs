using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MethodSelectorConsole
{
    /// <summary>
    /// Interaction logic for BankControl.xaml
    /// </summary>
    public partial class BankControl : UserControl
    {
        private AccountNameViewModel vm = new AccountNameViewModel();

        private static TransactionServer _server;

        public BankControl()
        {
            InitializeComponent();
            acctTextBox.DataContext = Vm;
            errorTextBox.DataContext = Vm;
            acctTypeComboBox.ItemsSource = Account.LoadAccountTypes();

            _server = new TransactionServer(BankControlForm.Bank);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            string acctId = BankControlForm.Bank.GetNewAcctId();
            Vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                int idx = acctTypeComboBox.SelectedIndex;
                AccountType type = (AccountType)Enum.Parse(typeof(AccountType), idx.ToString());
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                if (type == AccountType.SIMPLE_CHECKING)
                {
                    BankControlForm.Bank.PerformAction(acctId, name, "deposit", amt, type, true);
                }
                else if (type == AccountType.INTEREST_CHECKING)
                {
                    BankControlForm.Bank.PerformAction(acctId, name, "accrue", amt, type, true);
                }
                else if (type == AccountType.SAVINGS)
                {
                    BankControlForm.Bank.PerformAction(acctId, name, "accrue", amt, type, true);
                }
                else
                {
                    Vm.ErrorString = "Unsupported Checking Type. Choose Simple, Interest or Savings Account Type.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Vm.ErrorString = ex.Message;
            }
        }

        public AccountNameViewModel Vm
        {
            get
            {
                return vm;
            }

            private set
            {
                vm = value;
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            Vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                string id = BankControlForm.Bank.GetDetailsByName(name).AccountId;
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                BankControlForm.Bank.PerformAction(id, name, "deposit", amt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Vm.ErrorString = ex.Message;
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            Vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                string id = BankControlForm.Bank.GetDetailsByName(name).AccountId;
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                BankControlForm.Bank.PerformAction(id, name, "withdraw", amt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Vm.ErrorString = ex.Message;
            }
        }

        private void AcctListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = (sender as ListView).SelectedIndex;
            AccountDetailsViewModel details = BankControlForm.Bank.GetDetailsByIndex(idx);
            if (details != null)
            {
                Vm.ActiveAccountName = details.AccountName;
            }
            acctDetailsPanel.Visibility = Visibility.Hidden;
        }

        private void InterestButton_Click(object sender, RoutedEventArgs e)
        {
            AcctListView_SelectionChanged(acctListView, null);
            AccountDetailsViewModel vm = BankControlForm.Bank.GetDetailsByName(Vm.ActiveAccountName);
            if (vm != null)
            {
                float interest = (vm.Type == AccountType.INTEREST_CHECKING ? BankControlForm.Bank.CheckingInterest : BankControlForm.Bank.SavingsInterest);
                BankControlForm.Bank.PerformAction(vm.AccountId, Vm.ActiveAccountName, "accrue", interest);
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            string name = acctTextBox.Text;
            AccountDetailsViewModel vm = BankControlForm.Bank.GetDetailsByName(name);
            if (vm != null)
            {
                acctDetailsGrid.DataContext = vm;
                acctDetailsPanel.Visibility = Visibility.Visible;
            }
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false; // not needed
        }

        #endregion
    }
}
