using NLog;
using NLog.Config;
using NLog.Targets;

class Account
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public string name;
    public float MoneyBorrowed { get; set; }
    public float MoneyLent { get; set; }
    public List<Transaction> getTransactionsLent(List<Transaction> transactionsList)
    {
        List<Transaction> listTransactionsLent = new List<Transaction>();

        foreach (Transaction transaction in transactionsList)
        {
            if (transaction.FromAccount == this.name)
            {
                listTransactionsLent.Add(transaction);
            }
        }
        // Logger.Info("Created Transaction Lent list");
        return listTransactionsLent;
    }
    public List<Transaction> getTransactionsBorrowed(List<Transaction> transactionsList)
    {
        List<Transaction> listTransactionsBorrowed = new List<Transaction>();

        foreach (Transaction transaction in transactionsList)
        {
            if (transaction.ToAccount == this.name)
            {
                listTransactionsBorrowed.Add(transaction);
            }
        }
        // Logger.Info("Created Transaction Borrowed list");
        return listTransactionsBorrowed;
    }

    public static List<Account> getAccounts(List<Transaction> transactionsList)
    {
        List<string> userFromList = transactionsList.Select(x => x.FromAccount).Distinct().ToList();
        List<string> userToList = transactionsList.Select(x => x.ToAccount).Distinct().ToList();

        List<string> uniqueUsers = userFromList.Union(userToList).ToList();

        List<Account> accounts = new List<Account>();

        foreach (var user in uniqueUsers)
        {
            Account account = new Account();

            account.name = user;

            account.MoneyBorrowed = account.getTransactionsBorrowed(transactionsList).Select(x => x.amount).Sum();

            account.MoneyLent = account.getTransactionsLent(transactionsList).Select(x => x.amount).Sum();

            accounts.Add(account);
            //Console.WriteLine($"{account.name} has borrowed {account.MoneyBorrowed} and lent {account.MoneyLent}");

        }
        return accounts;
    }

    public static void listAll(List<Account> accounts)
    {

        foreach (var account in accounts)
        {
            Console.WriteLine($"{account.name} has borrowed {account.MoneyBorrowed} and lent {account.MoneyLent}");
        };
    }

    public static void listUser(List<Account> accounts, string accountName, List<Transaction> transactionsList)
    {
        //filter accounts with accountName
        List<Account> thisAccount = accounts.Where(p => p.name == accountName).ToList();
        Account account = thisAccount[0];

        Console.WriteLine($"{account.name} has borrowed {account.MoneyBorrowed} and lent {account.MoneyLent}");
        Console.WriteLine($"List of {account.name}'s transactions:");

        Console.WriteLine($"{account.name} lent:");
        foreach (var transaction in account.getTransactionsLent(transactionsList))
        {
            Console.WriteLine(transaction.date.ToShortDateString() + " " + transaction.FromAccount + " " + transaction.ToAccount + " " + transaction.narrative + " " + transaction.amount);
        }

        Console.WriteLine($"{account.name} borrowed:");
        foreach (var transaction in account.getTransactionsBorrowed(transactionsList))
        {
            Console.WriteLine(transaction.date.ToShortDateString() + " " + transaction.FromAccount + " " + transaction.ToAccount + " " + transaction.narrative + " " + transaction.amount);
        }

    }
}

