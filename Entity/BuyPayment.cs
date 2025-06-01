namespace PersonalERP.Entity
{
    public class BuyPayment : DateTimeEntity
    {
        //if full payment done , then only this entity her will be enough otherwise , it have to go Billpayment and PayingOffCredit 
        public int Id { get; set; }
        public int CraftsOrderId {  get; set; }
        public virtual CraftsOrder CraftsOrder { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string PaymentMethod {  get; set; } // it cant be null , because if it is bought there is atleast one way for paying
        public bool CompletelyPaid {  get; set; }
        public decimal PaidAmount {  get; set; }
        public decimal? PaymentReceivable { get; set; }
        public int? BillPaymentCreditId { get; set; }
        public virtual BillPaymentCredit? BillPaymentCredits { get; set; }

        //one craftsOrder can have been paid in multiple time
    }
}
