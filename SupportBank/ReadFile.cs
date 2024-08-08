using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using Newtonsoft.Json;


class ReadFile
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public static int skippedTransactionsCount = 0;

    public static string ListFiles()
    {
        int fileCount = 0;
        string selectedFile = "";
        List<string> files = new List<string>();

        List<string[]> dirs = new List<string[]>();
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.json"));
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.xml"));
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.csv"));

        Console.WriteLine("The number of files is {0}.", dirs.Count);
        foreach (string[] dir in dirs)
        {
            foreach (string file in dir)
            {
                fileCount++;
                Console.WriteLine($"{fileCount}) {file}");
                files.Add(file);
            }
        }

        Console.WriteLine("Choose a file (select its number):");
        selectedFile = files[Int32.Parse(Console.ReadLine()) - 1];

        return selectedFile;
    }

    public static List<Transaction> ReadCsv(string path)
    {
        List<Transaction> transactionsList = new List<Transaction>();
        //skippedTransactionsCount = 0;

        // Open the file using a StreamReader
        using (var reader = new StreamReader(path))
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
                        Logger.Warn($"Missing value at line {fileLine}. Please fill in all the columns of the file. Transaction at line {fileLine} was skipped.");
                        Console.WriteLine($"Missing value at line {fileLine}. Please fill in all the columns of the file. Transaction at line {fileLine} was skipped.");
                        skippedTransactionsCount++;
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

                        Logger.Info($"Created Transaction List for line {fileLine}");

                    }
                    catch (Exception e)
                    {
                        Logger.Error($"Line {fileLine} of your file causes an error: {e.Message} Transaction at line {fileLine} was skipped");
                        Console.WriteLine($"Line {fileLine} of your file causes an error: {e.Message} Transaction at line {fileLine} was skipped");

                        // transactionsList.Remove(transaction);
                        skippedTransactionsCount++;

                    }

                }

            }
        }

        return transactionsList;
    }

    public static List<Transaction> ReadJson(string path)
    {

        String getStringFromFile = File.ReadAllText(path);
        var transactionsList = JsonConvert.DeserializeObject<List<Transaction>>(getStringFromFile);

        return transactionsList;

    }

}

