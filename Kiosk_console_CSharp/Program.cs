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
    //{
    //    CashDrawer drawer;
    //    //Denom[] drawerTotals;
    //    int userChoice;
    //    string userChoiceString;
    //    bool parseSuccess;

    //    do
    //    {
    //        Header("ChangeBot v0.01", "ADMIN");


    //        Console.WriteLine("Choose:\n");
    //        Console.WriteLine("1. Start Program and keep standard drawer totals");
    //        Console.WriteLine("     Coins:\n" +
    //                            "       Penny: $5.00        Nickel: $8.00       Dime: $20.00 \n" +
    //                            "       Quarter: $50.00     Half Dol: $-.00     DollarCoin: $-.00" );
    //        Console.WriteLine("     Cash:\n" +
    //                            "       One-Dollar: $500.00         Two-Dollar: $0.00      Fives: $500.00        Ten: $500.00\n" +
    //                            "       Twenties:   $1000.00        Fifties: $0.00         Hundreds: $5000.00");
    //        Console.WriteLine("");
    //        Console.WriteLine("2. Start program and enter drawer totals");
    //        Console.Write("\nChoice = ");
    //        userChoiceString = Console.ReadLine();
    //        parseSuccess = int.TryParse(userChoiceString, out userChoice);

    //    } while (!parseSuccess || !(userChoice == 1 || userChoice == 2 ));

    //    if(userChoice == 1)
    //    {
    //        drawer = new();
    //    } else 
    //    {
    //        drawer = new(GetDenomTotals());
    //    }

    //    Begin(drawer);

    //    Console.ReadLine();
    //}

    //static Denom[] GetDenomTotals()
    //{
    //    Denom Pennies = new() { Name = "Pennies" }, Nickels = new() { Name = "Nickels" }, Dimes = new() { Name = "Dimes" }, Quarters = new() { Name = "Quarters" }, HalfDollars = new() { Name = "Half-Dollars" }, DollarCoins = new() { Name = "Dollar Coins" }, Dollars = new() { Name = "Dollars" }, TwoDollars = new() { Name = "Two-Dollar Bills" }, Fives = new() { Name = "Fives" }, Tens = new() { Name = "Tens" }, Twenties = new() { Name = "Twenties" }, Fifties = new() { Name = "Fifties" }, Hundreds = new()
    //    {
    //        Name = "Hundreds"
    //    };

    //    //denom Pennies = new("Pennies"), Nickels = new("Nickels"), Dimes = new("Dimes"), Quarters = new("Quarters"), HalfDollars = new("Half-Dollars"), DollarCoins = new("Dollar Coins"), Dollars = new("One-Dollar Bills"), TwoDollars = new("Two-Dollar Bills"), Fives = new("Fives"), Tens = new("Tens"), Twenties = new("Twenties"), Fifties = new("Fifties"), Hundreds = new("Hundreds");
    //    Denom[] denoms = { Pennies, Nickels, Dimes, Quarters, HalfDollars, DollarCoins, Dollars, TwoDollars, Fives, Tens, Twenties, Fifties, Hundreds };
        
    //    bool functionSuccess;
    //    do
    //    {
    //        bool parseSuccess1 = false;
    //        bool parseSuccess2 = false;
    //        functionSuccess = false;
    //        decimal value;
    //        int choice = 0;

    //        Header("ChangeBot v0.01", "ADMIN");

    //        Console.WriteLine("Enter values:");

    //        for (int i = 0; i < denoms.Length; i++)
    //        {
    //            Denom denom = denoms[i];
    //            Console.Write($"{denom.Name}: ");
    //            do
    //            {
    //                parseSuccess1 = decimal.TryParse(Console.ReadLine(), out value);
    //                denom.Total = value;
    //                denoms[i] = denom;
    //            } while (!parseSuccess1);
    //        }
    //        Pennies = denoms[0];
    //        Nickels = denoms[1];
    //        Dimes = denoms[2];
    //        Quarters = denoms[3];
    //        HalfDollars = denoms[4];
    //        DollarCoins = denoms[5];
    //        Dollars = denoms[6];
    //        TwoDollars = denoms[7];
    //        Fives = denoms[8];
    //        Tens = denoms[9];
    //        Twenties = denoms[10];
    //        Fifties = denoms[11];
    //        Hundreds = denoms[12];


    //        Console.WriteLine(" \n\nYou Entered: \n");
    //        Console.WriteLine($"     Coins:\n" +
    //                            $"       Penny: ${Pennies.Total}    Nickel: ${Nickels.Total}       Dime: ${Dimes.Total} \n" +
    //                            $"       Quarter: ${Quarters.Total}     Half Dol: ${HalfDollars.Total}     DollarCoin: ${DollarCoins.Total}");
    //        Console.WriteLine("");
    //        Console.WriteLine($"     Cash:\n" +
    //                            $"       One-Dollar: ${Dollars.Total}        Two-Dollar: ${TwoDollars.Total}      Fives: ${Fives.Total}        Ten: ${Tens.Total}\n" +
    //                            $"       Twenties:   ${Twenties.Total}        Fifties: ${Fifties.Total}         Hundreds: ${Hundreds.Total}"
    //                            );
    //        Console.WriteLine("");

    //        Console.WriteLine("");
    //        do
    //        {
    //            Console.WriteLine("If this is correct, Enter 1");
    //            Console.WriteLine("If this is incorrect, Enter 9 to re-enter values");
    //            parseSuccess2 = int.TryParse(Console.ReadLine(), out choice);
    //        } while (!parseSuccess2 || !(choice == 1 || choice == 9));

    //        if (choice == 1)
    //        {
    //            functionSuccess = true;
    //        }

    //    } while (functionSuccess == false);

    //    return denoms;
    //}
//static void Begin(CashDrawer drawer)
static void Main(string[] args)
    {
        CashDrawer drawer= new CashDrawer();

        ConsoleKeyInfo key;
        do
        {
            Header("ChangeBot v0.01", "By NHS Corp");
            Console.WriteLine("Press enter to begin transaction.");

            key = Console.ReadKey();

            TransactionFunction(drawer);

        } while (key.Key != ConsoleKey.Z);

        if (key.Key == ConsoleKey.Z)
        {
            TransactionsList.PrintTransactions();
        }

        Console.ReadLine();
    }


    //static void TransactionLogging(Transaction transaction)
    //{
    //    string transNumber;
    //    string transDate;
    //    string transTime;
    //    string transCash;
    //    string transCC = "";
    //    string transChange;

    //    transNumber = transaction.transactionNumber.ToString();
    //    transDate = transaction.transactionDateTime.ToString("d");
    //    transTime = transaction.transactionDateTime.ToString("T");
    //    transCash = transaction.totalCashReceived.ToString();
    //    transChange = (transaction.cashBackDispensed + transaction.changeDispensed).ToString();

    //    foreach (Payment payment in transaction.paymentsList)
    //    {
    //        if (payment.paymentType == PaymentType.Card && payment.declined == false)
    //        {
    //            transCC += payment.ccVendor.ToString() + payment.ccAmount.ToString();
    //        }
    //        if (transaction.paymentsList.Count > 1)
    //        {
    //            if (payment.Equals(PaymentType.Card))
    //            {
    //                for (int i = 0; i < transaction.paymentsList.Count - 1; i++)
    //                {
    //                    transCC += "-";
    //                }
    //            }
    //        }
    //    }
    //    string arg = transNumber + " " + transDate + " " + transTime + " " + transCash + " " + transChange + " " + transCC;

    //    ProcessStartInfo startInfo = new ProcessStartInfo();
    //    startInfo.FileName = @"C:\Users\natha\source\repos\Kiosk_console_CSharp\Kiosk_Logging_CSharp\Kiosk_Logging_CSharp\bin\Debug\net6.0\Kiosk_Logging_CSharp.exe";
    //    startInfo.Arguments = arg;
    //    Process.Start(startInfo);
    //}

    static void TransactionFunction(CashDrawer drawer)
    {
        decimal totalPayments;

        Header("Welcome to ChangeBot v0.0001", "By NHS Corp");
        decimal total = ManageItems();

        if (total > 0)
        {
            Transaction transaction = new(total, Guid.NewGuid());

            ManagePayments(transaction, drawer);

            totalPayments = Payment.TallyTotalPayments(transaction.paymentsList);

            transaction.Closing(transaction, totalPayments, drawer);

            //if (totalPayments >= transaction.originalTotal + transaction.cashBackReqAmount)
            //{
            //    if (transaction.IsCBrequested)
            //    {
            //        transaction.cashBackOwed -= transaction.cashBackReqAmount;
            //        transaction.changeOwed -= transaction.cashBackReqAmount;
            //    }
            //    else
            //    {
            //        transaction.changeOwed -= totalPayments - transaction.originalTotal;
            //        transaction.balance += Math.Abs(transaction.changeOwed);
            //    }
            //}

            //if (transaction.changeOwed < 0)
            //{
            //    bool changeAndCashBackSuccess = ChangeAndCashBack(transaction, drawer);

            //    if (changeAndCashBackSuccess)
            //    {
            //        WaitForKeyorSeconds(6, "\nTransaction Completed.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("\nDispensing Error: TransactionFunction");
            //    }
            //}
            //else
            //{
            //    WaitForKeyorSeconds(3, "\nTransaction Completed.");
            //}

            TransactionsList.Transactions.Add(transaction);

            Transaction.TransactionLogging(transaction);
        }
    }

    //static bool GiveChange(Transaction transaction, CashDrawer drawer, int[] changeCount) // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER
    //{
    //    for (int i = 0; i < changeCount.Length; i++)
    //    {
    //        if (changeCount[i] > 0)
    //        {
    //            decimal dispensedAmount = changeCount[i] * drawer.values[i];

    //            // drawer.cashInDrawer[i] -= dispensedAmount;
    //            drawer.DeductCashInDrawer(drawer, dispensedAmount, i);

    //            if (transaction.IsCBrequested)
    //            {
    //                transaction.cashBackOwed += dispensedAmount;
    //                transaction.cashBackDispensed += dispensedAmount;
    //                transaction.changeOwed += dispensedAmount;
    //            }
    //            else
    //            {
    //                transaction.changeOwed += dispensedAmount;
    //                transaction.changeDispensed += dispensedAmount;
    //            }

    //            var CP = Console.GetCursorPosition();
    //            for (int j = 0; j < changeCount[i]; j++)
    //            {
    //                WaitForKeyorSeconds(.5, $"{drawer.values[i]} dispensed");
    //            }
    //        }
    //    }
    //    WaitForKeyorSeconds(text: "\nDon't forget to take your change.");
    //    if (transaction.balance == 0 && transaction.changeOwed == 0 && transaction.cashBackOwed == 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
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
                }
                else
                {
                    //Console.WriteLine("Enter 2 to pay with credit/debit");
                    userSelection = 2;
                }
                selectionString = Console.ReadLine();
                parseSuccessfull = int.TryParse(selectionString, out userSelection);

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
                    WaitForKeyorSeconds(4);
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
                            WaitForKeyorSeconds(4);
                        }

                        else if (approvedAmount < transaction.balance)
                        {
                            Console.WriteLine("Transaction Partially Approved:");
                            Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                            payment.IsPartialPayment = true;
                            Payment.CCaccepted(transaction, payment, type, approvedAmount);
                            WaitForKeyorSeconds(4);
                        }
                        else
                        {
                            Console.WriteLine("Error- GetCardPayment");
                            WaitForKeyorSeconds(10);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Card Number Invalid. Try again. Or enter \"exit\" to quit.");
            }

        } while (!valid && creditCardNumber != "exit");

        //return payment;
    }

 

    static decimal GetCashBackAmt(/*CashDrawer drawer,*/ decimal totalCash)
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
                    WaitForKeyorSeconds(1);
                    return amount;
                }
                else if (amount > totalCash)
                {
                    //Console.WriteLine("Requested amount too large.");
                    WaitForKeyorSeconds(3, "Requested amount too large.");
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

    //static void GetCashPayments(Transaction transaction, CashDrawer drawer)
    //{
    //    Payment payment = new Payment(transaction.transactionNumber, PaymentType.Cash);

    //    var exit = (0M, 0, true);
    //    bool earlyExit;

    //    var CP1 = Console.GetCursorPosition();

    //    do
    //    {
    //        earlyExit = false;
    //        (decimal value, int index, bool earlyOut) cashIn = ValidateCash(drawer, transaction);
    //        if (cashIn == exit)
    //        {
    //            payment.IsPartialPayment = true;
    //            earlyExit = true;
    //        }
    //        else
    //        {
    //            //drawer.cashInDrawer[cashIn.index] += cashIn.value;
    //            drawer.AddCashInDrawer(drawer, cashIn.value, cashIn.index);
    //            payment.cashAmount += cashIn.value;
    //            transaction.balance -= cashIn.value;
    //            transaction.totalCashReceived += cashIn.value;

    //            if (transaction.balance > 0)
    //            {
    //                var CP2 = Console.GetCursorPosition();
    //                Console.SetCursorPosition(CP1.Left, CP1.Top);
    //                Console.WriteLine($"Amount Remaining: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}                  ");
    //                Console.SetCursorPosition(CP2.Left, CP2.Top);
    //            }
    //            else if (transaction.balance == 0)
    //            {
    //                WaitForKeyorSeconds(3, text: "Thank You!");
    //            }
    //        }
    //    } while (transaction.balance > 0 && earlyExit != true);

    //    payment.success = true;

    //    transaction.paymentsList.Add(payment);

    //}

    // Given the cursor position of the erroneous line(to write over it), prints error and waits 'seconds'
    // Remeber to 'get' cursor position earlier
    public static void WriteOver((int left, int top) cursorPosition, string message = "Error - Try Again", float seconds = 1)
    {
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        WaitForKeyorSeconds(seconds, message);
        Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
        Console.WriteLine("                                                                                 ");
    }
    //static (decimal, int, bool) ValidateCash(CashDrawer drawer, Transaction transaction)
    //{
    //    bool parseSuccess;
    //    bool valid;
    //    decimal value;
    //    int denomIndex = 9999;

    //    do
    //    {
    //        parseSuccess = false;
    //        valid = false;
    //        string[]? splitString = Array.Empty<string>();
    //        string? stringValue;

    //        Header("ChangeBot v0.0000015", "By NHS Corp");

    //        Console.WriteLine("\n");
    //        var CP = Console.GetCursorPosition();
    //        Console.WriteLine($"Amount Due: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}\n");
    //        Console.WriteLine($"Input payment by individual bill or coin value. Cash: $###.##  Coin:  $0.##");
    //        Console.WriteLine("(Example: 1.00 for dollar or 0.25 for quarter)");

    //        Console.WriteLine("\nEnter 'x' to return early\n");

    //        var CP2 = Console.GetCursorPosition();
    //        Console.Write("$");
    //        stringValue = Console.ReadLine();
    //        parseSuccess = decimal.TryParse(stringValue, out value);//ERROR VALUE removing period

    //        if (!string.IsNullOrEmpty(stringValue) && parseSuccess)
    //        {
    //            for (int i = 0; i < drawer.values.Length; i++)
    //            {
    //                if (drawer.values[i] == value)
    //                {
    //                    denomIndex = i;
    //                    valid = true;
    //                    Console.Beep();
    //                    return (value, denomIndex, false);
    //                }
    //            }
    //            //valid = true;
    //            //    if (parseSuccess && stringValue.Contains('.'))
    //            //    {
    //            //        splitString = stringValue.Split(".");
    //            //        if (splitString.Length != 2 || splitString[1].Length != 2)
    //            //        {
    //            //            valid = false;
    //            //           // WriteOver(CP2, seconds: 1);
    //            //            continue;
    //            //        }
    //            //        for (int i = 0; i < drawer.values.Length; i++)
    //            //        {
    //            //            if (drawer.values[i] == value)
    //            //            {
    //            //                denomIndex = i;
    //            //                valid = true;
    //            //                return (value, denomIndex, false);
    //            //            }
    //            //        }
    //            //        if (valid == false)
    //            //        {
    //            //            //WriteOver(CP2, seconds: 1);
    //            //        }
    //            //    }
    //            //    else if (stringValue == "x")
    //            //    {
    //            //        return (0M, 0, true);
    //            //    }
    //            //    else
    //            //    {
    //            //        valid = false;
    //            //        //WriteOver(CP2, seconds: 1);
    //            //    }
    //        }
    //        else if (stringValue == "x")
    //        {
    //            return (0M, 0, true);
    //        }
    //        else
    //        {
    //            valid = false;
    //            WriteOver(CP2, message: "Error. Try again.", seconds: 2);
    //        }
    //    } while (valid == false);

    //    //return (value, denomIndex, false);
    //    return (0M, 0, false);
    //}

    static decimal ManageItems(decimal previousBalance=0)
    {
        decimal total = previousBalance;
        int itemCount = 1;
        (decimal value, bool escape) itemTuple;

        do
        {
            itemTuple = (0, false);
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
        //string[] splitString;
        decimal value;
        do
        {
            parseSuccess = false;
            valid = false;
            escape = false;
            //splitString = Array.Empty<string>();
            string? stringValue = "";

            Header("ChangeBot v0.0001", "By NHS Corp");
            Console.WriteLine();
            var CPbalance = Console.GetCursorPosition();
            Console.WriteLine($"Balance: {total:C}");
            Console.WriteLine($"Input item {itemCount}. Format: ###.##  \nPress 'Enter' when finished.");

            var CPcursor = Console.GetCursorPosition();
            Console.Write("$");

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
    public static void WaitForKeyorSeconds(double seconds = 5.0, string text = "")
    {
        Console.WriteLine(text);
        Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(seconds));
    }
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

    static void ProcessingAnimation(string message = "Payment processing.", double seconds = 0.1)
    {
        var CP = Console.GetCursorPosition();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {

                Console.SetCursorPosition(CP.Left, CP.Top);
                WaitForKeyorSeconds(seconds, message += ".");
            }
        }

    }
}
class ConsoleColumnFormatter
{
    private int _columnWidth = 20;
    private int _numColumns = 4;

    private int _currentColumn = 0;

    public ConsoleColumnFormatter(int numColumns, int columnWidth)
    {
        _numColumns = numColumns;
        _columnWidth = columnWidth;
    }

    public void Write(string str)
    {
        Console.Write(str.PadRight(_columnWidth - str.Length, ' '));
        _currentColumn++;

        checkForNewLine();
    }

    private void checkForNewLine()
    {
        if (_currentColumn >= _numColumns)
        {
            Console.Write("\n");
            _currentColumn = 0;
        }
    }
}


