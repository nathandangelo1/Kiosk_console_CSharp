namespace Kiosk_Console_CSharp;

internal static class TransactionsList
{
    internal static List<Transaction> Transactions = new List<Transaction>();
    //internal static void PrintTransactions()
    //{
    //    //ConsoleColumnFormatter formatter = new ConsoleColumnFormatter(2, 30);
    //    foreach (var transaction in TransactionsList.Transactions)
    //    {
    //        Console.WriteLine($"Transaction Number: {transaction.transactionNumber}");
    //        Console.WriteLine($"DateTime: {transaction.transactionDateTime}");

    //        foreach (Payment payment in transaction.paymentsList)
    //        {
    //            Console.Write($"Total: {transaction.originalTotal} ");
    //            Console.Write($"Cash: {payment.cashAmount} ");
    //            Console.Write($"Vendor: {payment.ccVendor} ");
    //            Console.Write($"CC Total: {payment.ccAmount}");
    //        }
    //    }
    //}
}

