using System.Diagnostics;


namespace Kiosk_Console_CSharp;

public class Transaction
{
    internal Guid transactionNumber;
    internal DateTime transactionDateTime;
    internal decimal totalCashReceived;
    internal decimal totalCCreceived;
    internal decimal changeOwed;
    internal decimal changeDispensed;

    internal bool IsCBrequested;
    internal decimal cashBackReqAmount;
    internal decimal cashBackDispensed;
    internal decimal cashBackOwed;
    internal decimal originalTotal;
    internal decimal balance;

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
    public void Closing(Transaction transaction, decimal totalPayments, CashDrawer drawer)
    {
        if (totalPayments >= transaction.originalTotal + transaction.cashBackReqAmount)
        {
            if (transaction.IsCBrequested)
            {
                transaction.cashBackOwed -= transaction.cashBackReqAmount;
                transaction.changeOwed -= transaction.cashBackReqAmount;
            }
            else
            {
                transaction.changeOwed -= totalPayments - transaction.originalTotal;
                transaction.balance += Math.Abs(transaction.changeOwed);
            }
        }

        if (transaction.changeOwed < 0)
        {
            bool changeAndCashBackSuccess = ChangeAndCashBack(transaction, drawer);

            if (changeAndCashBackSuccess)
            {
                WaitForKeyorSeconds(6, "\nTransaction Completed.");
            }
            else
            {
                Console.WriteLine("\nDispensing Error: TransactionFunction");
            }
        }
        else
        {
            WaitForKeyorSeconds(3, "\nTransaction Completed.");
        }
    }
    static bool ChangeAndCashBack(Transaction transaction, CashDrawer drawer)
    {
        bool insuffChange = false;
        bool changeGiven = false;
        bool changeCounted = false;

        if (transaction.changeOwed < 0)
        {
            decimal changeTotal = transaction.changeOwed;

            changeTotal = Math.Abs(changeTotal);

            int[] changeCounts = new int[13];
            changeCounted = drawer.GetChangeCounts(changeCounts, changeTotal, drawer);

            if (changeCounted)
            {
                changeGiven = GiveChange(transaction, drawer, changeCounts);
            }
            else
            {
                Console.WriteLine("Insufficient Change");
                insuffChange = true;
            }
            if (insuffChange)
            {
                Console.WriteLine("Alternatives...");
            }
        }
        if (changeCounted && changeGiven)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    static bool GiveChange(Transaction transaction, CashDrawer drawer, int[] changeCount) // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER
    {
        for (int i = 0; i < changeCount.Length; i++)
        {
            if (changeCount[i] > 0)
            {
                decimal dispensedAmount = changeCount[i] * drawer.values[i];

                // drawer.cashInDrawer[i] -= dispensedAmount;
                drawer.DeductCashInDrawer(drawer, dispensedAmount, i);

                if (transaction.IsCBrequested)
                {
                    transaction.cashBackOwed += dispensedAmount;
                    transaction.cashBackDispensed += dispensedAmount;
                    transaction.changeOwed += dispensedAmount;
                }
                else
                {
                    transaction.changeOwed += dispensedAmount;
                    transaction.changeDispensed += dispensedAmount;
                }

                var CP = Console.GetCursorPosition();
                for (int j = 0; j < changeCount[i]; j++)
                {
                    WaitForKeyorSeconds(.5, $"{drawer.values[i]} dispensed");
                }
            }
        }
        WaitForKeyorSeconds(text: "\nDon't forget to take your change.");
        if (transaction.balance == 0 && transaction.changeOwed == 0 && transaction.cashBackOwed == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    static void WaitForKeyorSeconds(double seconds = 5.0, string text = "")
    {
        Console.WriteLine(text);
        Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(seconds));
    }

    public static void TransactionLogging(Transaction transaction)
    {
        string transNumber;
        string transDate;
        string transTime;
        string transCash;
        string transCC = "";
        string transChange;

        transNumber = transaction.transactionNumber.ToString();
        transDate = transaction.transactionDateTime.ToString("d");
        transTime = transaction.transactionDateTime.ToString("T");
        transCash = transaction.totalCashReceived.ToString();
        transChange = (transaction.cashBackDispensed + transaction.changeDispensed).ToString();

        foreach (Payment payment in transaction.paymentsList)
        {
            if (payment.paymentType == PaymentType.Card && payment.declined == false)
            {
                transCC += payment.ccVendor.ToString() + payment.ccAmount.ToString();
            }
            if (transaction.paymentsList.Count > 1)
            {
                if (payment.Equals(PaymentType.Card))
                {
                    for (int i = 0; i < transaction.paymentsList.Count - 1; i++)
                    {
                        transCC += "-";
                    }
                }
            }
        }
        string arg = transNumber + " " + transDate + " " + transTime + " " + transCash + " " + transChange + " " + transCC;

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Users\natha\source\repos\Kiosk_console_CSharp\Kiosk_Logging_CSharp\Kiosk_Logging_CSharp\bin\Debug\net6.0\Kiosk_Logging_CSharp.exe";
        startInfo.Arguments = arg;
        Process.Start(startInfo);
    }
}

