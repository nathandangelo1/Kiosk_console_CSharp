using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Kiosk_Console_CSharp
{

    public class CashDrawer
    {
        public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarCoinDec = 1.00M, dollarDec = 1.00M, twoDollarDec = 2.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

        public readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, twoDollarDec, dollarDec, dollarCoinDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };
        public readonly string[] valueNames = { "hundreds", "fifties", "twenties", "tens", "fives", "twos", "dollars", "dollarCoin", "halfdollar", "quarters", "dimes", "nickels", "pennies" };

        public decimal[] cashInDrawer;



        private decimal _pennyCount, _nickelCount, _dimeCount, _quarterCount, _halfDollarCount, _dollarCoinCount, _dollarCount, _twoDollarCount, _fiveCount, _tenCount, _twentyCount, _fiftyCount, _hundredCount;

        //Properties
        #region
        public decimal Pennies
        {
            get
            {
                return _pennyCount;
            }
            set
            {
                if (value > 0)
                {
                    _pennyCount = value;
                }
            }
        }
        public decimal Nickels
        {
            get
            {
                return _nickelCount;
            }
            set
            {
                if (value > 0)
                {
                    _nickelCount = value;
                }
            }
        }
        public decimal Dimes
        {
            get
            {
                return _dimeCount;
            }
            set
            {
                if (value > 0)
                {
                    _dimeCount = value;
                }
            }
        }
        public decimal Quarters
        {
            get
            {
                return _quarterCount;
            }
            set
            {
                if (value > 0)
                {
                    _quarterCount = value;
                }
            }
        }
        public decimal Halfdollars
        {
            get
            {
                return _halfDollarCount;
            }
            set
            {
                if (value > 0)
                {
                    _halfDollarCount = value;
                }
            }
        }
        public decimal DollarCoins
        {
            get
            {
                return _dollarCoinCount;
            }
            set
            {
                if (value > 0)
                {
                    _dollarCoinCount = value;
                }
            }
        }
        public decimal Dollars
        {
            get
            {
                return _dollarCount;
            }
            set
            {
                if (value > 0)
                {
                    _dollarCount = value;
                }
            }
        }
        public decimal TwoDollars
        {
            get
            {
                return _twoDollarCount;
            }
            set
            {
                if (value > 0)
                {
                    _twoDollarCount = value;
                }
            }
        }
        public decimal Fives
        {
            get
            {
                return _fiveCount;
            }
            set
            {
                if (value > 0)
                {
                    _fiveCount = value;
                }
            }
        }
        public decimal Tens
        {
            get
            {
                return _tenCount;
            }
            set
            {
                if (value > 0)
                {
                    _tenCount = value;
                }
            }
        }
        public decimal Twenties
        {
            get
            {
                return _twentyCount;
            }
            set
            {
                if (value > 0)
                {
                    _twentyCount = value;
                }
            }
        }
        public decimal Fifties
        {
            get
            {
                return _fiftyCount;
            }
            set
            {
                if (value > 0)
                {
                    _fiftyCount = value;
                }
            }
        }
        public decimal Hundreds
        {
            get
            {
                return _hundredCount;
            }
            set
            {
                if (value > 0)
                {
                    _hundredCount = value;
                }
            }
        }
        #endregion//

        public CashDrawer(decimal apennyCount = 1.00M, decimal anickelCount = 4.00M, decimal adimeCount = 10.00M, decimal aquarterCount = 20.00M, decimal ahalfDollarCount = 0.00M, decimal adollarCoinCount = 0.00M, decimal adollarCount = 100.00M, decimal atwoDollarCount = 0.00M, decimal afiveCount = 100.00M, decimal atenCount = 250.00M, decimal atwentyCount = 500.00M, decimal afiftyCount = 0.00M, decimal ahundredCount = 500.00M)
        {
            cashInDrawer = new decimal[13];

            Pennies = apennyCount;
            Nickels = anickelCount;
            Dimes = adimeCount;
            Quarters = aquarterCount;
            Halfdollars = ahalfDollarCount;
            DollarCoins = adollarCoinCount;
            Dollars = adollarCount;
            TwoDollars = atwoDollarCount;
            Fives = afiveCount;
            Tens = atenCount;
            Twenties = atwentyCount;
            Fifties = afiftyCount;
            Hundreds = ahundredCount;

            cashInDrawer[0] = _hundredCount;
            cashInDrawer[1] = _fiftyCount;
            cashInDrawer[2] = _twentyCount;
            cashInDrawer[3] = _tenCount;
            cashInDrawer[4] = _fiveCount;
            cashInDrawer[5] = _twoDollarCount;
            cashInDrawer[6] = _dollarCount;
            cashInDrawer[7] = _dollarCoinCount;
            cashInDrawer[8] = _halfDollarCount;
            cashInDrawer[9] = _quarterCount;
            cashInDrawer[10] = _dimeCount;
            cashInDrawer[11] = _nickelCount;
            cashInDrawer[12] = _pennyCount;

        }
    }

    partial class Program
    {
        static void Main(string[] args)
        {
            int userSelection;
            bool success;
            do
            {
                Console.WriteLine("Press 1 to begin transaction");
                success = int.TryParse(Console.ReadLine(), out userSelection);
            } while (success == false || userSelection != 1);

            if (userSelection == 1)
            {
                Transaction();
            }
        }
        static void Transaction()
        {
            bool insuffChange = false;
            CashDrawer drawer = new CashDrawer();

            decimal total = InputItems();

            decimal changeTotal = Payments(total, drawer);

            if (changeTotal == 0)
            {

            }
            else
            {

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
            Console.ReadLine();
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
        static decimal Payments(decimal total, CashDrawer drawer)
        {
            bool success;
            decimal originalTotal = total;
            (decimal, int) value;
            decimal change = 0M;
            string creditCardNumber = "378282246310005";  // American Express

            Console.WriteLine($"Total Due: {total}");
            do
            {
                int userSelection;
                bool successful = false;
                do
                {
                    Console.WriteLine("Press 1 to enter cash");
                    Console.WriteLine("Press 2 to enter credit/debit");
                    Console.WriteLine("Press 9 to cancel");
                    successful = int.TryParse(Console.ReadLine(), out userSelection);
                } while (successful == false || userSelection < 1 || userSelection > 3);

                if (userSelection == 1)
                {
                    value = GetPaymentByDenom(total, drawer);
                }
                else /*(userSelection == 2)*/
                {
                    //CCTest();
                    value = GetCardPayment(total, drawer);
                }
                //else
                //{

                //}

                total -= value.Item1;
                drawer.cashInDrawer[value.Item2] += value.Item1;

                if (total > 0)
                {
                    Console.WriteLine($"Amount Remaining: {total}");
                }
            } while (total > 0);

            if (total < 0)
            {
                change = total;
                Console.WriteLine($"Change Due: {change}");
            }
            else if (total == 0)
            {
                Console.WriteLine($"Transaction Complete.");
                Console.WriteLine($"Thank you for your payment of ${originalTotal} ");
            }
            return change;
        }

        private static (decimal, int) GetCardPayment(decimal total, CashDrawer drawer)
        {
             CardType type;
            string creditCardNumber = "378282246310005";  // American Express

            Console.WriteLine("Please enter card number:");
            bool valid = IsValid(creditCardNumber);

            if (valid)
            {
                type = FindType(creditCardNumber);
                Console.WriteLine(type);
            }



        }

        static (decimal, int) GetPaymentByDenom(decimal total, CashDrawer drawer)
        {
            bool parseSuccess;
            bool valid;
            string[] splitString;
            decimal value;
            int denomIndex = 0;
            do
            {
                parseSuccess = false;
                valid = true;
                splitString = Array.Empty<string>();

                Console.WriteLine($"Input payment by individual bill or coin value. Format: ###.##. Example: 10.00 ");
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
                        continue;
                    }
                }
                else
                {
                    valid = false;
                    Console.WriteLine("Formatting error. Try Again.");
                    continue;
                }
                bool contained = false;
                for (int i = 0; i < drawer.values.Length; i++)
                {
                    if (drawer.values[i] == value)
                    {
                        contained = true;
                        denomIndex = i;

                    }
                }
                if (contained == true)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                    Console.WriteLine("Formatting error. Try Again.");

                }
            } while (valid == false);

            return (value, denomIndex);
        }

        static decimal InputItems()
        {
            char quit;
            decimal total = 0.0M;
            decimal value;
            int itemCount = 1;
            (decimal, char) itemTuple;

            do
            {
                itemTuple = EnterItems(itemCount);
                itemCount++;
                total += itemTuple.Item1;

            } while (itemTuple.Item2 != 'z');
            return total;
        }
        static (decimal, char) EnterItems(int itemCount)
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
                return (value, 'z');
            }
            else
            {

                return (value, 'y');
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

        static decimal GetDrawerTotal(CashDrawer drawer1)
        {
            decimal bank = 0.00M;
            foreach (var item in drawer1.cashInDrawer)
            {
                bank += item;
            }
            //Console.WriteLine(bank);
            return bank;
        }
        static string[] MoneyRequest(string account_number, decimal amount)
        {
            Random rnd = new Random();
            //50% CHANCE TRANSACTION PASSES OR FAILS
            bool pass = rnd.Next(100) < 50;
            //50% CHANCE THAT A FAILED TRANSACTION IS DECLINED
            bool declined = rnd.Next(100) < 50;
            if (pass)
            {
                return new string[] { account_number, amount.ToString() };
            }
            else
            {
                if (!declined)
                {
                    return new string[] { account_number, (amount / rnd.Next(2, 6)).ToString() };
                }
                else
                {
                    return new string[] { account_number, "declined" };
                }//end if
            }//end if
        }//end if

        static void CCTest()
        {
            string[] cards = new string[] {
                //http://www.getcreditcardnumbers.com/
                "378282246310005",  // American Express
                "4716023102375986", // Visa
                "6011206705780705", // Discover
                "5333176180410867", // Mastercard
            };

            foreach (string card in cards)
            {
                Console.WriteLine(IsValid(card));
            }

            Console.ReadLine();
        }

        public static bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string ccValue = value as string;
            if (ccValue == null)
            {
                return false;
            }
            ccValue = ccValue.Replace("-", "");
            ccValue = ccValue.Replace(" ", "");

            int checksum = 0;
            bool evenDigit = false;

            foreach (char digit in ccValue.Reverse())
            {
                if (digit < '0' || digit > '9')
                {
                    return false;
                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }

            return checksum % 10 == 0;
        }

        public enum CardType
        {
            MasterCard, Visa, AmericanExpress, Discover, JCB
        };

        public static CardType FindType(string cardNumber)
        {
            //https://www.regular-expressions.info/creditcard.html
            if (Regex.Match(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            {
                return CardType.Visa;
            }

            if (Regex.Match(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            {
                return CardType.MasterCard;
            }

            if (Regex.Match(cardNumber, @"^3[47][0-9]{13}$").Success)
            {
                return CardType.AmericanExpress;
            }

            if (Regex.Match(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success)
            {
                return CardType.Discover;
            }

            throw new Exception("Unknown card.");
        }

    }
}
        
