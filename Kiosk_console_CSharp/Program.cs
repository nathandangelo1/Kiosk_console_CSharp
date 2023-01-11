using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Kiosk_Console_CSharp
{
    //public enum Denoms
    //{
    //    penny, 
    //    nickel, 
    //    dime, 
    //    quarter, 
    //    half, 
    //    dollar, 
    //    five, 
    //    ten, 
    //    twenty, 
    //    fifty, 
    //    hundred
    //}
    public class CashDrawer
    {
        public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarDec = 1.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

        public readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, dollarDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };
        public readonly string[] valueNames = { "hundreds", "fifties", "twenties", "tens", "fives", "dollars", "halfdollar", "quarters", "dimes", "nickels", "pennies" };

        public decimal[] cashInDrawer;

        private decimal _pennyCount, _nickelCount, _dimeCount, _quarterCount, _halfDollarCount, _dollarCount, _fiveCount, _tenCount, _twentyCount, _fiftyCount, _hundredCount;

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

        //public const int pennyInt = 1, nickelInt = 5, dimeInt = 10, quarterInt = 25, halfInt = 50, dollarInt = 100, fiveInt = 500, tenInt = 1000, twentyInt = 2000, fiftyInt = 5000, hundredInt = 10000;
        //public readonly Denoms[] valueNames =   { Denoms.hundred,   Denoms.fifty, Denoms.twenty, Denoms.ten, Denoms.five, Denoms.dollar, Denoms.half, Denoms.quarter, Denoms.dime, Denoms.nickel, Denoms.penny };

        public CashDrawer(decimal apennyCount = 1.00M, decimal anickelCount = 4.00M, decimal adimeCount = 10.00M, decimal aquarterCount = 20.00M, decimal ahalfDollarCount = 0.00M, decimal adollarCount = 100.00M, decimal afiveCount = 100.00M, decimal atenCount = 250.00M, decimal atwentyCount = 500.00M, decimal afiftyCount = 0.00M, decimal ahundredCount = 500.00M)
        {
            cashInDrawer = new decimal[11];

            _pennyCount = apennyCount;
            _nickelCount = anickelCount;
            _dimeCount = adimeCount;
            _quarterCount = aquarterCount;
            _halfDollarCount = ahalfDollarCount;
            _dollarCount = adollarCount;
            _fiveCount = afiveCount;
            _tenCount = atenCount;
            _twentyCount = atwentyCount;
            _fiftyCount = afiftyCount;
            _hundredCount = ahundredCount;

            cashInDrawer[0] = _hundredCount;
            cashInDrawer[1] = _fiftyCount;
            cashInDrawer[2] = _twentyCount;
            cashInDrawer[3] = _tenCount;
            cashInDrawer[4] = _fiveCount;
            cashInDrawer[5] = _dollarCount;
            cashInDrawer[6] = _halfDollarCount;
            cashInDrawer[7] = _quarterCount;
            cashInDrawer[8] = _dimeCount;
            cashInDrawer[9] = _nickelCount;
            cashInDrawer[10] = _pennyCount;

        }
    }
    class Program
    {
        public Dictionary<string, decimal> denomDict = new Dictionary<string, decimal>() { { "hundred", CashDrawer.hundredDec }, { "fifty", CashDrawer.fiftyDec } };


        static void Main(string[] args)
        {
            bool insuffChange = false;
            CashDrawer drawer = new CashDrawer();

            decimal total = InputItems();

            decimal changeTotal = InputCashPayments(total);

            changeTotal = Math.Abs(changeTotal);

            //decimal changeTotal = 2152.96M;

            int[] changeCounts = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
        static decimal InputCashPayments(decimal total)
        {
            bool success;
            decimal value = 0.0M;

            Console.WriteLine($"Total Due: {total}");
            do
            {
                //Console.WriteLine($"Total: {total}");
                Console.WriteLine("Input cash:");
                success = decimal.TryParse(Console.ReadLine(), out value);
                total -= value;

                if (total > 0)
                {
                    Console.WriteLine($"Amount Remaining: {total}");
                }
            } while (total > 0);

            if(total < 0)
            {
                Console.WriteLine($"Change Due: {total}");
            }
            return total;
        }
        static decimal InputItems()
        {
            char quit;
            decimal total = 0.0M;
            decimal value;
            int itemCount = 1;
            (Decimal, char) itemTuple;

            do
            {
                itemTuple = EnterItems(itemCount);
                itemCount++;
                total += (itemTuple.Item1);

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

                Console.WriteLine($"Input item {itemCount}. Format: ###.##  Example: 5.00 or 54.87  ENTER 'z' when finished.");
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
                else if (stringValue == "Z" || stringValue == "z")
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
                if ((changeAmount / drawer.values[i]) >= 1)
                {
                    //INTEGER DIVISION GIVING THE NUMBER OF TIMES NUM CAN BE DIVIDED BY VALUE
                    temp = (int)(changeAmount / drawer.values[i]);

                    //IF THERE IS ENOUGH CHANGE OF DENOM TO MAKE CHANGE
                    if (drawer.cashInDrawer[i] >= (temp * drawer.values[i]))
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

        }//end main

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
    }
}