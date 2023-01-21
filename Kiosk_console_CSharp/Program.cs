using System.Globalization;

namespace Kiosk_Console_CSharp;
public enum PaymentType
{
    Cash, Card
}
public struct Denom
{
    public string Name; public decimal Total;

    public Denom(string name, decimal total)
    {
        Name = name;
        Total = total;
    }
};

public class Program
{
    //static void Main(string[] args)
    //    {
    //        CashDrawer drawer= new CashDrawer();

    //        ConsoleKeyInfo key;
    //        do
    //        {
    //            Header("ChangeBot v0.01", "By NHS Corp");
    //            Console.WriteLine("Press enter to begin transaction.");

    //            key = Console.ReadKey();

    //            TransactionFunction(drawer);

    //        } while (key.Key != ConsoleKey.Z);

    //        if (key.Key == ConsoleKey.Z)
    //        {
    //            TransactionsList.PrintTransactions();
    //        }

    //        Console.ReadLine();
    //    }

    static void Main(string[] args)
    {
        CashDrawer drawer = new CashDrawer();

        ColorfulAnimation();

        while (true)
        {
            string key = null;

            Header("ChangeBot v0.01", "By NHS Corp");
            while (string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Enter 1 to begin transaction.");
                key = Console.ReadLine();
            }

            if (key == "1")
            {
                TransactionFunction(drawer);
            }
        }

        //TransactionsList.PrintTransactions();

    }

    static void TransactionFunction(CashDrawer drawer)
    {
        decimal totalPayments;

        //Header("Welcome to ChangeBot v0.0001", "By NHS Corp");
        decimal total = ManageItems();

        if (total > 0)
        {
            Transaction transaction = new(total, Guid.NewGuid());

            ManagePayments(transaction, drawer);

            totalPayments = Payment.TallyTotalPayments(transaction.paymentsList);

            transaction.Closing(transaction, totalPayments, drawer);

            TransactionsList.Transactions.Add(transaction);

            Transaction.TransactionLogging(transaction);
        }
    }

    static void ManagePayments(Transaction transaction, CashDrawer drawer)
    {
        string? selectionString;
        int userSelection;
        bool parseSuccessfull;
        decimal total;
        do
        {
            do
            {
                total = 0M;
                selectionString = "";
                userSelection = 0;
                parseSuccessfull = false;

                Header("ChangeBot v0.00001", "By NHS Corp");

                Console.WriteLine("Balance Due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));

                if (transaction.IsCBrequested == false)
                {
                    Console.WriteLine("\nEnter 1 to pay with cash");
                    Console.WriteLine("Enter 2 to pay with credit/debit");
                    Console.WriteLine("Enter 3 to add additional items\n");
                    selectionString = Console.ReadLine();
                    parseSuccessfull = int.TryParse(selectionString, out userSelection);
                }
                else
                {
                    Console.WriteLine("Enter 2 to pay with credit/debit");
                    selectionString = Console.ReadLine();
                    parseSuccessfull = true;
                    userSelection = 2;
                }

            } while (parseSuccessfull == false || !(userSelection == 1 || userSelection == 2 || userSelection == 3));

            if (userSelection == 1)
            {
                Payment.GetCashPayments(transaction, drawer);
            }
            else if (userSelection == 2)
            {
                GetCardPayment(transaction, drawer);
            }
            else if (userSelection == 3)
            {
                total = ManageItems(transaction.balance);
                if (total > 0)
                {
                    transaction.TransactionAdd(total);
                }
            }
        } while (transaction.balance > 0);
    }

    private static void GetCardPayment(Transaction transaction, CashDrawer drawer)
    {
        Payment payment = new(transaction.transactionNumber, PaymentType.Card);
        CreditCardFunctions.CreditCardType type;
        string creditCardNumber;
        string creditCardNumber1 = "4716023102375986";  // Visa
        bool valid;
        string[] APIresponse;
        decimal cashBackAmount;
        string? input = "";

        decimal totalCash = drawer.GetTotalCashInDrawer();

        if (transaction.IsCBrequested == false)
        {
            Header("ChangeBot v(-1000)", "By NHS Corp");

            Console.WriteLine("Cash Back?  y/n");
            input = Console.ReadLine();
            if (input == "y")
            {
                cashBackAmount = GetCashBackAmt(totalCash);

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
            Header("ChangeBot v000000001", "By NHS Corp");

            Console.WriteLine("Please enter card number. Use dash '-' or space ' ' between segments. Format: 0000-0000-0000-0000");

            string? creditCardString = Console.ReadLine();

            creditCardNumber = string.IsNullOrWhiteSpace(creditCardString) ? creditCardNumber1 : creditCardString;

            valid = CreditCardFunctions.IsValid(creditCardNumber);

            ProcessingAnimation();

            if (valid)
            {
                type = CreditCardFunctions.FindType(creditCardNumber);
                Console.WriteLine(type);
                APIresponse = CreditCardFunctions.MoneyRequest(creditCardNumber, transaction.balance);

                if (APIresponse[1] == "declined")
                {
                    Console.WriteLine("Card Declined by bank.");
                    Console.WriteLine();
                    Console.WriteLine($"\nDeclined Amount: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}.");
                    Payment.CCdeclined(transaction, payment, type, transaction.balance);
                    Wait();

                    //return payment;
                }
                else
                {
                    bool parseSuccess = decimal.TryParse(APIresponse[1], out decimal approvedAmount);
                    if (parseSuccess == true && approvedAmount > 0)
                    {
                        if (approvedAmount == transaction.balance)
                        {
                            Console.WriteLine("Transaction Approved.");
                            Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                            Payment.CCaccepted(transaction, payment, type, approvedAmount);
                            Wait();

                        }

                        else if (approvedAmount < transaction.balance)
                        {
                            Console.WriteLine("Transaction Partially Approved:");
                            Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                            payment.IsPartialPayment = true;
                            Payment.CCaccepted(transaction, payment, type, approvedAmount);
                            Wait();


                        }
                        else
                        {
                            Console.WriteLine("Error- GetCardPayment");
                            Wait();

                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Card Number Invalid. Try again. Or enter \"exit\" to quit.");
            }

        } while (!valid && creditCardNumber != "exit");

    }

    static decimal GetCashBackAmt( decimal totalCash)
    {
        bool parseSuccess;
        bool modSuccess = false;
        string input = "";
        do
        {
            Header("ChangeBot v00000001", "By NHS Corp");

            Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"exit\" to return.");
            //Console.WriteLine("\nExamples: 10 or 30");
            Console.Write("$");
            var CP = Console.GetCursorPosition();

            input = Console.ReadLine();

            if (input == "exit")
            {
                return 0;
            }
            else
            {
                parseSuccess = decimal.TryParse(input, out decimal amount);

                if (parseSuccess && amount % 10 == 0 && amount < totalCash)
                {
                    modSuccess = true;
                    Wait();
                    return amount;
                }
                else if (amount > totalCash)
                {
                    //Console.WriteLine("Requested amount too large.");
                    Wait(text: "Requested amount too large.");
                }
                else if (modSuccess == false)
                {
                    WriteOver(CP);
                }
                else
                {
                    Console.WriteLine("CashBack Error.");
                }

            }
        } while (modSuccess == false);

        return 0M;
    }

    //// Given the cursor position of the erroneous line(to write over it), prints error and waits 'seconds'
    //// Remeber to 'get' cursor position earlier
    public static void WriteOver((int left, int top) cursorPosition, string message = "Error - Try Again", float seconds = 1)
    {
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Wait(text: message);
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Console.WriteLine("                                                                                 ");
    }

    static decimal ManageItems(decimal previousBalance = 0)
    {
        decimal total = previousBalance;
        int itemCount = 1;
        (decimal value, bool escape) itemTuple;

        do
        {
            //itemTuple = (0, false);
            itemTuple = GetItems(itemCount, total);

            itemCount++;

            total += itemTuple.value;

        } while (itemTuple.escape != true);
        return total - previousBalance;
    }
    static (decimal, bool) GetItems(int itemCount, decimal total)
    {
        bool parseSuccess;
        bool valid;
        bool escape;
        decimal value;
        do
        {
            parseSuccess = false;
            valid = false;
            escape = false;
            string stringValue = "";

            Header("ChangeBot v0.0001", "By NHS Corp");
            Console.WriteLine("");
            //var CPbalance = Console.GetCursorPosition();
            Console.WriteLine($"Balance: {total:C}");
            Console.WriteLine($"Input item {itemCount}. Format: ###.##  \nPress 'Enter' when finished.");

            var CPcursor = Console.GetCursorPosition();
            Console.Write("$: ");

            stringValue = Console.ReadLine();
            parseSuccess = decimal.TryParse(stringValue, out value);

            if (parseSuccess && value > 0)
            {
                valid = true;
                Console.Beep();
            }

            else if (string.IsNullOrWhiteSpace(stringValue))
            {
                escape = true;
            }
            else
            {
                valid = false;
                WriteOver(CPcursor, "Formatting error. Try Again.");
            }

        } while (valid == false && escape == false);

        return (value, escape);
    }
    //static bool ChangeAndCashBack(Transaction transaction, CashDrawer drawer)
    //{
    //    bool insuffChange = false;
    //    bool changeGiven = false;
    //    bool changeCounted = false;

    //    if (transaction.changeOwed < 0)
    //    {
    //        decimal changeTotal = transaction.changeOwed;

    //        changeTotal = Math.Abs(changeTotal);

    //        int[] changeCounts = new int[13];
    //        changeCounted = drawer.GetChangeCounts(changeCounts, changeTotal, drawer);

    //        if (changeCounted)
    //        {
    //            changeGiven = GiveChange(transaction, drawer, changeCounts);
    //        }
    //        else
    //        {
    //            Console.WriteLine("Insufficient Change");
    //            insuffChange = true;
    //        }
    //        if (insuffChange)
    //        {
    //            Console.WriteLine("Alternatives...");
    //        }
    //    }
    //    if (changeCounted && changeGiven)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}
    //static bool GetChangeCounts(int[] changeCounts, decimal changeAmount, CashDrawer drawer)
    //{
    //    int temp;
    //    // FOR EACH INDEX IN VALUES[]
    //    for (int i = 0; i < drawer.values.Length; i++)
    //    {
    //        //IF QUOTIENT OF CHANGEAMOUNT/VALUES[I] IS GREATER THAN OR EQUAL TO 1   
    //        if (changeAmount / drawer.values[i] >= 1)
    //        {
    //            //INTEGER DIVISION GIVING THE NUMBER OF TIMES NUM CAN BE DIVIDED BY VALUE
    //            temp = (int)(changeAmount / drawer.values[i]);

    //            //IF THERE IS ENOUGH CHANGE OF DENOM TO MAKE CHANGE
    //            if (drawer.cashInDrawer[i] >= temp * drawer.values[i])
    //            {
    //                //REDUCE NUM BY (VALUE*TEMP) (EXAMPLE: 1199->199)
    //                changeAmount %= drawer.values[i];
    //                //INCREMENT CHANGECOUNTS
    //                changeCounts[i] += temp;
    //            }

    //        }
    //    }
    //    //IF EXACT CHANGE IS POSSIBLE
    //    if (changeAmount == 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}//END GETCHANGECOUNTS

    //static decimal TallyTotalPayments(List<Payment> paymentsList)
    //{
    //    decimal total = 0.00M;
    //    foreach (var item in paymentsList)
    //    {
    //        total += item.cashAmount + item.ccAmount;
    //    }
    //    //Console.WriteLine(bank);
    //    return total;
    //}

    //static void CCaccepted(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal approvedAmount)
    //{
    //    payment.success = true;
    //    payment.declined = false;

    //    payment.ccVendor = type;
    //    payment.ccAmount = approvedAmount;
    //    payment.paymentType = PaymentType.Card;

    //    transaction.balance -= payment.ccAmount;
    //    transaction.totalCCreceived += payment.ccAmount;

    //    transaction.paymentsList.Add(payment);

    //}
    //static void CCdeclined(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal declinedAmount)
    //{
    //    payment.success = false;
    //    payment.declined = true;

    //    payment.ccVendor = type;
    //    payment.declinedAmount = declinedAmount;
    //    payment.paymentType = PaymentType.Card;
    //}


    public static void Header(string title, string subtitle = "")
    {
        Console.Clear();
        Console.WriteLine();
        int windowWidth = 90 - 2;
        string titleContent = string.Format("    ║{0," + ((windowWidth / 2) + (title.Length / 2)) + "}{1," + (windowWidth - (windowWidth / 2) - (title.Length / 2) + 1) + "}", title, "║");
        string subtitleContent = string.Format("    ║{0," + ((windowWidth / 2) + (subtitle.Length / 2)) + "}{1," + (windowWidth - (windowWidth / 2) - (subtitle.Length / 2) + 1) + "}", subtitle, "║");

        Console.WriteLine("    ╔════════════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine(titleContent);
        if (!string.IsNullOrEmpty(subtitle))
        {
            Console.WriteLine(subtitleContent);
        }
        Console.WriteLine("    ╚════════════════════════════════════════════════════════════════════════════════════════╝");
        Console.WriteLine("\n");
    }
    public static void Wait(int milliseconds = 1000, string text = "")
    {
        Console.WriteLine(text);
        Thread.Sleep(milliseconds);
        //Task.Factory.StartNew(() => Console.ReadLine()).Wait(TimeSpan.FromSeconds(seconds));
    }
    static void ProcessingAnimation(string message = "Payment processing", int milliseconds = 100)
    {
        var CP = Console.GetCursorPosition();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {

                Console.WriteLine(message += ".");
                Console.SetCursorPosition(CP.Left, CP.Top);
                //WaitForKeyorSeconds(seconds, message += ".");
                Thread.Sleep(milliseconds);
            }
        }
    }

    static void ColorfulAnimation()
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Console.Clear();
                Console.WriteLine("LOADING.....");

                // steam
                Console.Write("       . . . . o o o o o o", ConsoleColor.Gray);
                for (int s = 0; s < j / 2; s++)
                {
                    Console.Write(" o", ConsoleColor.Gray);
                }
                Console.WriteLine();

                var margin = "".PadLeft(j);
                Console.WriteLine(margin + "                _____      o", ConsoleColor.Gray);
                Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.", ConsoleColor.DarkBlue);
                Console.WriteLine(margin + "      [________]_|__|________)< ", ConsoleColor.DarkBlue);
                Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_", ConsoleColor.Blue);
                Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", ConsoleColor.White);

                Thread.Sleep(200);
            }
        }
    }
    //}
    //}
    //class ConsoleColumnFormatter
    //{
    //    private int _columnWidth = 20;
    //    private int _numColumns = 4;

    //    private int _currentColumn = 0;

    //    public ConsoleColumnFormatter(int numColumns, int columnWidth)
    //    {
    //        _numColumns = numColumns;
    //        _columnWidth = columnWidth;
    //    }

    //    public void Write(string str)
    //    {
    //        Console.Write(str.PadRight(_columnWidth - str.Length, ' '));
    //        _currentColumn++;

    //        checkForNewLine();
    //    }

    //    private void checkForNewLine()
    //    {
    //        if (_currentColumn >= _numColumns)
    //        {
    //            Console.Write("\n");
    //            _currentColumn = 0;
    //        }
    //    }

}


