using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses;
using System.Windows.Data;

namespace MethodSelectorConsole
{
    public class AccountDetailsListViewModel : ViewModelBase
    {
        private ObservableCollection<AccountDetailsViewModel> accountDetailsList;

        private readonly object _collectionLock = new object();

        // Called by GUI thread dispatcher
        public static Action<AccountDetailsViewModel> addAction;

        public AccountDetailsListViewModel()
        {
            accountDetailsList = new ObservableCollection<AccountDetailsViewModel>();
            addAction = new Action<AccountDetailsViewModel>(Add);
            BindingOperations.EnableCollectionSynchronization(accountDetailsList, _collectionLock);
        }

        public ObservableCollection<AccountDetailsViewModel> AccountDetailsList
        {
            get { return accountDetailsList; }
            set
            {
                accountDetailsList = value;
            }
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
