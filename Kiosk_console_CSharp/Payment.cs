using System.Globalization;

namespace Kiosk_Console_CSharp;
public enum PaymentType
{
    Cash, Card
}
public class Payment
{
    internal Guid transactionNumber;
    internal DateTime datetime;
    internal PaymentType paymentType;
    internal bool success;
    private decimal cashAmount;

    internal CreditCardFunctions.CreditCardType? ccVendor;
    internal decimal ccAmount;
    internal bool declined;
    internal decimal declinedAmount;
    internal bool IsPartialPayment;

    public Payment(Guid atransactionNumber, PaymentType apaymentType)
    {
        transactionNumber = atransactionNumber;
        datetime = DateTime.Now;
        paymentType = apaymentType;
    }

    internal static void GetCardPayment(Transaction transaction, CashDrawer drawer)
    {
        Payment payment = new(transaction.transactionNumber, PaymentType.Card);
        CreditCardFunctions.CreditCardType type;
        string creditCardNumber;
        string creditCardNumber1 = "4716023102375986";  // Visa
        bool IsValid;
        string[] APIresponse;
        decimal cashBackAmount;
        string? input = "";

        decimal totalCash = CashDrawer.GetTotalCashInDrawer();

        if (transaction.IsCBrequested == false)
        {
            Program.Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("Cash Back?  y/n");
            input = Console.ReadLine();
            if (input == "y")
            {

                cashBackAmount = Program.GetCashBackAmt(totalCash);

                if (cashBackAmount > 0)
                {
                    transaction.IsCBrequested = true;
                    transaction.cashBackReqAmount = cashBackAmount;
                    transaction.balance += cashBackAmount;
                }
            }
        }

        do
        {
            Program.Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("Please enter card number- with or without a dash '-' or space ' ' between segments. Format: 0000-0000-0000-0000\n");

            string? creditCardString = Console.ReadLine();
            creditCardString = creditCardString.Replace("-", "");
            creditCardString = creditCardString.Replace(" ", "");

            creditCardNumber = string.IsNullOrWhiteSpace(creditCardString) ? creditCardNumber1 : creditCardString;

            IsValid = CreditCardFunctions.IsValid(creditCardNumber);

            Program.Header("ChangeBot", "By NHS Corp");

            if (IsValid)
            {

                var CP = Console.GetCursorPosition();

                Program.ProcessingAnimation();

                Console.SetCursorPosition(CP.Left, CP.Top);
                string ccDisplay = creditCardNumber.Substring(creditCardNumber.Length - 4);
                ccDisplay = ccDisplay.PadLeft(16, '*');

                type = CreditCardFunctions.FindType(creditCardNumber);
                //Console.WriteLine(type);
                APIresponse = CreditCardFunctions.MoneyRequest(creditCardNumber, transaction.balance);

                if (APIresponse[1] == "declined")
                {
                    Console.WriteLine("                    Declined                      ");
                    Console.WriteLine("--------------------------------------------------");

                    Console.WriteLine($"\n{type} {ccDisplay}: Declined Amount: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}.                              ");
                    Payment.CCdeclined(transaction, payment, type, transaction.balance);
                    Program.Wait();
                }
                else
                {
                    bool parseSuccess = decimal.TryParse(APIresponse[1], out decimal approvedAmount);
                    if (parseSuccess == true && approvedAmount > 0)
                    {
                        if (approvedAmount == transaction.balance)
                        {
                            Console.WriteLine("             Transaction Approved.               ");
                            Console.WriteLine("--------------------------------------------------");
                            Console.WriteLine($"{type} {ccDisplay}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}                    ");

                            Payment.CCaccepted(transaction, payment, type, approvedAmount);
                            Program.Wait();
                        }

                        else if (approvedAmount < transaction.balance)
                        {
                            Console.WriteLine("         Transaction Partially Approved:             ");
                            Console.WriteLine("--------------------------------------------------");

                            Console.WriteLine($"{type} {ccDisplay}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}                     ");
                            Console.WriteLine($"\nAmount remaining: {(transaction.balance - approvedAmount).ToString("C", CultureInfo.CurrentCulture)}         ");
                            payment.IsPartialPayment = true;
                            Payment.CCaccepted(transaction, payment, type, approvedAmount);
                            Program.Wait();
                        }
                        else
                        {
                            //Console.WriteLine("Error- GetCardPayment");
                            Program.Wait(text: "Error- GetCardPayment");
                        }
                    }
                }
            }
            else if (creditCardNumber != "exit")
            {
                //Console.WriteLine("Card Number Invalid. Try again. Or enter \"exit\" to quit.");
                Program.Wait(3000, text: "Card Number Invalid. Re-enter card number or enter \"exit\" to return to payments.");
            }

        } while (!IsValid && creditCardNumber != "exit");

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
                CashDrawer.AddCashToDrawer(/*drawer,*/ cashIn.value, cashIn.index);
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

            Program.Header("ChangeBot 3000 v1.1", "By NHS Corp");

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
                for (int i = 0; i < CashDrawer.values.Length; i++)
                {
                    if (CashDrawer.values[i] == value)
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
                Program.WriteOver(CP2, message: "Error. Try again.");
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



