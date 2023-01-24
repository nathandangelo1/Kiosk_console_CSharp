
using System.Diagnostics;
using System.Transactions;

namespace Kiosk_Console_CSharp;
internal class Transaction
{
    internal Guid transactionNumber;
    internal DateTime transactionDateTime;
    internal decimal totalCashReceived;
    internal decimal totalCCreceived;
    internal decimal changeOwed;
    internal decimal changeDispensed;
    internal bool insufficientChange;

    internal bool IsCBrequested;
    internal decimal cashBackReqAmount;
    internal decimal cashBackDispensed;
    internal decimal cashBackOwed;
    internal decimal originalTotal;
    internal decimal balance;

    public List<Payment> paymentsList = new();

    internal Transaction(decimal atotal, Guid atransactionNumber)
    {
        decimal roundedTotal = decimal.Round(atotal, 2, MidpointRounding.AwayFromZero);
        originalTotal = roundedTotal;
        transactionNumber = atransactionNumber;
        transactionDateTime = DateTime.Now;
        balance = roundedTotal;
    }
    internal void TransactionAddItem(decimal atotal)
    {
        originalTotal += atotal;
        balance += atotal;
    }
    internal static void CloseTransaction(Transaction transaction, decimal totalPayments)
    {
        // VERIFIES TOTAL PAYMENTS COVER TOTAL OWED
        if (totalPayments >= transaction.originalTotal + transaction.cashBackReqAmount)
        {
            if (transaction.IsCBrequested) // IF CASHBACK REQUESTED, RECORD AMOUNT *OWED* (-NEG) (NOT YET DISPENSED)
            {
                transaction.cashBackOwed -= transaction.cashBackReqAmount; // CB AND CHANGE ARE PARALLEL
                transaction.changeOwed -= transaction.cashBackReqAmount;
            }
            else // IF CB NOT REQUESTED, RECORD CHANGE OWED(-NEG), ( MOVES NEGATIVE BALANCE FROM BALANCE TO CHANGE OWED)
            {
                transaction.changeOwed -= totalPayments - transaction.originalTotal;
                transaction.balance += Math.Abs(transaction.changeOwed); // ZEROS OUT
            }
        }
        // IF CHANGE IS OWED...
        if (transaction.changeOwed < 0) 
        {
            bool changeAndCashBackSuccess = ChangeAndCashBack(transaction); // CALCULATE AND DISPENSE CHANGE

            if (changeAndCashBackSuccess) // IF NO ERRORS DISPENSING CHANGE
            {
                Program.Wait(text: "\nTransaction Completed. Thank You.");
            }
            else // IF INSUFFICIENT CHANGE, TRANSACTION RECORDS SHOULD SHOW CHANGE STILL OWED
            {
                InsuffChange(transaction);
            }

        }
        else // IF NO CHANGE OR CB OWED...
        {
            Program.Wait(text:"\nTransaction Completed.");
        }
    }
    static bool ChangeAndCashBack(Transaction transaction)
    {
        bool changeGiven = false;
        bool changeCounted = false;

        if (transaction.changeOwed < 0) // IF CHANGE OWED.. (neg)
        {
            decimal changeTotal = Math.Abs(transaction.changeOwed); // GET ABS(pos) VALUE

            int[] changeCounts = new int[13];

            changeCounted = CashDrawer.GetChangeCounts(changeCounts, changeTotal); // GET COUNT FOR EACH DENOM TO BE DISPENSED, RETURNS TRUE IF SUFFICIENT CHANGE

            if (changeCounted) // IF SUFFICIENT CHANGE
            {
                changeGiven = GiveChange(transaction, changeCounts); // DISPENSE CHANGE
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

    
    static bool GiveChange(Transaction transaction, int[] changeCount)
    { // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER, RETURNS

        for (int i = 0; i < changeCount.Length; i++)
        {
            if (changeCount[i] > 0)
            {
                decimal dispensedAmount = changeCount[i] * CashDrawer.cashInDrawer[i].decValue;

                CashDrawer.DeductFromCashInDrawer(dispensedAmount, i);

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

                for (int j = 0; j < changeCount[i]; j++)
                {
                    Program.Wait(1000, text: $"{CashDrawer.values[i]} dispensed");
                }
            }
        }
        Program.Wait(text: "\nDon't forget to take your change.");

        if (transaction.balance == 0 && transaction.changeOwed == 0 && transaction.cashBackOwed == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    internal static void InsuffChange(Transaction transaction)
    {// IF INSUFFICIENT CHANGE, TRANSACTION RECORDS SHOULD SHOW CHANGE STILL OWED
        
        Program.Wait(4000, "Insufficient Change, Please find another method of payment");
        transaction.insufficientChange = true;
        transaction.changeOwed = -(transaction.totalCashReceived);
        for (int i = 0; i <= transaction.paymentsList.Count; i++)
        {
            Payment payment = transaction.paymentsList[i];
            if(payment.paymentType == PaymentType.Cash)
            {;
                transaction.paymentsList.RemoveAt(i);
            }
        }

        int[] changeCounts1 = new int[13];
        CashDrawer.GetChangeCounts(changeCounts1, Math.Abs(transaction.changeOwed));
        GiveChange(transaction, changeCounts1);
        
        transaction.balance = transaction.originalTotal - transaction.totalCCreceived;
        
        Program.ManagePayments(transaction); // GETS PAYMENTS

        decimal totalPayments = Program.TallyTotalPayments(transaction.paymentsList); // GETS TOTAL OF ALL PAYMENTS FOR VERIFICATION

        Transaction.CloseTransaction(transaction, totalPayments); // 

        //TransactionsList.Transactions.Add(transaction);

        //Transaction.TransactionLogging(transaction);
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
                transCC += payment.ccVendor.ToString() + "-" + payment.ccAmount.ToString();
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

        ProcessStartInfo startInfo = new()
        {
            FileName = @"C:\Users\POBOYINSAMSARA\source\repos\Kiosk_console_CSharp\Kiosk_Logging_CSharp\Kiosk_Logging_CSharp\bin\Debug\net6.0\Kiosk_Logging_CSharp.exe",
            Arguments = arg
        };
        Process.Start(startInfo);
    }
}

