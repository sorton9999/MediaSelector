using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodSelectorConsole
{
    public class AccountDetailsListViewModel : ViewModelBase
    {
        public ObservableCollection<AccountDetailsViewModel> AccountDetailsList = new ObservableCollection<AccountDetailsViewModel>();

        public static Action<AccountDetailsViewModel> addAction;

        public AccountDetailsListViewModel()
        {
            AccountDetailsList = new ObservableCollection<AccountDetailsViewModel>();
            addAction = new Action<AccountDetailsViewModel>(Add);
        }

        public void Add(AccountDetailsViewModel item)
        {
            try
            {
                AccountDetailsList.Add(item);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
