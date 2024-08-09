using NLog;
using NLog.Config;
using NLog.Targets;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

//logging materials
var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Training\w6d2_SupportBank\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
LogManager.Configuration = config;
Logger logger = LogManager.GetCurrentClassLogger();


void useSupportBank()
{

    logger.Info("Program starts");

    //List files in the current path of the project
    string selectedFilePath = ReadFile.ListFiles();

    //Allows the user to choose a file from the previous list printed and stores data in transactionList
    List<Transaction> transactionsList = ReadFile.readSelectedFile(selectedFilePath);

    //Get number of transactions lines skipped
    ReadFile.getSkippedTransactionsCount();

    //Allow the user to add more transactions to the transaction list
    string customTransactions = "";
    int newTransactionsCount = 0;

    do
    {
        Console.WriteLine("Do you want to add a custom transaction? (Y/N)");
        customTransactions = Console.ReadLine();
        if (customTransactions == "Y")
        {
            newTransactionsCount++;
            TransactionList.addNewTransaction(transactionsList);
        }
        else { 
            customTransactions = "N"; 
            Console.WriteLine($"You added {newTransactionsCount} transactions");
        }
    } while (customTransactions == "Y");


    //Create unique accounts for all the people in the transaction list
    List<Account> accounts = Account.getAccounts(transactionsList);

    //Allows the user to print all the transactions of the data file, or only transaction for a specific account
    Console.WriteLine("Would you like to list ALL transactions [ALL]");
    Console.WriteLine("Or the transactions for a specific unique ACCOUNT? ([Name Surname])");
    string command = Console.ReadLine();
    if (command == "ALL")
    {
        Account.listAll(accounts);
    }
    else
    {
        Account.listUser(accounts, command, transactionsList);
    }

    // allows the user to choose to export the data previously printed 
    Console.WriteLine("Do you want to export your file? (Y/N)");
    string continueToExport = Console.ReadLine();
    if (continueToExport == "Y")
    {
        WriteFile.ExportCsv(transactionsList);
    }
    else
    {
        Console.WriteLine("Goodbye!");
    }

    logger.Info("Program ends");

}

useSupportBank();