namespace MethodSelectorConsole
{
    public interface IAccount
    {
        AccountDetailsViewModel AccountDetails { get; }
        string AccountName { get; }

        float AccountAction(string action, float amount);
        float AccountBalance();
        void PrintAccountDetails(int idx);
    }
}