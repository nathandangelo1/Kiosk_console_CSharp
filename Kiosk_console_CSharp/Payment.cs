namespace Kiosk_Console_CSharp
{
    public class Payment
    {
        public System.Guid transactionNumber;
        public DateTime datetime;

        public decimal cashAmount;

        public PaymentType paymentType;
        public bool success;

        public CreditCardFunctions.CreditCardType? ccVendor;
        public decimal ccAmount;
        public bool declined;
        public decimal declinedAmount;
        public bool IsPartialPayment;

        public Payment(Guid atransactionNumber, PaymentType apaymentType)
        {
            transactionNumber = atransactionNumber;
            datetime = DateTime.Now;
            paymentType = apaymentType;
        }

    }

}

