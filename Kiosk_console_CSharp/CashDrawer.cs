namespace Kiosk_Console_CSharp;

//public class TempIsZeroException : Exception
//{
//    public FractionalDenomException(string message) : base(message)
//    {
//    }
//}

public class CashDrawer
{
    public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarCoinDec = 1.00M, dollarDec = 1.00M, twoDollarDec = 2.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

    public readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, twoDollarDec, dollarDec, dollarCoinDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };

    private decimal[] cashInDrawer;

    private decimal _pennies, _nickels, _dimes, _quarters, _halfDollars, _dollarCoins, _dollars, _twoDollars, _fives, _tens, _twenties, _fifties, _hundreds;

    #region //PROPERTIES
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
            else
            {
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
            else
            {
                //throw new InvalidMarksException("Sorry, Marks must be less than 100");
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
    #endregion// //PROPERTIES

    //CLASS METHODS
    public decimal GetTotalCashInDrawer()
    {
        decimal total = 0M;
        foreach (var denom in cashInDrawer)
        {
            total += denom;
        }
        return total;
    }
    internal void DeductCashInDrawer(CashDrawer drawer, decimal dispensedAmount, int index)
    {
        drawer.cashInDrawer[index] -= dispensedAmount;
    }
    internal void AddCashInDrawer(CashDrawer drawer, decimal amount, int index)
    {
        drawer.cashInDrawer[index] += amount;
    }
    internal bool GetChangeCounts(int[] changeCounts, decimal changeAmount, CashDrawer drawer)
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

    // PARAMETERIZED CONSTRUCTORS
    public CashDrawer(decimal pennyTotal = 5.00M, decimal nickelTotal = 8.00M, decimal dimeTotal = 20.00M, decimal quarterTotal = 50.00M, decimal halfdollarTotal = 0.00M, decimal dollarCoinTotal = 0.00M, decimal dollarTotal = 500.00M, decimal twoDollarTotal = 0.00M, decimal fiveTotal = 500.00M, decimal tenTotal = 500.00M, decimal twentyTotal = 1000.00M, decimal fiftyTotal = 0.00M, decimal hundredTotal = 5000.00M)
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
    //OVERLOADED CONSTRUCTOR
    //public CashDrawer(Denom[] denoms)
    //{
    //    cashInDrawer = new decimal[13];

    //    Pennies = denoms[0].Total;
    //    Nickels = denoms[1].Total;
    //    Dimes = denoms[2].Total;
    //    Quarters = denoms[3].Total;
    //    Halfdollars = denoms[4].Total;
    //    DollarCoins = denoms[5].Total;
    //    Dollars = denoms[6].Total;
    //    TwoDollars = denoms[7].Total;
    //    Fives = denoms[8].Total;
    //    Tens = denoms[9].Total;
    //    Twenties = denoms[10].Total;
    //    Fifties = denoms[11].Total;
    //    Hundreds = denoms[12].Total;

    //    cashInDrawer[0] = Hundreds;
    //    cashInDrawer[1] = Fifties;
    //    cashInDrawer[2] = Twenties;
    //    cashInDrawer[3] = Tens;
    //    cashInDrawer[4] = Fives;
    //    cashInDrawer[5] = TwoDollars;
    //    cashInDrawer[6] = Dollars;
    //    cashInDrawer[7] = DollarCoins;
    //    cashInDrawer[8] = Halfdollars;
    //    cashInDrawer[9] = Quarters;
    //    cashInDrawer[10] = Dimes;
    //    cashInDrawer[11] = Nickels;
    //    cashInDrawer[12] = Pennies;

    //}


    





}

