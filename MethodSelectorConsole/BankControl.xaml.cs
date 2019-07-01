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

namespace MethodSelectorConsole
{
    /// <summary>
    /// Interaction logic for BankControl.xaml
    /// </summary>
    public partial class BankControl : UserControl
    {
        public AccountNameViewModel vm = new AccountNameViewModel();
        public BankControl()
        {
            InitializeComponent();
            acctTextBox.DataContext = vm;
            errorTextBox.DataContext = vm;
            acctTypeComboBox.ItemsSource = Account.LoadAccountTypes();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                int idx = acctTypeComboBox.SelectedIndex;
                AccountType type = (AccountType)Enum.Parse(typeof(AccountType), idx.ToString());
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                if (type == AccountType.SIMPLE_CHECKING)
                {
                    BankControlForm.Bank.PerformAction(name, "deposit", amt, false, true);
                }
                else if (type == AccountType.INTEREST_CHECKING)
                {
                    BankControlForm.Bank.PerformExtendedAction(name, "accrue", amt, true);
                }
                else
                {
                    vm.ErrorString = "Unsupported Checking Type. Choose Simple or Interest Account Type.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                vm.ErrorString = ex.Message;
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                BankControlForm.Bank.PerformAction(name, "deposit", amt, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                vm.ErrorString = ex.Message;
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ErrorString = String.Empty;
            try
            {
                string name = acctTextBox.Text;
                float amt = (float)Convert.ToDouble(entryTextBox.Text);
                BankControlForm.Bank.PerformAction(name, "withdraw", amt, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                vm.ErrorString = ex.Message;
            }
        }

        private void AcctListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = (sender as ListView).SelectedIndex;
            AccountDetailsViewModel details = BankControlForm.Bank.AccountDetailsList.AccountDetailsList[idx];
            vm.ActiveAccountName = details.AccountName;
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
