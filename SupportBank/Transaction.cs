using NLog;
using NLog.Config;
using NLog.Targets;

class Transaction
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public string date { get; set; }
    public string from { get; set; }
    public string to { get; set; }
    public string narrative { get; set; }
    public float amount { get; set; }

    // To KEEP :
    // private string _date;

    // public string GetDate()
    // {
    //     return _date;
    // }

    // private void SetDate(string newDate)
    // {
    //     _date = newDate;
    // }

}