namespace Kiosk_Console_CSharp
{
    public class CashDrawer
    {

        public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarCoinDec = 1.00M, dollarDec = 1.00M, twoDollarDec = 2.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

        public readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, twoDollarDec, dollarDec, dollarCoinDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };
        public readonly string[] valueNames = { "hundreds", "fifties", "twenties", "tens", "fives", "twos", "dollars", "dollarCoin", "halfdollar", "quarters", "dimes", "nickels", "pennies" };

        public decimal[] cashInDrawer;

        private decimal _pennies, _nickels, _dimes, _quarters, _halfDollars, _dollarCoins, _dollars, _twoDollars, _fives, _tens, _twenties, _fifties, _hundreds;
        

        //Properties
        #region
        public decimal Pennies
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
        public decimal Nickels
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
        public decimal Dimes
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
        public decimal Quarters
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
        public decimal Halfdollars
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
        public decimal DollarCoins
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
        public decimal Dollars
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
        public decimal TwoDollars
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
        public decimal Fives
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
        public decimal Tens
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
        public decimal Twenties
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
        public decimal Fifties
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
        public decimal Hundreds
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

            cashInDrawer[0] = _hundreds;
            cashInDrawer[1] = _fifties;
            cashInDrawer[2] = _twenties;
            cashInDrawer[3] = _tens;
            cashInDrawer[4] = _fives;
            cashInDrawer[5] = _twoDollars;
            cashInDrawer[6] = _dollars;
            cashInDrawer[7] = _dollarCoins;
            cashInDrawer[8] = _halfDollars;
            cashInDrawer[9] = _quarters;
            cashInDrawer[10] = _dimes;
            cashInDrawer[11] = _nickels;
            cashInDrawer[12] = _pennies;

        }

        public decimal GetTotalCashInDrawer()
        {
            decimal total =0M;
            foreach (var item in cashInDrawer)
            {
                total += item;
            }
            return total;
        }
    }
}
        
