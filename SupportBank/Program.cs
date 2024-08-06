// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

List<Transaction> getTransactionsListFromCSV(string csvPath)
{
    List<Transaction> transactionsList = new List<Transaction>();

    // Open the file using a StreamReader
    using (var reader = new StreamReader(csvPath))
    {
        // Read the first line of the file
        var headerLine = reader.ReadLine();
        string line;

        // Read the rest of the file
        while ((line = reader.ReadLine()) != null)
        {

            // Split the data line into an array of values
            var values = line.Split(',');

            Transaction transaction = new Transaction();
            {
                transaction.date = values[0];
                transaction.from = values[1];
                transaction.to = values[2];
                transaction.narrative = values[3];
                transaction.amount = float.Parse(values[4]);
            };
            transactionsList.Add(transaction);

            //code below tests content of each transaction instance
            // Console.WriteLine(transaction.date + " " + transaction.from + " " + transaction.to + " " + transaction.narrative + " " + transaction.amount);
        }
    }
    return transactionsList;
}

List<Account> getAccounts(List<Transaction> transactionsList)
{
    List<string> userFromList = transactionsList.Select(x => x.from).Distinct().ToList();
    List<string> userToList = transactionsList.Select(x => x.to).Distinct().ToList();

    List<string> uniqueUsers = userFromList.Union(userToList).ToList();

    List<Account> accounts = new List<Account>();

    foreach (var user in uniqueUsers)
    {
        Account account = new Account();

        {
            account.name = user;

            account.MoneyBorrowed = account.getTransactionsBorrowed(transactionsList).Select(x => x.amount).Sum();

            account.MoneyLent = account.getTransactionsLent(transactionsList).Select(x => x.amount).Sum();

            accounts.Add(account);
            //Console.WriteLine($"{account.name} has borrowed {account.MoneyBorrowed} and lent {account.MoneyLent}");

        };

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

    Console.WriteLine($"{account.name} borrowed:");
    foreach (var transaction in account.getTransactionsLent(transactionsList))
    {
        Console.WriteLine(transaction.date + " " + transaction.from + " " + transaction.to + " " + transaction.narrative + " " + transaction.amount);
    }

    Console.WriteLine($"{account.name} lent:");
    foreach (var transaction in account.getTransactionsBorrowed(transactionsList))
    {
        Console.WriteLine(transaction.date + " " + transaction.from + " " + transaction.to + " " + transaction.narrative + " " + transaction.amount);
    }

}

void useSupportBank()
{

    //readFile and create transactions instances
    List<Transaction> transactionsList = getTransactionsListFromCSV("C:/Training/w6d2_SupportBank/Transactions2014.csv");

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
}

useSupportBank();