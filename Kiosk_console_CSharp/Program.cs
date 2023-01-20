
using System.Data.SqlTypes;
using System.Globalization;
using System.Net.Quic;
using System.Runtime.InteropServices;
using System.Xml.Schema;

//Console.WriteLine(value.ToString("C", CultureInfo.CurrentCulture));

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
        public Decimal totalCashRecieved;
        //public string ccVendor;
        public Decimal CCtotalRecieved;
        public Decimal change;

        public bool IsCBrequested;
        public Decimal cashBackDispensed;
        public Decimal cashBackOwed;
        public decimal total;
        public decimal balance;

        public List<Payment> paymentsList = new();

        public Transaction(decimal atotal, System.Guid atransactionNumber, DateTime atransactionDateTime)
        {
            total = atotal;
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
        //public int denomIndex;
        
        public PaymentType paymentType;
        public bool success;

        public CardType? ccVendor;
        public decimal ccAmount;
        public bool declined;
        public decimal cashBackAmount;
        public bool IsCBrequested;

        public Payment(Guid atransactionNumber, DateTime aDateTime, PaymentType apaymentType)
        {
            transactionNumber = atransactionNumber;
            datetime = aDateTime;
            paymentType = apaymentType;
        }
        //public void ccSuccessful(CardType type, decimal approvedAmount, decimal cashBackAmount)
        //{
        //    success = true;
        //    ccVendor = type;
        //    ccAmount = approvedAmount;
        //    paymentType = PaymentType.Card;
        //    this.cashBackAmount = cashBackAmount;
            
        //    if(cashBackAmount > 0)
        //    {
        //        IsCBrequested = true;
        //    }

        //}
        //public void ccDeclined(CardType type, decimal declinedAmount, decimal cashBackAmount)
        //{
        //    success = false;
        //    declined = true;

        //    ccVendor = type;
        //    ccAmount = declinedAmount;
        //    paymentType = PaymentType.Card;
        //    this.cashBackAmount = cashBackAmount;

        //    if (cashBackAmount > 0)
        //    {
        //        IsCBrequested = true;
        //    }
        //}
        //public void cashAccepted(decimal cashAmount)
        //{
        //    this.cashAmount = cashAmount;
        //    success = true;
        //    paymentType = PaymentType.Cash;
        //}
    }
    public enum PaymentType
    {
        Cash, Card
    }
    public enum CardType
    {
        MasterCard, Visa, AmericanExpress, Discover, JCB
    };

    partial class Program
    {
        static void Main(string[] args)
        {
            CashDrawer drawer = new();
            int userSelection;
            bool success;
            bool quit = false;
            do
            {
                userSelection = 0;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Press 1 to begin transaction.");
                    success = int.TryParse(Console.ReadLine(), out userSelection);
                } while (success == false || !(userSelection == 1 || userSelection == 9));

                if (userSelection == 1)
                {
                    drawer.Pennies = -1;
                    Console.WriteLine(drawer.Pennies.ToString()) ;
                    TransactionFunction(drawer);
                    
                }
                else if (userSelection == 9)
                {
                    quit = true;
                }
            } while (quit == false);
            Console.ReadLine();
        }
        static void TransactionFunction(CashDrawer drawer)
        {
            bool insuffChange = false;

            decimal total = InputItems();

            if (total > 0)
            {

                Transaction transaction = new(total, Guid.NewGuid(), new DateTime());

                //transaction.total = total;
                //transaction.balance = transaction.total;

                Payments(transaction, drawer);

                if (transaction.change < 0)
                {

                    decimal changeTotal = (decimal)transaction.change;

                    changeTotal = Math.Abs(changeTotal);

                    int[] changeCounts = new int[13];
                    bool changeCounted = GetChangeCounts(changeCounts, changeTotal, drawer);

                    if (changeCounted)
                    {
                        bool success = GiveChange(transaction, drawer, changeCounts, changeTotal);
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
                TransactionsList.Transactions.Add(transaction);
            }
            //Console.ReadLine();
        }

        static bool GiveChange(Transaction transaction, CashDrawer drawer, int[] changeCounts, decimal changeAmounts ) // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER
        {
            for (int i = 0; i < changeCounts.Length; i++)
            {
                if (changeCounts[i] > 0)
                {
                    drawer.cashInDrawer[i] -= changeCounts[i] * drawer.values[i];
                    transaction.balance += changeCounts[i] * drawer.values[i];

                    for (int j = 0; j < changeCounts[i]; j++)
                    {
                        Console.WriteLine($"{drawer.values[i]} dispensed");
                    }
                }
            }
            if (transaction.balance == 0)
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
            decimal totalPayments;
            //decimal originalTotal = transaction.total;
            Payment payment;
            //decimal change = 0M;
            //string creditCardNumber = "378282246310005";  // American Express

            //Console.WriteLine($"Balance Due: {transaction.total.ToString("C", CultureInfo.CurrentCulture)}");
            do
            {
                int userSelection;
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
                    //Console.WriteLine("Press 9 to cancel");
                    successful = int.TryParse(Console.ReadLine(), out userSelection);
                } while (successful == false || userSelection < 1 || userSelection > 3);

                if (userSelection == 1)
                {
                    payment = GetCashPayments(transaction, drawer);
                    //transaction.totalCashRecieved = payment.cashAmount;
                    //transaction.balance -= payment.cashAmount;
                    transaction.paymentsList.Add(payment);
                }
                else if (userSelection == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Balance due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));
                    //CCTest();
                    payment = GetCardPayment(transaction, drawer);

                    ////move balance reduction to moment of acceptance/approval
                    //if (payment.success == true)
                    //{
                    //    //transaction.balance -= payment.ccAmount;
                    //    //transaction.CCtotalRecieved += payment.ccAmount;
                    //    //transaction.paymentsList.Add(payment);
                    //}

                    totalPayments = GetPaymentsTotal(transaction.paymentsList);
                    
                    if (totalPayments >= transaction.total + transaction.cashBackOwed)
                    {
                        transaction.change -= transaction.cashBackOwed;
                        return;

                    }
                }

            } while (transaction.balance + transaction.cashBackOwed > 0 /*|| (transaction.balance)*/ );
        }

        private static Payment GetCardPayment(Transaction transaction, CashDrawer drawer)
        {
            Payment payment = new(transaction.transactionNumber, DateTime.Now, PaymentType.Card);
            CardType type;
            string creditCardNumber = "4716023102375986";  // Visa
            bool valid = false;
            string[] APIresponse;
            int cashBackAmount = 0;
            //decimal approvedAmount;
            decimal totalAmountDue;
            bool IsCashBackRequested = false;
            decimal totalCash = drawer.GetTotalCashInDrawer();

            //if (transaction.cashBackOwed == 0)
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
                        
                       
                        //payment.IsCBrequested = true;  //  Think about adding/replacing above
                    }
                }
            }

            totalAmountDue = transaction.balance + cashBackAmount;

            do
            {
                //decimal ccPaymentAmount = CcEnterAmount(transaction, payment, totalAmountDue);
                if (totalAmountDue != 0)
                {
                    Console.Clear();
                    Console.WriteLine("Please enter card number. Use dash '-' or space ' ' between segments. Format: 0000-0000-0000-0000");
                    string userInput = Console.ReadLine();

                    valid = CreditCardFunctions.IsValid(creditCardNumber);

                    if (valid)
                    {
                        type = CreditCardFunctions.FindType(creditCardNumber);
                        Console.WriteLine(type);
                        APIresponse = CreditCardFunctions.MoneyRequest(creditCardNumber, totalAmountDue);

                        if (APIresponse[1] == "declined")
                        {
                            Console.WriteLine(" Card Declined by bank.");
                            ccDeclined(transaction, payment, type, totalAmountDue, cashBackAmount);
                            //payment.success = false;
                            //payment.declined = true;
                            return payment;
                        }
                        else
                        {
                            bool parseSuccess = decimal.TryParse(APIresponse[1], out decimal approvedAmount);
                            if (parseSuccess == true && approvedAmount > 0)
                            {
                                if (transaction.IsCBrequested == false && approvedAmount <= totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Approved");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    CCaccepted(transaction, payment, type, approvedAmount, cashBackAmount);
                                    WaitForKeyorSeconds();
                                    return payment;
                                }

                                else if (transaction.IsCBrequested && approvedAmount == totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Approved");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    CCaccepted(transaction, payment, type, approvedAmount, cashBackAmount);
                                    Console.WriteLine($"Cash back: {cashBackAmount}");
                                    //transaction.IsCBrequested = true;
                                   // transaction.cashBackOwed = cashBackAmount;
                                    //transaction.balance += transaction.cashBackOwed;
                                    WaitForKeyorSeconds();
                                    return payment;

                                    //transaction.balance -= payment.ccAmount;
                                    //transaction.CCtotalRecieved += payment.ccAmount;
                                    //transaction.paymentsList.Add(payment);

                                }
                                else if (transaction.IsCBrequested && approvedAmount < totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Partially Approved:");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    CCaccepted(transaction, payment, type, approvedAmount, cashBackAmount);
                                   // transaction.IsCBrequested = true;
                                   // transaction.cashBackOwed = cashBackAmount;
                                    WaitForKeyorSeconds();
                                    return payment;

                                }
                                else
                                {
                                    payment.success = false;
                                    Console.WriteLine("Error- GetCardPayment");
                                    Console.WriteLine($"Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
                                    WaitForKeyorSeconds();
                                    return payment;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Card Number Invalid. Try again. Or enter \"exit\" to quit.");
                    }
                }
                
                

            } while (!valid || creditCardNumber != "exit");

           return payment;

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
            //decimal change = 0M;
            Payment payment = new Payment(transaction.transactionNumber, DateTime.Now, PaymentType.Cash);
            //payment.transactionNumber = ;
            //decimal total = transaction.balance;
            (decimal value, int index, bool finished) exit = (0M, 0, true);
            bool earlyExit = false;

            Console.WriteLine($"Amount Due: {transaction.balance}");
            Console.WriteLine($"Input payment by individual bill or coin value. Format: ###.##. Example: 10.00.");
            Console.WriteLine("Enter 'exit' to return to Payment Methods");
            do
            {
                (decimal value, int index, bool finished) cashIn = ValidateCash(drawer);
                if (cashIn == exit)
                {
                    earlyExit = true;
                }
                else
                {
                    drawer.cashInDrawer[cashIn.index] += cashIn.value;
                    payment.cashAmount += cashIn.value;
                    transaction.balance -= cashIn.value;
                    transaction.totalCashRecieved += cashIn.value;
                    //transaction.paymentsList.Add(payment);

                    if (transaction.balance > 0)
                    {
                        Console.WriteLine($"Amount Remaining: {transaction.balance.ToString("C", CultureInfo.CurrentCulture)}");

                    }
                }
            } while (transaction.balance > 0 && earlyExit != true);

            if (transaction.balance < 0)
            {
                transaction.change = transaction.balance;

                Console.WriteLine($"Change Due: {transaction.change.ToString("C", CultureInfo.CurrentCulture)}");

            }
            else if (transaction.balance == 0)
            {
                Console.WriteLine($"Transaction Complete.");

                Console.WriteLine($"Thank you for your payment of ${transaction.total.ToString("C", CultureInfo.CurrentCulture)} ");
            }
            payment.success = true;
            return payment;
        }
        static (decimal, int, bool) ValidateCash(CashDrawer drawer)
        {
            bool parseSuccess;
            bool valid;
            string[] splitString;
            decimal value;
            int denomIndex = 9999;
            bool finished;
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
            char quit;
            decimal total = 0.0M;
            decimal value;
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
                    splitString = stringValue.Split(".");

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
                    else
                    {

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
        static void WaitForKeyorSeconds(double seconds=5.0)
        {
            Console.WriteLine("Press any key to continue");
            Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(seconds));
        }
        static void CCaccepted(Transaction transaction, Payment payment, CardType type, decimal approvedAmount, decimal cashBackAmount)
        {
            payment.success = true;
            payment.ccVendor = type;
            payment.ccAmount = approvedAmount;
            payment.paymentType = PaymentType.Card;
            payment.cashBackAmount = cashBackAmount;

            if (cashBackAmount > 0)
            {
                payment.IsCBrequested = true;
                transaction.IsCBrequested = true;

            }

            transaction.balance -= payment.ccAmount;
            transaction.CCtotalRecieved += payment.ccAmount;
            transaction.cashBackOwed = cashBackAmount;
            transaction.paymentsList.Add(payment);

        }
        static void ccDeclined(Transaction transaction, Payment payment, CardType type, decimal declinedAmount, decimal cashBackAmount)
        {
            payment.success = false;
            payment.declined = true;

            payment.ccVendor = type;
            payment.ccAmount = declinedAmount;
            payment.paymentType = PaymentType.Card;
            payment.cashBackAmount = cashBackAmount;

            if (cashBackAmount > 0)
            {
                payment.IsCBrequested = true;
               
            }
        }
        static void cashAccepted(Transaction transaction, Payment payment, decimal cashAmount)
        {
            payment.cashAmount = cashAmount;
            payment.success = true;
            payment.paymentType = PaymentType.Cash;
        }
    }
}

