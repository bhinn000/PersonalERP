namespace PersonalERP.Entity
{
    //OrderPayment 
    public class BillPaymentCredit : DateTimeEntity
    {
        public int Id { get; set; }
        //public int? MenuOrderId { get; set; }
        public decimal BillAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PaymentReceivable { get; set; }
        public decimal? CreditLimit { get; set; } // no need in this table

        //public decimal RemainingAmount { get; set; }
        public int? PaymentMethod { get; set; } // bank=>bank option (NICA , esewa) ; cash
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public bool IsInitialPayment { get; set; }
        public int CraftsOrderId { get; set; }
        public virtual CraftsOrder CraftsOrder { get; set; }
        public bool CompletelyPaid { get; set; }
        public virtual ICollection<PayingOffCredit>? PayingOffCredits { get; set; }
    
    }

}
