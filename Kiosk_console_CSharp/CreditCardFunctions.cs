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
            return new Regex(@"^4[0-9]{6,}$").IsMatch(cardNumber) ? CreditCardType.Visa :
                   new Regex(@"^5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}$").IsMatch(cardNumber) ? CreditCardType.MasterCard :
                   new Regex(@"^3[47][0-9]{5,}$").IsMatch(cardNumber) ? CreditCardType.AmericanExpress :
                   new Regex(@"^65[4-9][0-9]{13}|64[4-9][0-9]{13}|6011[0-9]{12}|(622(?:12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10})$").IsMatch(cardNumber) ? CreditCardType.Discover :
                   new Regex(@"^3[47][0-9]{13}$").IsMatch(cardNumber) ? CreditCardType.Amex :
                   new Regex(@"^(6541|6556)[0-9]{12}$").IsMatch(cardNumber) ? CreditCardType.BCGlobal :
                   new Regex(@"^389[0-9]{11}$").IsMatch(cardNumber) ? CreditCardType.CarteBlanch :
                   new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{11}$").IsMatch(cardNumber) ? CreditCardType.DinersClub :
                   new Regex(@"^63[7-9][0-9]{13}$").IsMatch(cardNumber) ? CreditCardType.InstaPaymentCard :
                   new Regex(@"^(?:2131|1800|35\d{3})\d{11}$").IsMatch(cardNumber) ? CreditCardType.JCBCard :
                   new Regex(@"^9[0-9]{15}$").IsMatch(cardNumber) ? CreditCardType.KoreanLocal :
                   new Regex(@"^(6304|6706|6709|6771)[0-9]{12,15}$").IsMatch(cardNumber) ? CreditCardType.LaserCard :
                   new Regex(@"^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$").IsMatch(cardNumber) ? CreditCardType.Maestro :
                   new Regex(@"^(6334|6767)[0-9]{12}|(6334|6767)[0-9]{14}|(6334|6767)[0-9]{15}$").IsMatch(cardNumber) ? CreditCardType.Solo :
                   new Regex(@"^(4903|4905|4911|4936|6333|6759)[0-9]{12}|(4903|4905|4911|4936|6333|6759)[0-9]{14}|(4903|4905|4911|4936|6333|6759)[0-9]{15}|564182[0-9]{10}|564182[0-9]{12}|564182[0-9]{13}|633110[0-9]{10}|633110[0-9]{12}|633110[0-9]{13}$").IsMatch(cardNumber) ? CreditCardType.SwitchCard :
                   new Regex(@"^(62[0-9]{14,17})$").IsMatch(cardNumber) ? CreditCardType.UnionPay :
                   cardNumber.Where((e) => e >= '0' && e <= '9').Reverse().Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2)).Sum((e) => e / 10 + e % 10) == 0 ? CreditCardType.NotFormatted :
                   CreditCardType.Unknown;
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
            Visa = 0,
            MasterCard = 1,
            AmericanExpress = 2,
            Discover = 3,
            Amex = 4,
            BCGlobal = 5,
            CarteBlanch = 6,
            DinersClub = 7,
            InstaPaymentCard = 8,
            JCBCard = 9,
            KoreanLocal = 10,
            LaserCard = 11,
            Maestro = 12,
            Solo = 13,
            SwitchCard = 14,
            UnionPay = 15,
            NotFormatted = 16,
            Unknown = 17
        }
    }
}
        
