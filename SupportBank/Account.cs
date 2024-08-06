class Account {
    string name;
    private float MoneyBorrowed { get; set; }
    private float MoneyLanded { get; set; }
    List<Transaction> getTransactionsFromAccount(){
        //looks at all transaction objects and selects only those with this account's name in "from"
        //puts them together in a list and returns it
        List<Transaction> listTransactionsFromAccount = new List<Transaction>();
        return listTransactionsFromAccount;
    }
    List<Transaction> getTransactionsToAccount() {
        List<Transaction> listTransactionsToAccount = new List<Transaction>();
        return listTransactionsToAccount;
    }
}

