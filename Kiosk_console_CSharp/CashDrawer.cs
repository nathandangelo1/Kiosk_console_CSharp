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
        
