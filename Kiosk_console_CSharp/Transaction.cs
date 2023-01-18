namespace Kiosk_Console_CSharp;
    static class TransactionsList
    {
        public static List<Transaction> Transactions = new List<Transaction>();
    public static void PrintTransactions()
    {
        ConsoleColumnFormatter formatter = new ConsoleColumnFormatter(2, 30);
        foreach (var transaction in TransactionsList.Transactions)
        {
            Console.WriteLine($"Transaction Number: {transaction.transactionNumber}");
            Console.WriteLine($"DateTime: {transaction.transactionDateTime}");

            foreach (Payment payment in transaction.paymentsList)
            {
                formatter.Write($"Total: {transaction.originalTotal}");
                formatter.Write($"Cash: {payment.cashAmount}");
                formatter.Write($"Vendor: {payment.ccVendor}");
                formatter.Write($"CC Total: {payment.ccAmount}");
            }
        }
    }
}

public class Transaction
    {
        public Guid transactionNumber;
        public DateTime transactionDateTime;
        public decimal totalCashReceived;
        public decimal totalCCreceived;
        public decimal changeOwed;
        public decimal changeDispensed;

        public bool IsCBrequested;
        public decimal cashBackReqAmount;
        public decimal cashBackDispensed;
        public decimal cashBackOwed;
        public decimal originalTotal;
        public decimal balance;

        public List<Payment> paymentsList = new();

        public Transaction(decimal atotal, Guid atransactionNumber)
        {
            originalTotal = atotal;
            transactionNumber = atransactionNumber;
            transactionDateTime = DateTime.Now;
            balance = atotal;
        }
        public void TransactionAdd(decimal atotal)
        {
            originalTotal += atotal;
            balance += atotal;
        }
    
}

