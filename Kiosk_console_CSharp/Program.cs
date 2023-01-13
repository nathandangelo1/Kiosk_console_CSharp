
using System.Data.SqlTypes;
using System.Globalization;
using System.Net.Quic;
using System.Runtime.InteropServices;
using System.Xml.Schema;

//Console.WriteLine(value.ToString("C", CultureInfo.CurrentCulture));

namespace Kiosk_Console_CSharp
{
    public class Transaction
    {
        public System.Guid transactionNumber;
        public DateTime transactionDate;
        public DateTime transactionTime;
        public Decimal? totalCashRecieved;
        public string? ccVendor;
        public Decimal? ccAmount;
        public Decimal change;

        public bool cashback;
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
    public struct Payment
    {
        public System.Guid transactionNumber;
        public DateTime datetime;

        public decimal cashAmount;
        //public int denomIndex;
        public PaymentType paymentType;
        public bool success;

        public CardType? ccVendor;
        public Decimal ccAmount;
        public bool declined;
        // public bool? cashBack;

        public Payment(Guid atransactionNumber, DateTime aDateTime, PaymentType apaymentType)
        {
            transactionNumber = atransactionNumber;
            datetime = aDateTime;
            paymentType = apaymentType;
        }
        public void ccSuccessful(CardType type, decimal approvedAmount, bool success = true)
        {
            this.success = true;
            this.ccVendor = type;
            this.ccAmount = approvedAmount;
            this.paymentType = PaymentType.Card;
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

    partial class Program
    {
        static void Main(string[] args)
        {
            CashDrawer drawer = new CashDrawer();
            int userSelection;
            bool success;
            bool quit = false;
            do
            {
                do
                {
                    Console.WriteLine("Press 1 to begin transaction.");
                    success = int.TryParse(Console.ReadLine(), out userSelection);
                } while (success == false || !(userSelection == 1 || userSelection == 9));

                if (userSelection == 1)
                {
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

                transaction.total = total;
                transaction.balance = transaction.total;

                Payments(transaction, drawer);

                if (transaction.change < 0)
                {
                    decimal changeTotal = (decimal)transaction.change;

                    changeTotal = Math.Abs(changeTotal);

                    int[] changeCounts = new int[13];
                    bool changeCounted = GetChangeCounts(changeCounts, changeTotal, drawer);

                    if (changeCounted)
                    {
                        GiveChange(changeCounts, drawer);
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
            }
            //Console.ReadLine();
        }

        static void GiveChange(int[] changeCounts, CashDrawer drawer) // DISPENSES CHANGE FROM DENOMS IN DRAWER, DEDUCTS VALUE FROM CASHINDRAWER
        {
            for (int i = 0; i < changeCounts.Length; i++)
            {
                if (changeCounts[i] > 0)
                {
                    drawer.cashInDrawer[i] -= changeCounts[i] * drawer.values[i];

                    for (int j = 0; j < changeCounts[i]; j++)
                    {
                        Console.WriteLine($"{drawer.values[i]} dispensed");
                    }
                }
            }
        }
        static void Payments(Transaction transaction, CashDrawer drawer)
        {
            bool success;
            decimal totalPayments = 0M;
            //decimal originalTotal = transaction.total;
            Payment payment;
            decimal change = 0M;
            string creditCardNumber = "378282246310005";  // American Express

            //Console.WriteLine($"Balance Due: {transaction.total.ToString("C", CultureInfo.CurrentCulture)}");
            do
            {
                int userSelection;
                bool successful = false;
                do
                {
                    Console.WriteLine("Balance Due: " + transaction.balance.ToString("C", CultureInfo.CurrentCulture));
                    if (transaction.cashback == false)
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
                    transaction.totalCashRecieved = payment.cashAmount;
                    transaction.paymentsList.Add(payment);


                }
                else if (userSelection == 2)
                {
                    Console.WriteLine("Balance due:" + transaction.balance.ToString("C", CultureInfo.CurrentCulture));
                    //CCTest();
                    payment = GetCardPayment(transaction, drawer);

                    //move balance reduction to moment of acceptance/approval
                    transaction.balance -= payment.ccAmount;
                    transaction.ccAmount += payment.ccAmount;
                    transaction.paymentsList.Add(payment);
                    totalPayments = GetPaymentsTotal(transaction.paymentsList);

                    if (totalPayments > transaction.total)
                    // if( transaction.cashback == true && transaction.cashBackOwed > 0 )
                    {
                        if (transaction.balance > 0)
                        {
                            Console.WriteLine("ERROR. Payments: balance > 0 ");
                        }
                        //payment.cashBack = true;
                        //transaction.cashBackOwed = payment.amount - transaction.balance;
                        transaction.change -= transaction.cashBackOwed;
                    }
                }
                if (totalPayments >= transaction.balance)
                {
                    return;
                }

            } while (transaction.balance > 0);

        }

        private static Payment GetCardPayment(Transaction transaction, CashDrawer drawer)
        {
            Payment payment = new Payment(transaction.transactionNumber, DateTime.Now, PaymentType.Card);
            CardType type;
            string creditCardNumber = "4716023102375986";  // Visa
            bool valid = false;
            string[] APIresponse;
            int cashbackPreApproval = 0;
            //decimal approvedAmount;
            decimal totalAmountDue;
            bool cashBackRequested = false;
            decimal totalCash = drawer.GetTotalCashInDrawer();

            if (transaction.cashBackOwed == 0)
            {
                Console.WriteLine("Cash Back?  y/n");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    cashbackPreApproval = CashBack(drawer, totalCash);
                    if (cashbackPreApproval > 0)
                    {
                        cashBackRequested = true;
                    }
                }
            }

            totalAmountDue = transaction.balance + cashbackPreApproval;

            do
            {
                //decimal ccPaymentAmount = CcEnterAmount(transaction, payment, totalAmountDue);
                if (totalAmountDue != 0)
                {
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
                            payment.success = false;
                            payment.declined = true;
                            return payment;
                        }
                        else
                        {
                            bool parseSuccess = decimal.TryParse(APIresponse[1], out decimal approvedAmount);
                            if (parseSuccess == true && approvedAmount > 0)
                            {
                                if (cashBackRequested == false && approvedAmount <= totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Approved");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    payment.ccSuccessful(type, approvedAmount);
                                    return payment;
                                }

                                else if (cashBackRequested && approvedAmount == totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Approved");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    payment.ccSuccessful(type, approvedAmount);
                                    transaction.cashback = true;
                                    transaction.cashBackOwed = cashbackPreApproval;
                                    return payment;

                                }
                                else if (cashBackRequested && approvedAmount < totalAmountDue)
                                {
                                    Console.WriteLine("Transaction Partially Approved:");
                                    Console.WriteLine($"{type}: Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");

                                    payment.ccSuccessful(type, approvedAmount);
                                    transaction.cashback = true;
                                    transaction.cashBackOwed = cashbackPreApproval;
                                    return payment;

                                }
                                else
                                {
                                    payment.success = false;
                                    Console.WriteLine("Error- GetCardPayment");
                                    Console.WriteLine($"Approved amount: {approvedAmount.ToString("C", CultureInfo.CurrentCulture)}");
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

        //static decimal CcEnterAmount(Transaction transaction, Payment payment, decimal amountdue)
        //{
        //    bool success;
        //    bool successfull;
        //    int choice;
        //    string userSelection;
        //    string[] splitString;
        //    bool valid = false;
        //    decimal partialAmount = 0M;

        //    Console.WriteLine("Amount Due: "+ amountdue.ToString("C", CultureInfo.CurrentCulture));
        //    Console.WriteLine("1 for full amount");
        //    Console.WriteLine("3 for partial amount");

        //    userSelection = Console.ReadLine();
        //    success = int.TryParse(userSelection, out choice);

        //    if (success == true && choice == 1)
        //    {
        //        return amountdue;
        //    }
        //    else if (choice == 3)
        //    {
        //        do
        //        {
        //            Console.WriteLine("Enter amount to two decimal places. Example: 34.01");
        //            //Console.WriteLine("Format: #####.##");
        //            string stringValue = Console.ReadLine();
        //            successfull = decimal.TryParse(stringValue, out partialAmount);
        //            if (successfull && stringValue.Contains('.') && (partialAmount < amountdue))
        //            {
        //                splitString = stringValue.Split(".");
        //                if (splitString.Length != 2 || splitString[1].Length != 2)
        //                {
        //                    valid = false;
        //                    Console.WriteLine("Error. Try Again.");
        //                    continue;
        //                }
        //                valid = true;
        //            }
        //            else if (stringValue == "exit")
        //            {
        //                return 0;
        //            }


        //        } while (valid == false);
        //    }
        //    return partialAmount;
        //}

        static int CashBack(CashDrawer drawer, decimal totalCash)
        {
            bool success;
            int amount;
            bool modSuccess = false;

            do
            {
                Console.WriteLine("Please enter Cash Back amount in increments of 10. Or enter \"exit\" to return.");
                Console.WriteLine("Example: 10 or 20 or 50 or 100");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    return 0;
                }
                else
                {
                    success = int.TryParse(input, out amount);

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
    }
}

