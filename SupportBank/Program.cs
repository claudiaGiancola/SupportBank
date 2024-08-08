using NLog;
using NLog.Config;
using NLog.Targets;
// using System.Text.Json;
// using System.Text.Json.Serialization;
using Newtonsoft.Json;


//logging materials
var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Training\w6d2_SupportBank\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
LogManager.Configuration = config;
Logger logger = LogManager.GetCurrentClassLogger();


int skippedTransactions = 0;

List<Transaction> testRead()
{
    String getStringFromFile = File.ReadAllText("C:/Training/w6d2_SupportBank/Transactions2013.json");
    // Console.WriteLine(getStringFromFile);
    // var jsonFle = JsonConverter.DeserializeObject<List<Transaction>>(getStringFromFile);
    var transactionsList = JsonConvert.DeserializeObject<List<Transaction>>(getStringFromFile);

    return transactionsList;

}

List<Transaction> getTransactionsListFromCSV(string csvPath)
{
    List<Transaction> transactionsList = new List<Transaction>();

    // Open the file using a StreamReader
    using (var reader = new StreamReader(csvPath))
    {
        // Read the first line of the file
        var headerLine = reader.ReadLine();
        string line;
        int fileLine = 1;

        // Read the rest of the file
        while ((line = reader.ReadLine()) != null)
        {
            fileLine++;

            // Split the data line into an array of values
            var values = line.Split(',');

            Transaction transaction = new Transaction();

            bool isLineOk = true;

            foreach (var value in values)
            {
                if (value == "" || value == null)
                {
                    logger.Warn($"Missing value at line {fileLine}. Please fill in all the columns of the file. Transaction at line {fileLine} was skipped.");
                    Console.WriteLine($"Missing value at line {fileLine}. Please fill in all the columns of the file. Transaction at line {fileLine} was skipped.");
                    skippedTransactions++;
                    isLineOk = false;
                }
            }

            if (isLineOk == true)
            {

                try
                {

                    transaction.date = DateTime.Parse(values[0]);
                    transaction.FromAccount = values[1];
                    transaction.ToAccount = values[2];
                    transaction.narrative = values[3];
                    transaction.amount = float.Parse(values[4]);

                    transactionsList.Add(transaction);

                    logger.Info($"Created Transaction List for line {fileLine}");

                }
                catch (Exception e)
                {
                    logger.Error($"Line {fileLine} of your file causes an error: {e.Message} Transaction at line {fileLine} was skipped");
                    Console.WriteLine($"Line {fileLine} of your file causes an error: {e.Message} Transaction at line {fileLine} was skipped");

                    // transactionsList.Remove(transaction);
                    skippedTransactions++;

                }

                // transactionsList.Add(transaction);

                // logger.Info($"Created Transaction List for line {fileLine}");
            }

            //code below tests content of each transaction instance
            // Console.WriteLine(transaction.date + " " + transaction.from + " " + transaction.to + " " + transaction.narrative + " " + transaction.amount);
        }
    }
    return transactionsList;
}

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

    if (skippedTransactions != 0)
    {
        Console.WriteLine($"There are {skippedTransactions} transactions missing, please check logging file for more information.");
    }
    else
    {
        Console.WriteLine("No transactions missing.");
    }
}

void useSupportBank()
{
    logger.Info("Program starts");

    ReadFile.ListFiles();

    List<Transaction> transactionsList = testRead();

    //readFile and create transactions instances
    // List<Transaction> transactionsList = getTransactionsListFromCSV("C:/Training/w6d2_SupportBank/Transactions2014.csv");
    // List<Transaction> transactionsList = getTransactionsListFromCSV("C:/Training/w6d2_SupportBank/DodgyTransactions2015.csv");

    //from transactions create accounts
    // List<Account> accounts = getAccounts(transactionsList);
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