// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

List<Transaction> transactionsList = new List<Transaction>();

// Open the file using a StreamReader
using (var reader = new StreamReader("C:/Training/w6d2_SupportBank/Transactions2014.csv"))
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

// List<string> uniqueNames = new List<string>();

// foreach (var transaction in transactionsList)
// {
    //check each from and to and print them uniquely
    // if (uniqueNames.Contains(transaction.from)) {
    //     uniqueNames.Add(transaction.from);
    // }

    // if (uniqueNames.Contains(transaction.to)) {
    //     uniqueNames.Add(transaction.to);
    // }
    // }
    
List<string> userFromList = transactionsList.Select(x => x.from).Distinct().ToList();
List<string> userToList = transactionsList.Select(x => x.to).Distinct().ToList();

List<string> uniqueUsers = userFromList.Union(userToList).ToList();