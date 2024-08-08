using NLog;
using NLog.Config;
using NLog.Targets;
// using System.Text.Json;
// using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


//logging materials
var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Training\w6d2_SupportBank\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
LogManager.Configuration = config;
Logger logger = LogManager.GetCurrentClassLogger();


List<Account> getAccounts(List<Transaction> transactionsList)
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

void listAll(List<Account> accounts)
{

    foreach (var account in accounts)
    {
        Console.WriteLine($"{account.name} has borrowed {account.MoneyBorrowed} and lent {account.MoneyLent}");
    };
}

void listUser(List<Account> accounts, string accountName, List<Transaction> transactionsList)
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

void listSkippedTransactions()
{
    //just creating a line for better readability
    Console.WriteLine("***********");

    if (ReadFile.skippedTransactionsCount != 0)
    {
        Console.WriteLine($"There are {ReadFile.skippedTransactionsCount} transactions missing, please check logging file for more information.");
    }
    else
    {
        Console.WriteLine("No transactions missing.");
    }
}


List<Transaction> readSelectedFile(string selectedFilePath)
{
    //get the extension from selectedFilePath

    string pattern = @"(\.[^.]+)$";
    Regex rg = new Regex(pattern);
    Match m = rg.Match(selectedFilePath);

    List<Transaction> transactionsList = new List<Transaction>();

    switch (m.Value)
    {
        case ".json":
            transactionsList = ReadFile.ReadJson(selectedFilePath);
            break;
        case ".xml":
            transactionsList = ReadFile.ReadXml(selectedFilePath);
            break;
        case ".csv":
            transactionsList = ReadFile.ReadCsv(selectedFilePath);
            break;
        default:
            // code block
            break;
    }

    return transactionsList;
}

void useSupportBank()
{

    logger.Info("Program starts");

    string selectedFilePath = ReadFile.ListFiles();

    //readFile and create transactions instances
    List<Transaction> transactionsList = readSelectedFile(selectedFilePath);

    //from transactions create accounts
    List<Account> accounts = getAccounts(transactionsList);

    //user selects if printing all results or results for a specific user
    Console.WriteLine("Would you like to list ALL transactions (ALL)");
    Console.WriteLine("Or the transactions for a specific unique ACCOUNT? ([NAME])");
    string command = Console.ReadLine();

    if (command == "ALL")
    {
        listAll(accounts);
    }
    else
    {
        listUser(accounts, command, transactionsList);
    }

    listSkippedTransactions();
}

useSupportBank();