namespace Kiosk_Console_CSharp;

internal static class TransactionsList
{
    internal static List<Transaction> Transactions = new List<Transaction>();
    internal static void PrintTransactions()
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

