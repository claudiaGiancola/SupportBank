// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

List<Transaction> transactionsList = new List<Transaction>();

// Open the file using a StreamReader
using (var reader = new StreamReader("../Transactions2014.csv"))
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

        // foreach (var value in values)
        // {
        //     Console.Write(value + " ");
        // }

        Console.WriteLine(transaction.date + " " + transaction.from + " " + transaction.to + " " + transaction.narrative + " " + transaction.amount);
    }
}