using System.Linq.Expressions;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Kiosk_Console_CSharp
{
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
                }//end if
            }//end if
        }//end if

        // LUHN ALGORITHM TO CHECK CC NUMBER
        public static bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string? ccValue = value as string;

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

        // REGEX EXPRESSIONS TO MATCH CC TYPE
        //public enum CardType
        //{
        //    MasterCard, Visa, AmericanExpress, Discover, JCB
        //};

        //public static CardType FindType(string cardNumber)
        //{
        //    //https://www.regular-expressions.info/creditcard.html
        //    if (Regex.Match(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
        //    {
        //        return CardType.Visa;
        //    }

        //    if (Regex.Match(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
        //    {
        //        return CardType.MasterCard;
        //    }

        //    if (Regex.Match(cardNumber, @"^3[47][0-9]{13}$").Success)
        //    {
        //        return CardType.AmericanExpress;
        //    }

        //    if (Regex.Match(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success)
        //    {
        //        return CardType.Discover;
        //    }

        //    if (Regex.Match(cardNumber, @"^(?:2131|1800|35\d{3})\d{11}$").Success)
        //    {
        //        return CardType.JCB;
        //    }

        //    throw new Exception("Unknown card.");
        //}



    //public string cardNumber { get; set; }

        /// <summary>
        /// getCardType()
        /// </summary>
        /// <returns>Matches a object reference to regex to bring back a card type, the validity of the card, or a default (Unknown)</returns>
    public static CreditCardType FindType(string cardNumber)
    {
        Regex regAmex = new Regex("^3[47][0-9]{13}$");
        Regex regBCGlobal = new Regex("^(6541|6556)[0-9]{12}$");
        Regex regCarteBlanche = new Regex("^389[0-9]{11}$");
        Regex regDinersClub = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
        Regex regDiscover = new Regex("^65[4-9][0-9]{13}|64[4-9][0-9]{13}|6011[0-9]{12}|(622(?:12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10})$");
        Regex regInstaPayment = new Regex("^63[7-9][0-9]{13}$");
        Regex regJCB = new Regex(@"^(?:2131|1800|35\d{3})\d{11}$");
        Regex regKoreanLocal = new Regex("^9[0-9]{15}$");
        Regex regLaser = new Regex("^(6304|6706|6709|6771)[0-9]{12,15}$");
        Regex regMaestro = new Regex("^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$");
        Regex regMastercard = new Regex("^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$");
        Regex regSolo = new Regex("^(6334|6767)[0-9]{12}|(6334|6767)[0-9]{14}|(6334|6767)[0-9]{15}$");
        Regex regSwitch = new Regex("^(4903|4905|4911|4936|6333|6759)[0-9]{12}|(4903|4905|4911|4936|6333|6759)[0-9]{14}|(4903|4905|4911|4936|6333|6759)[0-9]{15}|564182[0-9]{10}|564182[0-9]{12}|564182[0-9]{13}|633110[0-9]{10}|633110[0-9]{12}|633110[0-9]{13}$");
        Regex regUnionPay = new Regex("^(62[0-9]{14,17})$");
        Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
        Regex regVisaMasterCard = new Regex("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$");

        if (regAmex.IsMatch(cardNumber))
            return CreditCardType.AmericanExpress;
        else if (regBCGlobal.IsMatch(cardNumber))
            return CreditCardType.BCGlobal;
        else if (regCarteBlanche.IsMatch(cardNumber))
            return CreditCardType.CarteBlanche;
        else if (regDinersClub.IsMatch(cardNumber))
            return CreditCardType.DinersClub;
        else if (regDiscover.IsMatch(cardNumber))
            return CreditCardType.Discover;
        else if (regInstaPayment.IsMatch(cardNumber))
            return CreditCardType.InstaPayment;
        else if (regJCB.IsMatch(cardNumber))
            return CreditCardType.JCB;
        else if (regKoreanLocal.IsMatch(cardNumber))
            return CreditCardType.KoreanLocal;
        else if (regLaser.IsMatch(cardNumber))
            return CreditCardType.Laser;
        else if (regMaestro.IsMatch(cardNumber))
            return CreditCardType.Maestro;
        else if (regMastercard.IsMatch(cardNumber))
            return CreditCardType.Mastercard;
        else if (regSolo.IsMatch(cardNumber))
            return CreditCardType.Solo;
        else if (regSwitch.IsMatch(cardNumber))
            return CreditCardType.Switch;
        else if (regUnionPay.IsMatch(cardNumber))
            return CreditCardType.UnionPay;
        else if (regVisa.IsMatch(cardNumber))
            return CreditCardType.Visa;
        else if (regVisaMasterCard.IsMatch(cardNumber))
            return CreditCardType.VisaMasterCard;
        else
            return CreditCardType.Invalid;
    }

        /// <summary>
        /// isCreditCardAccepted()
        /// </summary>
        /// <returns>Checks to see if the credit card is allowed by comparing it to the integer value of CreditCardType to a local array of allowed integers</returns>
        //public bool isCreditCardAccepted()
        //{
        //    // This should honestly be internalized somewhere for security reasons
        //    int[] allowed = new int[] { 0, 1, 2, 3 };
        //    return Array.IndexOf(allowed, FindType()) >= 0;
        //}

        public enum CreditCardType
        {
            AmericanExpress,
            BCGlobal,
            CarteBlanche,
            DinersClub,
            Discover,
            InstaPayment,
            JCB,
            KoreanLocal,
            Laser,
            Maestro,
            Mastercard,
            Solo,
            Switch,
            UnionPay,
            Visa,
            VisaMasterCard,


            Invalid
        }
    }
}
        
