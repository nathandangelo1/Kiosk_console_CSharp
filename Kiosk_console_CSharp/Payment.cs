using System.Globalization;


namespace Kiosk_Console_CSharp;
public enum PaymentType
{
    Cash, Card
}
public class Payment
{
    public Guid transactionNumber;
    internal DateTime datetime;
    internal PaymentType paymentType;
    internal bool success;
    internal decimal cashAmount;

    internal CreditCardFunctions.CreditCardType? ccVendor;
    internal decimal ccAmount;
    internal bool declined;
    internal decimal declinedAmount;
    internal bool IsPartialPayment;

    public Payment(Guid atransactionNumber, PaymentType apaymentType)
    {
        transactionNumber = atransactionNumber;
        datetime = DateTime.Now;
        paymentType = apaymentType;
    }

}



