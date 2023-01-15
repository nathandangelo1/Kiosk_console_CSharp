
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Kiosk_Console_CSharp
{
    public class TransactionsList
    {
        public static List<Transaction> Transactions = new List<Transaction>();
    }
    public class Transaction
    {
        public Guid transactionNumber;
        public DateTime transactionDate;
        public DateTime transactionTime;
        public decimal totalCashReceived;
        //public string ccVendor;
        public decimal totalCCreceived;
        public decimal changeOwed;
        public decimal changeDispensed;

        public bool IsCBrequested;
        public decimal cashBackReqAmount;
        public decimal cashBackDispensed;
        public decimal cashBackOwed;
        public decimal originalTotal;
        //public decimal newBalance;
        public decimal balance;

        public List<Payment> paymentsList = new();

        public Transaction(decimal atotal, System.Guid atransactionNumber, DateTime atransactionDateTime)
        {
            originalTotal = atotal;
            transactionNumber = atransactionNumber;
            transactionDate = atransactionDateTime.Date;
            transactionTime = atransactionDateTime.ToLocalTime();
            balance = atotal;

        }
    }
    public class Payment
    {
        public System.Guid transactionNumber;
        public DateTime datetime;

        public decimal cashAmount;

        public PaymentType paymentType;
        public bool success;

        public CardType? ccVendor;
        public decimal ccAmount;
        public bool declined;
        public decimal declinedAmount;
        public bool IsPartialPayment;

        public Payment(Guid atransactionNumber, DateTime aDateTime, PaymentType apaymentType)
        {
            transactionNumber = atransactionNumber;
            datetime = aDateTime;
            paymentType = apaymentType;
        }

    }
    public enum PaymentType
    {
        Cash, Card
    }
    public enum CardType
    {
        MasterCard, Visa, AmericanExpress, Discover, JCB
    };

    public class Program
    {
        static void Main(string[] args)
        {
            CashDrawer drawer = new();

            bool quit = false;
            do
            {
                Action line = () => Console.WriteLine();

                //int userSelection = 0;
                quit = false;

                Console.Clear();
                Console.WriteLine("Press any key to begin transaction.");
                Console.ReadKey(true);
                //while (!int.TryParse(Console.Readkey(), out userSelection) || userSelection != 1) { }
                
                TransactionFunction(drawer);

                //if (userSelection == 1)
                //{
                //}
                //else if (userSelection == 9)
                //{
                //    quit = true;
                //}

            } while (quit == false);
        }
        static void TransactionFunction(CashDrawer drawer)
        {
            decimal totalPayments;

            decimal total = InputItems();

            if (total > 0)
            {

                Transaction transaction = new(total, Guid.NewGuid(), new DateTime());

                Payments(transaction, drawer);

      
                totalPayments = GetPaymentsTotal(transaction.paymentsList);


                if (totalPayments >= transaction.originalTotal + transaction.cashBackReqAmount)
                {
                    if (transaction.IsCBrequested) {
                        transaction.cashBackOwed -= transaction.cashBackReqAmount;
                        transaction.changeOwed -= transaction.cashBackReqAmount;
                    }
                    else 
                    {
                        transaction.changeOwed -= totalPayments - transaction.originalTotal;
                        transaction.balance += Math.Abs(transaction.changeOwed);
                    }
                }

                bool changeAndCashBackSuccess = ChangeAndCashBack(transaction, drawer);

                if (changeAndCashBackSuccess)
                {
                    WaitForKeyorSeconds(6, "Transaction Completed. Thank you.");
                }
                else
                {
                    Console.WriteLine("Dispensing Error: TransactionFunction");
                }
                
                TransactionsList.Transactions.Add(transaction);
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

                    for (int j = 0; j < changeCounts[i]; j++)
                    {
                        Console.WriteLine($"{drawer.values[i]} dispensed");
                    }
                }
            }
            WaitForKeyorSeconds(text: "Don't forget to take your change.");
            if (transaction.balance == 0 && transaction.changeOwed == 0 && transaction.cashBackOwed == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void Payments(Transaction transaction, CashDrawer drawer)
        {
            //bool success;
            //decimal totalPayments;
            //decimal originalTotal = transaction.total;
            Payment payment;
            //decimal change = 0M;
            //string creditCardNumber = "378282246310005";  // American Express

            //Console.WriteLine($"Balance Due: {transaction.total.ToString("C", CultureInfo.CurrentCulture)}");
            do
            {
                int userSelection = 0;
                bool successful = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Balance Due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));
                    if (transaction.IsCBrequested == false)
                    {
                        Console.WriteLine("Press 1 to enter cash");
                    }
                    Console.WriteLine("Press 2 to enter credit/debit");
                    //while (!int.TryParse(Console.ReadLine(), out userSelection) || userSelection != 1) { }

                    successful = int.TryParse(Console.ReadLine(), out userSelection);
                } while (successful == false || userSelection < 1 || userSelection >= 3);

                if (userSelection == 1)
                {
                    payment = GetCashPayments(transaction, drawer);
                    transaction.paymentsList.Add(payment);
                }
                else if (userSelection == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Balance due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));
                    //CCTest();
                    payment = GetCardPayment(transaction, drawer);

                }

            } while ( transaction.balance > 0 );
        }

        private static Payment GetCardPayment(Transaction transaction, CashDrawer drawer)
        {
            Payment payment = new(transaction.transactionNumber, DateTime.Now, PaymentType.Card);
            CardType type;
            string creditCardNumber;
            string creditCardNumber1 = "4716023102375986";  // Visa
            bool valid = false;
            string[] APIresponse;
            int cashBackAmount = 0;
            decimal totalAmountDue;
            decimal totalCash = drawer.GetTotalCashInDrawer();

            if (transaction.IsCBrequested == false)
            {
                Console.Clear();
                Console.WriteLine("Cash Back?  y/n");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    cashBackAmount = GetCashBackAmt(drawer, totalCash);
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
                Console.Clear();
                Console.WriteLine("Please enter card number. Use dash '-' or space ' ' between segments. Format: 0000-0000-0000-0000");
                string? creditCardString = Console.ReadLine();

                creditCardNumber = string.IsNullOrWhiteSpace(creditCardString) ? creditCardNumber1 : creditCardString;

                creditCardString = CustomerMobileNumberMasking(creditCardNumber);
                Console.WriteLine(creditCardString);

                valid = CreditCardFunctions.IsValid(creditCardNumber);

                if (valid)
                {
                    type = CreditCardFunctions.FindType(creditCardNumber);
                    Console.WriteLine(type);
                    APIresponse = CreditCardFunctions.MoneyRequest(creditCardNumber, transaction.balance);

                    if (APIresponse[1] == "declined")
                    {
                        Console.WriteLine(" Card Declined by bank.");
                        Console.WriteLine($"Declined Amount: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}.");
                        CCdeclined(transaction, payment, type, transaction.balance);
                        return payment;
                    }
                    else
                    {
                        bool parseSuccess = decimal.TryParse(APIresponse[1], out decimal approvedAmount);
                        if (parseSuccess == true && approvedAmount > 0)
                        {
                            if (approvedAmount == transaction.balance)
                            {
                                Console.WriteLine("Transaction Approved");
                                Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                                Console.WriteLine();

                                CCaccepted(transaction, payment, type, approvedAmount);
                                WaitForKeyorSeconds(3);
                                return payment;
                            }

                            else if ( approvedAmount < transaction.balance )
                            {
                                Console.WriteLine("Transaction Partially Approved:");
                                Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                                payment.IsPartialPayment = true;
                                CCaccepted(transaction, payment, type, approvedAmount);
                                WaitForKeyorSeconds();
                                return payment;

                            }
                            else
                            {
                                Console.WriteLine("Error- GetCardPayment");
                                WaitForKeyorSeconds(20);
                                return payment;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Card Number Invalid. Try again. Or enter \"exit\" to quit.");
                }

            } while (!valid && creditCardNumber != "exit");

            return payment;
        }
        static string CustomerMobileNumberMasking(string input)
        {
            string Output = "";
            
            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    string pattern = @"\d(?!\d{0,1}$)";
                    string result = Regex.Replace(input, pattern, m => new string('*', m.Length));
                    Output = result;

                }
                return Output;
            }
            catch (Exception ex)
            {

                return Output;
            }
        }

        static int GetCashBackAmt(CashDrawer drawer, decimal totalCash)
        {
            bool success;
            bool modSuccess = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"exit\" to return.");
                Console.WriteLine("Example: 10 or 20 or 50 or 100");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    return 0;
                }
                else
                {
                    success = int.TryParse(input, out int amount);

                    if (success && amount % 10 == 0 && amount < totalCash)
                    {
                        modSuccess = true;
                        return amount;
                    }
                    else if (amount > totalCash)
                    {
                        Console.WriteLine("Requested amount too large.");
                    }
                    else
                    {
                        Console.WriteLine("CashBack Error.");
                    }
                }
            } while (modSuccess == false);

            return 0;
        }

        static Payment GetCashPayments(Transaction transaction, CashDrawer drawer)
        {
            Payment payment = new Payment(transaction.transactionNumber, DateTime.Now, PaymentType.Cash);
            
            (decimal value, int index, bool exit) exit = (0M, 0, true);
            bool earlyExit = false;

            Console.Clear();
            Console.WriteLine($"Amount Due: {transaction.balance}");
            Console.WriteLine($"Input payment by individual bill or coin value. Format: ###.##. Example: 10.00.");
            Console.WriteLine("Enter 'exit' to return to Payment Methods");
            do
            {
                (decimal value, int index, bool exit) cashIn = ValidateCash(drawer);
                if (cashIn == exit)
                {
                    earlyExit = true;
                }
                else
                {
                    drawer.cashInDrawer[cashIn.index] += cashIn.value;
                    payment.cashAmount += cashIn.value;
                    transaction.balance -= cashIn.value;
                    transaction.totalCashReceived += cashIn.value;
                    payment.success = true;

                    if (transaction.balance > 0)
                    {
                        Console.WriteLine($"Amount Remaining: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}");
                    }
                }
            } while (transaction.balance > 0 && earlyExit != true);

            return payment;
        }
        static (decimal, int, bool) ValidateCash(CashDrawer drawer)
        {
            bool parseSuccess;
            bool valid;
            string[] splitString;
            decimal value;
            int denomIndex = 9999;
            do
            {
                parseSuccess = false;
                valid = false;
                splitString = Array.Empty<string>();


                Console.Write("$");
                string? stringValue = Console.ReadLine();
                parseSuccess = decimal.TryParse(stringValue, out value);

                if (parseSuccess && stringValue.Contains('.'))
                {
                    splitString = stringValue.Split(".");
                    if (splitString.Length != 2 || splitString[1].Length != 2)
                    {
                        valid = false;
                        Console.WriteLine("Error. Try Again.");
                        continue;
                    }
                    for (int i = 0; i < drawer.values.Length; i++)
                    {
                        if (drawer.values[i] == value)
                        {
                            denomIndex = i;
                            valid = true;
                        }
                    }
                    if (valid == false)
                    {
                        Console.WriteLine("Error. Try again.");
                    }
                }
                else if (stringValue == "exit")
                {
                    return (0M, 0, true);
                }
                else
                {
                    valid = false;
                    Console.WriteLine("Error. Try Again.");
                }

            } while (valid == false);

            return (value, denomIndex, false);
        }

        static decimal InputItems()
        {
            decimal total = 0.0M;
            int itemCount = 1;
            (decimal value, bool escape) itemTuple;

            do
            {
                itemTuple = EnterItems(itemCount);
                itemCount++;
                total += itemTuple.value;

            } while (itemTuple.escape != true);
            return total;
        }
        static (decimal, bool) EnterItems(int itemCount)
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

                Console.WriteLine($"Input item {itemCount}. Format: ###.##  Example: 5.00 or 54.87  Enter 0 or press 'Enter' when finished.");
                Console.Write("$");
                string? stringValue = Console.ReadLine();
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
                        Console.WriteLine("Formatting error. Try Again.");
                    }
                }
                else if (value == 0M)
                {
                    splitString = Array.Empty<string>();
                    escape = true;
                }
                else
                {
                    valid = false;
                    Console.WriteLine("Formatting error. Try Again.");

                }

            } while (valid == false && escape != true);
            if (escape == true)
            {
                return (value, escape);
            }
            else
            {

                return (value, escape);
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
        static void WaitForKeyorSeconds(double seconds = 5.0, string text ="")
        {
            Console.WriteLine(text);
            Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(seconds));
        }
        static void CCaccepted(Transaction transaction, Payment payment, CardType type, decimal approvedAmount)
        {
            payment.success = true;
            payment.ccVendor = type;
            payment.ccAmount = approvedAmount;
            payment.paymentType = PaymentType.Card;

            transaction.balance -= payment.ccAmount;
            transaction.totalCCreceived += payment.ccAmount;

            transaction.paymentsList.Add(payment);

        }
        static void CCdeclined(Transaction transaction, Payment payment, CardType type, decimal declinedAmount)
        {
            payment.success = false;
            payment.declined = true;

            payment.ccVendor = type;
            payment.declinedAmount = declinedAmount;
            payment.paymentType = PaymentType.Card;

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
    }
}

