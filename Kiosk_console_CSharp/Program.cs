using System.Globalization;

namespace Kiosk_Console_CSharp;

public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "ChangeBot 3000";

        //ColorfulAnimation();
        Robot();

        ConsoleKeyInfo key;

        //Console.ResetColor();
        Console.WriteLine("\n\n\tPress enter to load application with standard Drawer totals.");
        Console.WriteLine("\tPress control+F to enter totals");
        Console.ResetColor();

        key = Console.ReadKey();


        
        if (key.Key == ConsoleKey.F && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            CashDrawer drawer = CashDrawer.Admin();
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            CashDrawer drawer = new CashDrawer();
        }
        
        Startup();
    }
    static void Startup()
    {
        while (true)
        {
            ConsoleKeyInfo key;

            Header("ChangeBot 3000 v1.1", "By NHS Corp");
            do
            {
                var CP = Console.GetCursorPosition();
                Console.WriteLine("Press Enter to begin transaction.");
                key = Console.ReadKey(true);
                Console.SetCursorPosition(CP.Left, CP.Top);
            } 
            while (key.Key != ConsoleKey.Enter);
  
            TransactionManager();
        }
    }


    internal static void TransactionManager() //
    {
        decimal totalPayments;
        decimal total = ManageItems(); // TOTAL OF ALL ITEMS ENTERED

        if (total > 0) // ONLY BEGINS ONCE ITEMS HAVE BEEN ENTERED
        {
            Transaction transaction = new(total, Guid.NewGuid()); // CONSTRUCTS NEW TRANSACTION OBJECT

            ManagePayments(transaction); // GETS PAYMENTS

            totalPayments = TallyTotalPayments(transaction.paymentsList); // GETS TOTAL OF ALL PAYMENTS FOR VERIFICATION

            Transaction.CloseTransaction(transaction, totalPayments); // 

            TransactionsList.Transactions.Add(transaction);

            Transaction.TransactionLogging(transaction);
        }
    }

    
    internal static decimal TallyTotalPayments(List<Payment> paymentsList)  // ADDS UP TOTAL AMOUNT OF ALL PAYMENTS
    {
        decimal total = 0.00M;
        foreach (var item in paymentsList)
        {
            total += item.cashAmount + item.ccAmount;
        }
        //Console.WriteLine(bank);
        return total;
    }   

    internal static void ManagePayments(Transaction transaction) // MANAGES CASH AND/OR CARD PAYMENTS, RETURNS WHEN BALANCE IS LESS THAN OR EQUAL TO ZERO
    {

        string? selectionString;
        int userSelection;
        bool parseSuccessfull;
        decimal total;
        
        do // WHILE BALANCE IS GREATER THAN ZERO, CONTINUE REQUESTING PAYMENTS
        {
            do // INPUT VALIDATION LOOP
            {
                Header("ChangeBot 3000 v1.1", "By NHS Corp");

                Console.Write("Balance Due: ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" {transaction.balance.ToString("C", CultureInfo.CurrentCulture)} ");
                Console.ResetColor();

                if (transaction.IsCBrequested == false && transaction.insufficientChange == false) // IF CB NOT REQUESTED, ALL OPTIONS OF PAYMENT AVAILABLE
                {
                    Console.WriteLine("\nEnter 1 to pay with cash");
                    Console.WriteLine("Enter 2 to pay with credit/debit");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Enter 3 to add additional items\n");
                    Console.ResetColor();
                    selectionString = Console.ReadLine();
                    parseSuccessfull = int.TryParse(selectionString, out userSelection);
                }
                else //IF CASHBACK IS REQUESTED, CASH PAYMENTS ARE DISALLOWED PER REQUIREMENTS P.2/R.3
                {
                    Console.WriteLine("Enter 2 to pay with credit/debit");
                    selectionString = Console.ReadLine();
                    parseSuccessfull = true;
                    userSelection = 2;
                }

            } while (parseSuccessfull == false || !(userSelection == 1 || userSelection == 2 || userSelection == 3));

            switch (userSelection)
            {
                case 1: ManageCashPayments(transaction); break;
                case 2: GetCardPayment(transaction); break;
                case 3:
                    total = ManageItems(transaction.balance);
                    if (total > 0)
                    {
                        transaction.TransactionAddItem(total);
                    }
                    break;
            }

        } while (transaction.balance > 0);
    }

    #region // CASH MGMT
    // Handles cash payments
    internal static void ManageCashPayments(Transaction transaction)
    {
        Payment payment = new Payment(transaction.transactionNumber, PaymentType.Cash);

        var exit = (0M, 0, true);
        bool earlyExit;

        var CP1 = Console.GetCursorPosition();

        do
        {
            earlyExit = false;
            (decimal value, int index, bool earlyOut) cashIn = GetCash(transaction);
            if (cashIn == exit)
            {
                payment.IsPartialPayment = true;
                earlyExit = true;
            }
            else
            {
                CashDrawer.AddCashToDrawer(cashIn.value, cashIn.index);
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

    internal static (decimal, int, bool) GetCash(Transaction transaction)
    {
        bool parseSuccess;
        bool valid;
        decimal value;
        int denomIndex;

        do
        {
            parseSuccess = false;
            valid = false;
            string? stringValue;
            denomIndex = 9999;

            Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("\n");
            Console.Write($"Amount Due: ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($" { transaction.balance.ToString("C", CultureInfo.CurrentCulture)} \n");
            Console.ResetColor();

            Console.ResetColor();
            Console.WriteLine($"Input payment by individual bill or coin value. Cash: $###.##  Coin:  $0.##");
            Console.WriteLine("(Example: 1.00 for dollar or 0.25 for quarter)");

            Console.WriteLine("\nEnter 'x' to return early\n", Console.ForegroundColor = ConsoleColor.DarkGray);
            Console.ResetColor();
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
                WriteOver(CP2, message: "Error. Try again.");
            }
        } while (valid == false);

        return (0M, 0, false);
    }
    #endregion

    #region // CC/CB MGMT
    internal static decimal GetCashBackAmt( decimal totalCash)
    {
        bool parseSuccess;
        bool modSuccess = false;
        string input = "";
        do
        {
            Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"cancel\" to continue payment.");
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
                    Wait(2000);
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

    internal static void GetCardPayment(Transaction transaction)
    {
        Payment payment = new(transaction.transactionNumber, PaymentType.Card);
        CreditCardFunctions.CreditCardType type;
        string creditCardNumber;
        string testCCnumber = "4716023102375986";  // Visa
        bool IsValid;
        string[] APIresponse;
        decimal cashBackAmount;
        string? input = "";

        decimal totalCash = CashDrawer.GetTotalCashInDrawer();

        if (transaction.IsCBrequested == false && transaction.insufficientChange == false)
        {
            Header("ChangeBot 3000 v1.1", "By NHS Corp");

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
            Header("ChangeBot 3000 v1.1", "By NHS Corp");

            Console.WriteLine("Please enter card number- with or without a dash '-' or space ' ' between segments. Format: 0000-0000-0000-0000\n");

            string? creditCardString = Console.ReadLine();
            creditCardString = creditCardString.Replace("-", "");
            creditCardString = creditCardString.Replace(" ", "");

            creditCardNumber = string.IsNullOrWhiteSpace(creditCardString) ? testCCnumber : creditCardString;

            IsValid = CreditCardFunctions.IsValid(creditCardNumber);
            IsValid = CreditCardFunctions.IsValid(creditCardNumber);

            Header("ChangeBot", "By NHS Corp");

            if (IsValid)
            {

                var CP = Console.GetCursorPosition();
                ProcessingAnimation();
                Console.SetCursorPosition(CP.Left, CP.Top);

                string ccDisplay = creditCardNumber[^4..];

                ccDisplay = ccDisplay.PadLeft(16, '*');

                type = CreditCardFunctions.FindType(creditCardNumber);

                APIresponse = CreditCardFunctions.MoneyRequest(creditCardNumber, transaction.balance);

                if (APIresponse[1] == "declined")
                {
                    Console.WriteLine("                    Declined                      ");
                    Console.WriteLine("--------------------------------------------------");

                    Console.WriteLine($"\n{type} {ccDisplay}: Declined Amount: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}.                              ");
                    CCdeclined(transaction, payment, type, transaction.balance);
                    Wait();
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

                            CCaccepted(transaction, payment, type, approvedAmount);
                            Wait();
                        }

                        else if (approvedAmount < transaction.balance)
                        {
                            Console.WriteLine("         Transaction Partially Approved:             ");
                            Console.WriteLine("--------------------------------------------------");

                            Console.WriteLine($"{type} {ccDisplay}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}                     ");
                            Console.WriteLine($"\nAmount remaining: {(transaction.balance - approvedAmount).ToString("C", CultureInfo.CurrentCulture)}         ");
                            payment.IsPartialPayment = true;
                            CCaccepted(transaction, payment, type, approvedAmount);
                            Wait();
                        }
                        else
                        {
                            Wait(text: "Error- GetCardPayment");
                        }
                    }
                }
            }
            else if (creditCardNumber != "exit")
            {
                Wait(3000, text: "Card Number Invalid. Re-enter card number or enter \"exit\" to return to payments.");
            }

        } while (!IsValid && creditCardNumber != "exit");

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
    #endregion

    #region// ITEM MGMT
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
            Console.Write($"Balance: ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($" {total:C} ");
            Console.ResetColor();
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
    #endregion

    #region// UI/PRESENTATION MGMT
    public static void Header(string title, string subtitle = "")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
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

    //// Given the cursor position of the line to write over, prints error and waits 'seconds'
    //// Remeber to 'get' cursor position earlier
    public static void WriteOver((int left, int top) cursorPosition, string message = "Error - Try Again", int milliseconds = 1000)
    {
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Wait(milliseconds, text: message);
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Console.WriteLine("                                                                                 ");
    }

    static void Robot()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("                                  _____\r\n                                 |     |\r\n                                 | | | |\r\n                                 |_____|\r\n                           ____ ___|_|___ ____\r\n                          ()___)         ()___)\r\n                          // /| ChangeBot |\\ \\\\\r\n                         // / |    3000   | \\ \\\\\r\n                        (___) |___________| (___)\r\n                        (___)   (_______)   (___)\r\n                        (___)     (___)     (___)\r\n                        (___)      |_|      (___)\r\n                        (___)  ___/___\\___   | |\r\n                         | |  |           |  | |\r\n                         | |  |___________| /___\\\r\n                        /___\\  |||     ||| //   \\\\\r\n                       //   \\\\ |||     ||| \\\\   //\r\n                       \\\\   // |||     |||  \\\\ //\r\n                        \\\\ // ()__)   (__()\r\n                              ///       \\\\\\\r\n                             ///         \\\\\\\r\n                           _///___     ___\\\\\\_\r\n                          |_______|   |_______|");
        Console.ResetColor();
    }
    static void ColorfulAnimation()
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Console.Clear();
                Console.WriteLine("LOADING.....", ConsoleColor.White);

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
                Thread.Sleep(100);


            }
        }
    }
    
    #endregion
}


