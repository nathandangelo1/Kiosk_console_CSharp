using System.Globalization;

namespace Kiosk_Console_CSharp;
public class Payment
{
    internal Guid transactionNumber;
    public DateTime datetime;

    public decimal cashAmount;

    public PaymentType paymentType;
    public bool success;

    public CreditCardFunctions.CreditCardType? ccVendor;
    public decimal ccAmount;
    public bool declined;
    public decimal declinedAmount;
    public bool IsPartialPayment;

    public Payment(Guid atransactionNumber, PaymentType apaymentType)
    {
        transactionNumber = atransactionNumber;
        datetime = DateTime.Now;
        paymentType = apaymentType;
    }

    public static void GetCashPayments(Transaction transaction, CashDrawer drawer)
    {
        Payment payment = new Payment(transaction.transactionNumber, PaymentType.Cash);

        var exit = (0M, 0, true);
        bool earlyExit;

        var CP1 = Console.GetCursorPosition();

        do
        {
            earlyExit = false;
            (decimal value, int index, bool earlyOut) cashIn = ValidateCash(drawer, transaction);
            if (cashIn == exit)
            {
                payment.IsPartialPayment = true;
                earlyExit = true;
            }
            else
            {
                drawer.AddCashInDrawer(drawer, cashIn.value, cashIn.index);
                payment.cashAmount += cashIn.value;
                transaction.balance -= cashIn.value;
                transaction.totalCashReceived += cashIn.value;

                if (transaction.balance > 0)
                {
                    var CP2 = Console.GetCursorPosition();
                    Console.SetCursorPosition(CP1.Left, CP1.Top);
                    Console.WriteLine($"Amount Remaining: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}                  ");
                    Console.SetCursorPosition(CP2.Left, CP2.Top);
                }
                else if (transaction.balance == 0)
                {
                    Program.Wait(text: "Thank You!");
                }
            }
        } while (transaction.balance > 0 && earlyExit != true);

        payment.success = true;

        transaction.paymentsList.Add(payment);

    }

    static (decimal, int, bool) ValidateCash(CashDrawer drawer, Transaction transaction)
    {
        bool parseSuccess;
        bool valid;
        decimal value;
        int denomIndex = 9999;

        do
        {
            parseSuccess = false;
            valid = false;
            string? stringValue;

            Program.Header("ChangeBot v0.0000015", "By NHS Corp");

            Console.WriteLine("\n");
            var CP = Console.GetCursorPosition();
            Console.WriteLine($"Amount Due: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}\n");
            Console.WriteLine($"Input payment by individual bill or coin value. Cash: $###.##  Coin:  $0.##");
            Console.WriteLine("(Example: 1.00 for dollar or 0.25 for quarter)");

            Console.WriteLine("\nEnter 'x' to return early\n");

            var CP2 = Console.GetCursorPosition();
            Console.Write("$");
            stringValue = Console.ReadLine();
            parseSuccess = decimal.TryParse(stringValue, out value);

            if (!string.IsNullOrEmpty(stringValue) && parseSuccess)
            {
                for (int i = 0; i < drawer.values.Length; i++)
                {
                    if (drawer.values[i] == value)
                    {
                        denomIndex = i;
                        valid = true;
                        Console.Beep();
                        return (value, denomIndex, false);
                    }
                }
                
            }
            else if (stringValue == "x")
            {
                return (0M, 0, true);
            }
            else
            {
                valid = false;
                Program.WriteOver(CP2, message: "Error. Try again.", seconds: 2);
            }
        } while (valid == false);

        //return (value, denomIndex, false);
        return (0M, 0, false);
    }

    internal static decimal TallyTotalPayments(List<Payment> paymentsList)
    {
        decimal total = 0.00M;
        foreach (var item in paymentsList)
        {
            total += item.cashAmount + item.ccAmount;
        }
        //Console.WriteLine(bank);
        return total;
    }

    internal static void CCaccepted(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal approvedAmount)
    {
        payment.success = true;
        payment.declined = false;

        payment.ccVendor = type;
        payment.ccAmount = approvedAmount;
        payment.paymentType = PaymentType.Card;

        transaction.balance -= payment.ccAmount;
        transaction.totalCCreceived += payment.ccAmount;

        transaction.paymentsList.Add(payment);

    }
    internal static void CCdeclined(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal declinedAmount)
    {
        payment.success = false;
        payment.declined = true;

        payment.ccVendor = type;
        payment.declinedAmount = declinedAmount;
        payment.paymentType = PaymentType.Card;
    }
}



