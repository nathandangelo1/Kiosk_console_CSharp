
namespace Kiosk_Console_CSharp;

public class CardPayment : Payment
{
    internal CreditCardFunctions.CreditCardType? ccVendor;
    internal decimal ccAmount;
    internal bool declined;
    internal decimal declinedAmount;
    internal bool IsPartialPayment;
    public CardPayment(Guid atransactionNumber, PaymentType apaymentType)
    {
        transactionNumber = atransactionNumber;
        datetime = DateTime.Now;
        paymentType = apaymentType;
    }
}


