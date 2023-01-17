
using System.Globalization;
using System.Diagnostics;

namespace Kiosk_Console_CSharp
{
    public enum PaymentType
    {
        Cash, Card
    }

    public class Program
    {
        static void Main(string[] args)
        {
            CashDrawer drawer = new();

            ConsoleKeyInfo key;
            do
            {
                Header("Welcome to ChangeBot v0.01", "By NHS Corp");
                Console.WriteLine("Press enter to begin transaction.");

                key = Console.ReadKey();
                
                TransactionFunction(drawer);

            } while (key.Key != ConsoleKey.Z);

            if(key.Key == ConsoleKey.Z)
            {
                PrintTransactions();
            }

            Console.ReadLine();
        }
        static void PrintTransactions()
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

        static void TransactionLogging(Transaction transaction)
        {
            string transNumber = "";
            string transDate = "";
            string transTime = "";
            string transCash = "";
            string transCC = "";
            string transChange = "";

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

        static void TransactionFunction(CashDrawer drawer)
        {
            decimal totalPayments;

            Header("Welcom to ChangeBot v0.0001", "By NHS Corp");
            decimal total = ManageItems();

            if (total > 0)
            {
                Transaction transaction = new(total, Guid.NewGuid());

                ManagePayments(transaction, drawer);

                totalPayments = GetPaymentsTotal(transaction.paymentsList);

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

                TransactionsList.Transactions.Add(transaction);

                TransactionLogging(transaction);
            }
        }

        static bool GiveChange(Transaction transaction, CashDrawer drawer, int[] changeCounts) // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER
        {
            for (int i = 0; i < changeCounts.Length; i++)
            {
                if (changeCounts[i] > 0)
                {
                    decimal dispensedAmount = changeCounts[i] * drawer.values[i];

                    drawer.cashInDrawer[i] -= dispensedAmount;

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
                    for (int j = 0; j < changeCounts[i]; j++)
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
        static void ManagePayments(Transaction transaction, CashDrawer drawer)
        {
            int userSelection;
            bool parseSuccessfull;
            do
            {
                do
                {
                    userSelection = 0;
                    parseSuccessfull = false;

                    Header("ChangeBot", "By NHS Corp");

                    Console.WriteLine("Balance Due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));

                    if (transaction.IsCBrequested == false)
                    {
                        Console.WriteLine("\nPress 1 to enter cash");
                    }
                    Console.WriteLine("Press 2 to enter credit/debit\n");

                    parseSuccessfull = int.TryParse(Console.ReadLine(), out userSelection);
                } while (parseSuccessfull == false || !(userSelection == 1 || userSelection == 2));

                if (userSelection == 1)
                {
                    GetCashPayments(transaction, drawer);
                }
                else if (userSelection == 2)
                {
                    GetCardPayment(transaction, drawer);
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
            int cashBackAmount;
            string? input = "";

            decimal totalCash = drawer.GetTotalCashInDrawer();

            if (transaction.IsCBrequested == false)
            {
                Header("ChangeBot v00000001", "By NHS Corp");

                Console.WriteLine("Cash Back?  y/n");
                input = Console.ReadLine() ?? "n";
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
                        CCdeclined(transaction, payment, type, transaction.balance);
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

                                CCaccepted(transaction, payment, type, approvedAmount);
                                WaitForKeyorSeconds(4);
                            }

                            else if (approvedAmount < transaction.balance)
                            {
                                Console.WriteLine("Transaction Partially Approved:");
                                Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                                payment.IsPartialPayment = true;
                                CCaccepted(transaction, payment, type, approvedAmount);
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

        static int GetCashBackAmt(/*CashDrawer drawer,*/ decimal totalCash)
        {
            bool parseSuccess;
            bool modSuccess = false;
            string input = "";
            do
            {
                Header("ChangeBot v00000001", "By NHS Corp");

                Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"exit\" to return.");
                Console.WriteLine("Examples: 20 or 80");
                Console.Write("$");
                var CP = Console.GetCursorPosition();

                input = Console.ReadLine() ?? "";

                if (input == "exit")
                {
                    return 0;
                }
                else
                {
                    parseSuccess = int.TryParse(input, out int amount);

                    if (parseSuccess && amount % 10 == 0 && amount < totalCash)
                    {
                        modSuccess = true;
                        WaitForKeyorSeconds(1);
                        return amount;
                    }
                    else if (amount > totalCash)
                    {
                        Console.WriteLine("Requested amount too large.");
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

            return 0;
        }

        static void GetCashPayments(Transaction transaction, CashDrawer drawer)
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
                    drawer.cashInDrawer[cashIn.index] += cashIn.value;
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
                        WaitForKeyorSeconds(3, text: "Thank You!");
                    }
                }
            } while (transaction.balance > 0 && earlyExit != true);
            
            payment.success = true;

            transaction.paymentsList.Add(payment);
        
        }

        // Given the cursor position of the erroneous line(to write over it), prints error and waits 'seconds'
        // Remeber to 'get' cursor position earlier
        static void WriteOver((int left, int top) cursorPosition, string message = "Error - Try Again", float seconds = 1)
        {
            Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
            WaitForKeyorSeconds(seconds, message);
            Console.SetCursorPosition(cursorPosition.left, cursorPosition.top);
            Console.WriteLine("                                                                                 ");
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
                string[]? splitString = Array.Empty<string>();
                string? stringValue;

                Header("ChangeBot v0.0000015", "By NHS Corp");

                Console.WriteLine("\n");
                var CP = Console.GetCursorPosition();
                Console.WriteLine($"Amount Due: {transaction.balance}\n");
                Console.WriteLine($"Input payment by individual bill or coin value. Cash: $###.##  Coin:  $0.##");
                Console.WriteLine("(Example: 1.00 for dollar or 0.25 for quarter)");

                Console.WriteLine("Enter 'x' to return early\n");

                var CP2 = Console.GetCursorPosition();
                Console.Write("$");
                stringValue = Console.ReadLine();
                parseSuccess = decimal.TryParse(stringValue, out value);//ERROR VALUE removing period

                if (!string.IsNullOrEmpty(stringValue))
                {
                    if (parseSuccess && stringValue.Contains('.'))
                    {
                        splitString = stringValue.Split(".");
                        if (splitString.Length != 2 || splitString[1].Length != 2)
                        {
                            valid = false;
                            WriteOver(CP2, seconds: 1);
                            continue;
                        }
                        for (int i = 0; i < drawer.values.Length; i++)
                        {
                            if (drawer.values[i] == value)
                            {
                                denomIndex = i;
                                valid = true;
                                return (value, denomIndex, false);
                            }
                        }
                        if (valid == false)
                        {
                            WriteOver(CP2, seconds: 1);
                        }
                    }
                    else if (stringValue == "x")
                    {
                        return (0M, 0, true);
                    }
                    else
                    {
                        valid = false;
                        WriteOver(CP2, seconds: 1);
                    }
                }
            } while (valid == false) ;
            
            return (value, denomIndex, false);
        }

        static decimal ManageItems()
        {
            decimal total = 0.0M;
            int itemCount = 1;
            (decimal value, bool escape) itemTuple;

            do
            {
                itemTuple = GetItems(itemCount, total);
                itemCount++;
                total += itemTuple.value;

            } while (itemTuple.escape != true);
            return total;
        }
        static (decimal, bool) GetItems(int itemCount, decimal total)
        {
            bool parseSuccess;
            bool valid;
            bool escape;
            string[] splitString;
            decimal value;
            do
            {
                parseSuccess = false;
                valid = true;
                escape = false;
                splitString = Array.Empty<string>();
                string? stringValue = "";

                Header("ChangeBot v0.00001", "By NHS Corp");
                Console.WriteLine();
                var CPbalance = Console.GetCursorPosition();
                Console.WriteLine($"Balance: {total:C}");
                Console.WriteLine($"Input item {itemCount}. Format: ###.##  \nPress 'Enter' when finished.");

                Console.Write("$");
                var CPcursor = Console.GetCursorPosition();

                stringValue = Console.ReadLine() ?? "0.00";
                parseSuccess = decimal.TryParse(stringValue, out value);


                if (parseSuccess && stringValue.Contains('.'))
                {
                    try
                    {
                        splitString = stringValue.Split(".");
                    }
                    catch
                    {
                        valid = false;
                    }

                    if (splitString.Length != 2 || splitString[1].Length != 2)
                    {
                        valid = false;
                        WriteOver(CPcursor, "Formatting error. Try Again.");
                    }
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
                changeCounted = GetChangeCounts(changeCounts, changeTotal, drawer);

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
        static bool GetChangeCounts(int[] changeCounts, decimal changeAmount, CashDrawer drawer)
        {
            int temp;
            // FOR EACH INDEX IN VALUES[]
            for (int i = 0; i < drawer.values.Length; i++)
            {
                //IF QUOTIENT OF CHANGEAMOUNT/VALUES[I] IS GREATER THAN OR EQUAL TO 1   
                if (changeAmount / drawer.values[i] >= 1)
                {
                    //INTEGER DIVISION GIVING THE NUMBER OF TIMES NUM CAN BE DIVIDED BY VALUE
                    temp = (int)(changeAmount / drawer.values[i]);

                    //IF THERE IS ENOUGH CHANGE OF DENOM TO MAKE CHANGE
                    if (drawer.cashInDrawer[i] >= temp * drawer.values[i])
                    {
                        //REDUCE NUM BY (VALUE*TEMP) (EXAMPLE: 1199->199)
                        changeAmount %= drawer.values[i];
                        //INCREMENT CHANGECOUNTS
                        changeCounts[i] += temp;
                    }

                }
            }
            //IF EXACT CHANGE IS POSSIBLE
            if (changeAmount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }//END GETCHANGECOUNTS

        static decimal GetPaymentsTotal(List<Payment> paymentsList)
        {
            decimal total = 0.00M;
            foreach (var item in paymentsList)
            {
                total += item.cashAmount + item.ccAmount;
            }
            //Console.WriteLine(bank);
            return total;
        }
        static void WaitForKeyorSeconds(double seconds = 5.0, string text = "")
        {
            Console.WriteLine(text);
            Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(seconds));
        }
        static void CCaccepted(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal approvedAmount)
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
        static void CCdeclined(Transaction transaction, Payment payment, CreditCardFunctions.CreditCardType type, decimal declinedAmount)
        {
            payment.success = false;
            payment.declined = true;

            payment.ccVendor = type;
            payment.declinedAmount = declinedAmount;
            payment.paymentType = PaymentType.Card;
        }


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
}

