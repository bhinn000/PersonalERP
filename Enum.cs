namespace PersonalERP
{
    public class Enum
    {
        public enum PaymentMethodType
        {
            Bank = 1,
            Cash,
            OnlinePayment
        }

        public enum PaymentTypeMaybeCredit
        {
            Bank = 1,
            Cash = 2,
            Card = 3,
            Online = 4,
            Credit = 5,
        }

        public enum HowPaid
        {
            Credit = 1,
            FullPaid = 2,
        }

    }
}
