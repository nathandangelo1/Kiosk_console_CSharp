using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Kiosk_Console_CSharp
{

    class Program
    {
        public class CashDrawer
        {
            public decimal[] cashInDrawer;
            
            private int _pennyCount, _nickelCount, _dimeCount, _quarterCount, _halfCount, _dollarCount, _fiveCount, _tenCount, _twentyCount, _fiftyCount, _hundredCount;

            public int PennyCount { get { return _pennyCount; } set { if (value > 0) { _pennyCount = value; } } }
            public int NickelCount { get { return _nickelCount; } set { if (value > 0) { _nickelCount = value; } } }
            public int DimeCount { get { return _dimeCount; } set { if (value > 0) { _dimeCount = value; } } }
            public int QuarterCount { get { return _quarterCount; } set { if (value > 0) { _quarterCount = value; } } }
            public int HalfCount { get { return _halfCount; } set { if (value > 0) { _halfCount = value; } } }
            public int DollarCount { get { return _dollarCount; } set { if (value > 0) { _dollarCount = value; } } }
            public int FiveCount { get { return _fiveCount; } set { if (value > 0) { _fiveCount = value; } } }
            public int TenCount { get { return _tenCount; } set { if (value > 0) { _tenCount = value; } } }
            public int TwentyCount { get { return _twentyCount; } set { if (value > 0) { _twentyCount = value; } } }
            public int FiftyCount { get { return _fiftyCount; } set { if (value > 0) { _fiftyCount = value; } } }
            public int HundredCount { get { return _hundredCount; } set { if (value > 0) { _hundredCount = value; } } }

            public const decimal pennyDec = .01M, nickelDec = .05M, dimeDec = .10M, quarterDec = .25M, halfDec = .50M, dollarDec = 1.00M, fiveDec = 5.00M, tenDec = 10.00M, twentyDec = 20.00M, fiftyDec = 50.00M, hundredDec = 100.00M;
            public const int pennyInt = 1, nickelInt = 5,dimeInt = 10,quarterInt = 25,halfInt = 50,dollarInt = 100,fiveInt = 500,tenInt = 1000, twentyInt = 2000, fiftyInt = 5000, hundredInt = 10000;

            public void FillCashDrawer(int apennyCount=100, int anickelCount=80, int adimeCount=100, int aquarterCount=80, int ahalfDollarCount=0, int adollarCount=100, int afiveCount=20, int atenCount=25, int atwentyCount=25, int afiftyCount=0, int ahundredCount=5)
            {
                _pennyCount = apennyCount;
                _nickelCount = anickelCount;
                _dimeCount = adimeCount;
                _quarterCount = aquarterCount;
                _halfCount = ahalfDollarCount;
                _dollarCount = adollarCount;
                _fiveCount = afiveCount;
                _tenCount = atenCount;
                _twentyCount = atwentyCount;
                _fiftyCount = afiftyCount;
                _hundredCount = ahundredCount;
            }

        }

        static void Main(string[] args)
        {
            CashDrawer drawer1 = new CashDrawer();

            drawer1.FillCashDrawer();

            drawer1.cashInDrawer = new decimal[11];

            PopulateCashInDrawerArray(drawer1);

            decimal drawer1total = GetDrawerTotal(drawer1);
            Console.WriteLine(drawer1total);

            decimal saleTotal = 152.96M;

            convertToRoman(drawer1, saleTotal);

            foreach (var item in drawer1.)
            {

            }

        }
        static void convertToRoman(CashDrawer drawer, decimal saleTotal)
        {
            decimal[] values = {CashDrawer.hundredDec, CashDrawer.fiftyDec, CashDrawer.twentyDec, CashDrawer.tenDec, CashDrawer.fiveDec, CashDrawer.dollarDec, CashDrawer.halfDec,CashDrawer.quarterDec,CashDrawer.dimeDec,CashDrawer.nickelDec,CashDrawer.pennyDec };
            
            //string[] letters = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            //string roman = "";
            int temp = 0;

            //for each index in values[]
            for (int i = 0; i < values.Length; i++)
            {

                //if quotient of num/value is greater than or equal to 1 
                if ((saleTotal / values[i]) >= 1)
                {

                    //integer division giving the number of times num can be divided by value
                    temp = (int)(saleTotal / values[i]);

                    //reduce num by (value*temp) (example: 1199 ->199
                    saleTotal %= values[i];
                }
                //for temp times
                for (int j = 0; j < temp; j++)
                {
                    //remove/////WORK HERE GETTING CHANGE NOT SUBTRACTING- WORK ON IT
                    drawer.cashInDrawer[i] -= temp;
                }
                //reset temp
                temp = 0;
            }
            //Console.WriteLine("");
            //return roman;

        }//end main
        static void PopulateCashInDrawerArray(CashDrawer drawer)
        {
            drawer.cashInDrawer[0] = drawer.HundredCount * CashDrawer.hundredDec;
            drawer.cashInDrawer[1] = drawer.FiftyCount * CashDrawer.fiftyDec;
            drawer.cashInDrawer[2] = drawer.TwentyCount * CashDrawer.twentyDec;
            drawer.cashInDrawer[3] = drawer.TenCount * CashDrawer.tenDec;
            drawer.cashInDrawer[4] = drawer.FiveCount * CashDrawer.fiveDec;
            drawer.cashInDrawer[5] = drawer.DollarCount * CashDrawer.dollarDec;
            drawer.cashInDrawer[6] = drawer.HalfCount * CashDrawer.halfDec;
            drawer.cashInDrawer[7] = drawer.QuarterCount * CashDrawer.quarterDec;
            drawer.cashInDrawer[8] = drawer.DimeCount * CashDrawer.dimeDec;
            drawer.cashInDrawer[9] = drawer.NickelCount * CashDrawer.nickelDec;
            drawer.cashInDrawer[10] = drawer.PennyCount * CashDrawer.pennyDec;
        }
        static void BackFillCashInDrawerFromArray(CashDrawer drawer)
        {
            drawer.HundredCount = (int)drawer.cashInDrawer[0] / CashDrawer.hundredInt;
            drawer.FiftyCount = (int)drawer.cashInDrawer[1] / CashDrawer.fiftyInt;
            drawer.cashInDrawer[2] = drawer.TwentyCount * CashDrawer.twentyDec;
            drawer.cashInDrawer[3] = drawer.TenCount * CashDrawer.tenDec;
            drawer.cashInDrawer[4] = drawer.FiveCount * CashDrawer.fiveDec;
            drawer.cashInDrawer[5] = drawer.DollarCount * CashDrawer.dollarDec;
            drawer.cashInDrawer[6] = drawer.HalfCount * CashDrawer.halfDec;
            drawer.cashInDrawer[7] = drawer.QuarterCount * CashDrawer.quarterDec;
            drawer.cashInDrawer[8] = drawer.DimeCount * CashDrawer.dimeDec;
            drawer.cashInDrawer[9] = drawer.NickelCount * CashDrawer.nickelDec;
            drawer.cashInDrawer[10] = drawer.PennyCount * CashDrawer.pennyDec;
        }
        static decimal GetDrawerTotal(CashDrawer drawer)
        {
            decimal bank = 0.00M;
            foreach (var item in drawer.cashInDrawer)
            {
                bank += item;
            }
            //Console.WriteLine(bank);
            return bank;
        }
    }
}