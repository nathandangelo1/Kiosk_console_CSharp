namespace Kiosk_Console_CSharp
{

    // CLASS CONTAINS PROPERTIES
public class CashDrawer
    {
        public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarCoinDec = 1.00M, dollarDec = 1.00M, twoDollarDec = 2.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

        public readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, twoDollarDec, dollarDec, dollarCoinDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };
        public readonly string[] valueNames = { "hundreds", "fifties", "twenties", "tens", "fives", "twos", "dollars", "dollarCoin", "halfdollar", "quarters", "dimes", "nickels", "pennies" };

        public decimal[] cashInDrawer;

        private decimal _pennies, _nickels, _dimes, _quarters, _halfDollars, _dollarCoins, _dollars, _twoDollars, _fives, _tens, _twenties, _fifties, _hundreds;


        //Properties
        #region
        private decimal Pennies
        {
            get
            {
                return _pennies;
            }
            set
            {
                if (value >= 0 && value % pennyDec == 0)
                {
                    _pennies = value;
                }

            }
        }
        private decimal Nickels
        {
            get
            {
                return _nickels;
            }
            set
            {
                if (value >= 0 && value % nickelDec == 0)
                {
                    _nickels = value;
                }
            }
        }
        private decimal Dimes
        {
            get
            {
                return _dimes;
            }
            set
            {
                if (value >= 0 && value % dimeDec == 0)
                {
                    _dimes = value;
                }
            }
        }
        private decimal Quarters
        {
            get
            {
                return _quarters;
            }
            set
            {
                if (value >= 0 && value % quarterDec == 0)
                {
                    _quarters = value;
                }
            }
        }
        private decimal Halfdollars
        {
            get
            {
                return _halfDollars;
            }
            set
            {
                if (value >= 0 && value % halfdollarDec == 0)
                {
                    _halfDollars = value;
                }
            }
        }
        private decimal DollarCoins
        {
            get
            {
                return _dollarCoins;
            }
            set
            {
                if (value >= 0 && value % dollarCoinDec == 0)
                {
                    _dollarCoins = value;
                }
            }
        }
        private decimal Dollars
        {
            get
            {
                return _dollars;
            }
            set
            {
                if (value >= 0 && value % dollarDec == 0)
                {
                    _dollars = value;
                }
            }
        }
        private decimal TwoDollars
        {
            get
            {
                return _twoDollars;
            }
            set
            {
                if (value >= 0 && value % twoDollarDec == 0)
                {
                    _twoDollars = value;
                }
            }
        }
        private decimal Fives
        {
            get
            {
                return _fives;
            }
            set
            {
                if (value >= 0 && value % fiveDec == 0)
                {
                    _fives = value;
                }
            }
        }
        private decimal Tens
        {
            get
            {
                return _tens;
            }
            set
            {
                if (value >= 0 && value % tenDec == 0)
                {
                    _tens = value;
                }
            }
        }
        private decimal Twenties
        {
            get
            {
                return _twenties;
            }
            set
            {
                if (value >= 0 && value % pennyDec == 0)
                {
                    _twenties = value;
                }
            }
        }
        private decimal Fifties
        {
            get
            {
                return _fifties;
            }
            set
            {
                if (value >= 0 && value % fiftyDec == 0)
                {
                    _fifties = value;
                }
            }
        }
        private decimal Hundreds
        {
            get
            {
                return _hundreds;
            }
            set
            {
                if (value >= 0 && value % hundredDec == 0)
                {
                    _hundreds = value;
                }
            }
        }
        #endregion//

        public CashDrawer(decimal pennyTotal = 1.00M, decimal nickelTotal = 4.00M, decimal dimeTotal = 10.00M, decimal quarterTotal = 20.00M, decimal halfdollarTotal = 0.00M, decimal dollarCoinTotal = 0.00M, decimal dollarTotal = 100.00M, decimal twoDollarTotal = 0.00M, decimal fiveTotal = 100.00M, decimal tenTotal = 250.00M, decimal twentyTotal = 500.00M, decimal fiftyTotal = 0.00M, decimal hundredTotal = 500.00M)
        {
            cashInDrawer = new decimal[13];

            Pennies = pennyTotal;
            Nickels = nickelTotal;
            Dimes = dimeTotal;
            Quarters = quarterTotal;
            Halfdollars = halfdollarTotal;
            DollarCoins = dollarCoinTotal;
            Dollars = dollarTotal;
            TwoDollars = twoDollarTotal;
            Fives = fiveTotal;
            Tens = tenTotal;
            Twenties = twentyTotal;
            Fifties = fiftyTotal;
            Hundreds = hundredTotal;

            cashInDrawer[0] = Hundreds;
            cashInDrawer[1] = Fifties;
            cashInDrawer[2] = Twenties;
            cashInDrawer[3] = Tens;
            cashInDrawer[4] = Fives;
            cashInDrawer[5] = TwoDollars;
            cashInDrawer[6] = Dollars;
            cashInDrawer[7] = DollarCoins;
            cashInDrawer[8] = Halfdollars;
            cashInDrawer[9] = Quarters;
            cashInDrawer[10] = Dimes;
            cashInDrawer[11] = Nickels;
            cashInDrawer[12] = Pennies;




        }


        public decimal GetTotalCashInDrawer()
        {
            decimal total = 0M;
            foreach (var item in cashInDrawer)
            {
                total += item;
            }
            return total;
        }
    }
}

