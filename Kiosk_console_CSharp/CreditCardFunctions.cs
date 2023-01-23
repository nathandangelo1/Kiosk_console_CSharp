using System.Text.RegularExpressions;

namespace Kiosk_Console_CSharp;
public class CreditCardFunctions
{
    // CLASS CODE PROVIDED- NOT WRITTEN

    public static string[] MoneyRequest(string account_number, decimal amount)
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
            }
        }
    }

    // LUHN ALGORITHM TO CHECK CC NUMBER
    internal static bool IsValid(string ccValue)
    {
        if (ccValue is null)
        {
            return false;
        }

        ccValue = ccValue.Replace("-", "");
        ccValue = ccValue.Replace(" ", "");

        int checksum = 0;
        bool evenDigit = false;

        foreach (char digit in ccValue.Reverse())  // EXAMPLE - DIGIT = 6
        {
            if (digit < '0' || digit > '9') // IF LESS THAN 0 OR GREATER THAN 9
            {
                return false;
            }

            int digitValue = (digit - '0') * (evenDigit ? 2 : 1); // EVERY SECOND NUMBER MULTIPLY BY 2. 6*2 = 12
            
            evenDigit = !evenDigit; // FLIP

            while (digitValue > 0) // LOOP1: 12 > 0   LOOP2:  1 > 0
            {
                checksum += digitValue % 10;  //L1: CHECKSUM + 2 (12 % 10)    L2: CHECKSUM + 1 (1 % 10)  6*2= 12= 1+2= 3
                                              
                digitValue /= 10; // L1: DIGITVALUE = 1 (12/10)     L2 DIGITVALUE = 0 (1/10)
            }
        }
        return checksum % 10 == 0;
    }

    // REGEX EXPRESSIONS TO MATCH CC TYPE

    /// <summary>
    /// getCardType()
    /// </summary>
    /// <returns>Matches a object reference to regex to bring back a card type, the validity of the card, or a default (Unknown)</returns>
    internal static CreditCardType FindType(string cardNumber)
    {
        Regex regAmex = new Regex("^3[47][0-9]{13}$");
        Regex regCarteBlanche = new Regex("^389[0-9]{11}$");
        Regex regDinersClub = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
        Regex regDiscover = new Regex("^65[4-9][0-9]{13}|64[4-9][0-9]{13}|6011[0-9]{12}|(622(?:12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10})$");
        Regex regJCB = new Regex(@"^(?:2131|1800|35\d{3})\d{11}$");
        Regex regMastercard = new Regex("^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$");
        Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
        Regex regVisaMasterCard = new Regex("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$");

        if (regAmex.IsMatch(cardNumber))
            return CreditCardType.AmericanExpress;
        else if (regCarteBlanche.IsMatch(cardNumber))
            return CreditCardType.CarteBlanche;
        else if (regDinersClub.IsMatch(cardNumber))
            return CreditCardType.DinersClub;
        else if (regDiscover.IsMatch(cardNumber))
            return CreditCardType.Discover;
        else if (regMastercard.IsMatch(cardNumber))
            return CreditCardType.Mastercard;
        else if (regVisa.IsMatch(cardNumber))
            return CreditCardType.Visa;
        else if (regVisaMasterCard.IsMatch(cardNumber))
            return CreditCardType.VisaMasterCard;
        else
            return CreditCardType.Invalid;
    }

    public enum CreditCardType
    {
        AmericanExpress,
        CarteBlanche,
        DinersClub,
        Discover,
        Mastercard,
        Visa,
        VisaMasterCard,
        Invalid
    }
}


