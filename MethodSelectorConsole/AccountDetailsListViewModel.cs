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

        public AccountDetailsListViewModel()
        {
            AccountDetailsList = new ObservableCollection<AccountDetailsViewModel>();
        }
    }
}
