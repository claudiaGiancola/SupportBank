using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

class WriteFile
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public static void ExportCsv(List<Transaction> transactionsList)
    {
        Console.WriteLine("Please type the path you want your file to be saved at:");
        string filePath = Console.ReadLine();

        Console.WriteLine("Please type your file name:");
        string fileName = Console.ReadLine();

        System.Text.StringBuilder sb = new StringBuilder();

        sb.AppendLine("DATE" + "," + "NARRATIVE" + "," + "TO" + "," + "FROM" + "," + "AMOUNT" + ",");

        foreach (Transaction T in transactionsList)
        {
            sb.Append(T.date.ToShortDateString());
            sb.Append(",");
            sb.Append(T.FromAccount);
            sb.Append(",");
            sb.Append(T.ToAccount);
            sb.Append(",");
            sb.Append(T.narrative);
            sb.Append(",");
            sb.Append(T.amount.ToString());
            sb.AppendLine("");
        }

        string csvpath = $@"{filePath}\{fileName}.csv";

        if (File.Exists(csvpath))
        {
            File.Delete(csvpath);
        }

        File.WriteAllText(csvpath, sb.ToString());

        Console.WriteLine("Your file has been successfully exported.");
        Logger.Info("Your file has been successfully exported.");

    }


}