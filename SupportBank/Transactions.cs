using NLog;
using NLog.Config;
using NLog.Targets;

class Transaction
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public DateTime date { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public string narrative { get; set; }
    public float amount { get; set; }

}

class TransactionList
{

    public static void addNewTransaction(List<Transaction> transactionsList){
        //
        Transaction transaction= new Transaction();

        Console.WriteLine("Enter the date DD/MM/YYYY:");
        transaction.date= DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Enter the name of the person who lent money:");
        transaction.FromAccount= Console.ReadLine();
        Console.WriteLine("Enter the name of the person who borrowed money:");
        transaction.ToAccount= Console.ReadLine();
        Console.WriteLine("Enter a description for this transaction:");
        transaction.narrative= Console.ReadLine();
        Console.WriteLine("Enter the amount of money lent/borrowed:");
        transaction.amount= float.Parse(Console.ReadLine());

        transactionsList.Add(transaction);
    }
    
}