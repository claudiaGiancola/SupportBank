class Account
{
    public string name;
    public float MoneyBorrowed { get; set; }
    public float MoneyLent { get; set; }
    public List<Transaction> getTransactionsLent(List<Transaction> transactionsList)
    {
        List<Transaction> listTransactionsLent = new List<Transaction>();

        foreach (Transaction transaction in transactionsList)
        {
            if (transaction.from == this.name)
            {
                listTransactionsLent.Add(transaction);
            }
        }

        return listTransactionsLent;
    }
    public List<Transaction> getTransactionsBorrowed(List<Transaction> transactionsList)
    {
        List<Transaction> listTransactionsBorrowed = new List<Transaction>();

        foreach (Transaction transaction in transactionsList)
        {
            if (transaction.to == this.name)
            {
                listTransactionsBorrowed.Add(transaction);
            }
        }

        return listTransactionsBorrowed;
    }
}

