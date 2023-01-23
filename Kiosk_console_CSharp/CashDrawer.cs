namespace Kiosk_Console_CSharp;


public enum DenomType
{
    Hundreds = 0,
    Fifties = 1,
    Twenties = 2,
    Tens = 3,
    Fives = 4,
    Twos = 5,
    Dollars = 6,
    DollarCoin = 7,
    HalfDollar = 8,
    Quarters = 9,
    Dimes = 10,
    Nickels = 11,
    Pennies = 12
}

public struct Denom
{
    public DenomType type;
    public decimal total;
}

public class CashDrawer
{

    public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfdollarDec = .50M, dollarCoinDec = 1.00M, dollarDec = 1.00M, twoDollarDec = 2.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;

    public static readonly decimal[] values = { hundredDec, fiftyDec, twentyDec, tenDec, fiveDec, twoDollarDec, dollarDec, dollarCoinDec, halfdollarDec, quarterDec, dimeDec, nickelDec, pennyDec };

    
    public static Denom[] cashInDrawer;

    private static Denom _pennies , _nickels, _dimes, _quarters, _halfDollars, _dollarCoins, _dollars, _twoDollars, _fives, _tens, _twenties, _fifties, _hundreds;

    // PARAMETERIZED CONSTRUCTORS
    public CashDrawer(decimal pennyTotal = 5.00M, decimal nickelTotal = 8.00M, decimal dimeTotal = 20.00M, decimal quarterTotal = 50.00M, decimal halfdollarTotal = 0.00M, decimal dollarCoinTotal = 0.00M, decimal dollarTotal = 500.00M, decimal twoDollarTotal = 0.00M, decimal fiveTotal = 500.00M, decimal tenTotal = 500.00M, decimal twentyTotal = 1000.00M, decimal fiftyTotal = 0.00M, decimal hundredTotal = 5000.00M)
    {

        //_pennies.total = pennyTotal;
        //_nickels.total = nickelTotal;
        //_dimes.total = dimeTotal;
        //_quarters.total = quarterTotal;
        //_halfDollars.total = halfdollarTotal;
        //_dollarCoins.total = dollarCoinTotal;
        //_dollars.total = dollarTotal;
        //_twoDollars.total = twoDollarTotal;
        //_fives.total = fiveTotal;
        //_tens.total = tenTotal;
        //_twenties.total = twentyTotal;
        //_fifties.total = fiftyTotal;
        //_hundreds.total = hundredTotal;

        cashInDrawer = new Denom[13];

        cashInDrawer[0].type = DenomType.Hundreds;
        cashInDrawer[0].total = hundredTotal;

        cashInDrawer[1].type = DenomType.Fifties;
        cashInDrawer[1].total = fiftyTotal;

        cashInDrawer[2].type = DenomType.Twenties;
        cashInDrawer[2].total = twentyTotal;

        cashInDrawer[3].type = DenomType.Tens;
        cashInDrawer[3].total = tenTotal;

        cashInDrawer[4].type = DenomType.Fives;
        cashInDrawer[4].total = fiveTotal;

        cashInDrawer[5].type = DenomType.Twos;
        cashInDrawer[5].total = twoDollarTotal;

        cashInDrawer[6].type = DenomType.Dollars;
        cashInDrawer[6].total = dollarTotal;

        cashInDrawer[7].type = DenomType.DollarCoin;
        cashInDrawer[7].total = dollarCoinTotal;

        cashInDrawer[8].type = DenomType.HalfDollar;
        cashInDrawer[8].total = halfdollarTotal;

        cashInDrawer[9].type = DenomType.Quarters;
        cashInDrawer[9].total = quarterTotal;

        cashInDrawer[10].type = DenomType.Dimes;
        cashInDrawer[10].total = dimeTotal;

        cashInDrawer[11].type = DenomType.Nickels;
        cashInDrawer[11].total = nickelTotal;

        cashInDrawer[12].type = DenomType.Pennies;
        cashInDrawer[12].total = pennyTotal;

    }

    //CLASS METHODS
    internal static decimal GetTotalCashInDrawer()
    {
        decimal total = 0M;
        foreach (var denom in cashInDrawer)
        {
            total += denom.total;
        }
        return total;
    }
    internal static void DeductFromCashInDrawer(decimal dispensedAmount, int index)
    {
        cashInDrawer[index].total -= dispensedAmount;
    }
    internal static void AddCashToDrawer(decimal amount, int index)
    {
        cashInDrawer[index].total += amount;
    }
    internal static bool GetChangeCounts(int[] changeCounts, decimal changeAmount)
    {
        int count;
        // FOR EACH INDEX IN VALUES[]
        for (int i = 0; i < values.Length; i++)
        {
            //IF QUOTIENT OF CHANGEAMOUNT/VALUES[I] IS GREATER THAN OR EQUAL TO 1    
            if (changeAmount / values[i] >= 1)
            {
                //INTEGER DIVISION GIVING THE NUMBER OF TIMES NUM CAN BE DIVIDED BY VALUE
                count = (int)(changeAmount / values[i]);

                //IF THERE IS ENOUGH OF DENOMINATION TO MAKE CHANGE
                if (cashInDrawer[i].total >= count * values[i])
                {
                    //REDUCE NUM BY (VALUE*TEMP) (EXAMPLE: 1199->199)
                    changeAmount %= values[i];
                    //INCREMENT CHANGECOUNTS
                    changeCounts[i] += count;
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

    //#region //PROPERTIES
    //public static Denom Pennies
    //{
    //    get
    //    {
    //        return _pennies;
    //    }
    //    set
    //    {
    //        if (value.type == DenomType.Pennies && value.total >= 0 && value.total % pennyDec == 0)
    //        {
    //            _pennies = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % pennyDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Pennies");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //public static Denom Nickels
    //{
    //    get
    //    {
    //        return _nickels;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % nickelDec == 0)
    //        {
    //            _nickels = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % nickelDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Nickels");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //public static Denom Dimes
    //{
    //    get
    //    {
    //        return _dimes;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % dimeDec == 0)
    //        {
    //            _dimes = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % dimeDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Dimes");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Quarters
    //{
    //    get
    //    {
    //        return _quarters;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % quarterDec == 0)
    //        {
    //            _quarters = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % quarterDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Quarters");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Halfdollars
    //{
    //    get
    //    {
    //        return _halfDollars;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % halfdollarDec == 0)
    //        {
    //            _halfDollars = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % halfdollarDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: HalfDollars");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom DollarCoins
    //{
    //    get
    //    {
    //        return _dollarCoins;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % dollarCoinDec == 0)
    //        {
    //            _dollarCoins = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % dollarCoinDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: DollarCoins");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Dollars
    //{
    //    get
    //    {
    //        return _dollars;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % dollarDec == 0)
    //        {
    //            _dollars = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % dollarDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Dollars");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom TwoDollars
    //{
    //    get
    //    {
    //        return _twoDollars;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % twoDollarDec == 0)
    //        {
    //            _twoDollars = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % twoDollarDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: TwoDollars");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Fives
    //{
    //    get
    //    {
    //        return _fives;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % fiveDec == 0)
    //        {
    //            _fives = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % fiveDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Fives");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Tens
    //{
    //    get
    //    {
    //        return _tens;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % tenDec == 0)
    //        {
    //            _tens = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero");
    //        }
    //        else if (value.total % tenDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Tens");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Twenties
    //{
    //    get
    //    {
    //        return _twenties;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % twentyDec == 0)
    //        {
    //            _twenties = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % twentyDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Twenties");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Fifties
    //{
    //    get
    //    {
    //        return _fifties;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % fiftyDec == 0)
    //        {
    //            _fifties = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % fiftyDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Fifties");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //internal static Denom Hundreds
    //{
    //    get
    //    {
    //        return _hundreds;
    //    }
    //    set
    //    {
    //        if (value.total >= 0 && value.total % hundredDec == 0)
    //        {
    //            _hundreds = value;
    //        }
    //        else if (value.total < 0)
    //        {
    //            throw new ArgumentException("Not greater than zero ");
    //        }
    //        else if (value.total % hundredDec != 0)
    //        {
    //            throw new ArgumentException("Fractional values: Hundreds");
    //        }
    //        else
    //        {
    //            throw new ArgumentException("Error setting Drawer values.");
    //        }
    //    }
    //}
    //#endregion// //PROPERTIES


}

