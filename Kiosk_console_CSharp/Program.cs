using System.Globalization;
using Windows.Storage.Provider;

namespace Kiosk_Console_CSharp;

public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "ChangeBot 3000";
        CashDrawer drawer = new CashDrawer();

        ColorfulAnimation();

        while (true)
        {
            string key = null;

            Header("ChangeBot 3000 v1.1", "By NHS Corp");
            var CP = Console.GetCursorPosition();
            
            while (string.IsNullOrWhiteSpace(key))
            {
                Console.SetCursorPosition(CP.Left, CP.Top);
                Console.WriteLine("Enter 1 to begin transaction.");
                key = Console.ReadLine();
            }

            if (key == "1")
            {
                TransactionFunction(drawer);
            }
        }
    }

    static void TransactionFunction(CashDrawer drawer)
    {
        decimal totalPayments;

        decimal total = ManageItems();

        if (total > 0)
        {
            Transaction transaction = new(total, Guid.NewGuid());

            ManagePayments(transaction, drawer);

            totalPayments = Payment.TallyTotalPayments(transaction.paymentsList);

            transaction.CloseTransaction(transaction, totalPayments, drawer);

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

                Header("ChangeBot 3000 v1.1", "By NHS Corp");

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
                Payment.GetCardPayment(transaction, drawer);
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

    internal static decimal GetCashBackAmt( decimal totalCash)
    {
        bool parseSuccess;
        bool modSuccess = false;
        string input = "";
        do
        {
            Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"cancel\" to continue payment.");
            //Console.WriteLine("\nExamples: 10 or 30");
            Console.Write("$: ");
            var CP = Console.GetCursorPosition();

            input = Console.ReadLine();

            if (input == "cancel")
            {
                return 0;
            }
            else
            {
                parseSuccess = decimal.TryParse(input, out decimal amount);

                if (parseSuccess && amount % 10 == 0 && amount < totalCash)
                {
                    modSuccess = true;
                    Wait(3000);
                    return amount;
                }
                else if (amount > totalCash)
                {
                    //Console.WriteLine("Requested amount too large.");
                    Wait(5000, "Requested amount too large.");
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

            Header("ChangeBot 3000 v1.1", "By NHS Corp");
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
                WriteOver(CPcursor);
            }

        } while (valid == false && escape == false);

        return (value, escape);
    }

    public static void Header(string title, string subtitle = "")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
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
        Console.ResetColor();
    }
    public static void Wait(int milliseconds = 4000, string text = "")
    {
        Console.WriteLine(text);
        Thread.Sleep(milliseconds);
        //Task.Factory.StartNew(() => Console.ReadLine()).Wait(TimeSpan.FromSeconds(seconds));
    }
    public static void ProcessingAnimation(string message = "Payment processing", int milliseconds = 100)
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

    //// Given the cursor position of the erroneous line(to write over it), prints error and waits 'seconds'
    //// Remeber to 'get' cursor position earlier
    public static void WriteOver((int left, int top) cursorPosition, string message = "Error - Try Again", int milliseconds = 1000)
    {
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Wait(milliseconds, text: message);
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Console.WriteLine("                                                                                 ");
    }
    static void ColorfulAnimation()
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 20; j++)
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
                Console.WriteLine(margin + "                _____      o", Console.ForegroundColor = ConsoleColor.DarkGray);
                Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.", Console.ForegroundColor = ConsoleColor.DarkBlue);
                Console.WriteLine(margin + "      [________]_|__|________)< ", Console.ForegroundColor = ConsoleColor.DarkBlue);
                Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_", Console.ForegroundColor = ConsoleColor.Blue);
                Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", Console.ForegroundColor = ConsoleColor.White);
                Console.WriteLine("     (source: www.michalbialecki.com)", Console.ForegroundColor = ConsoleColor.DarkGray);
                Thread.Sleep(200);
            }
        }
    }                                                                             
}


