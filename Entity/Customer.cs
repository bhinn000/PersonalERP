using PersonalERP.Entity;

namespace PersonalERP.Entity
{
    public class Customer : DateTimeEntity
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNum { get; set; } = null!;

        public decimal TotalBillAmount { get; set; }
        public decimal? TotalBillPaid { get; set; }
        public decimal TotalBillPayable { get; set; }

        public virtual ICollection<BillPaymentCredit> BillPaymentCredits { get; set; }
        //public virtual ICollection<PayingOffCredit> PayingOffCredits { get; set; }
        public decimal InitialCreditLimit { get; set; }
        public decimal? CurrentCreditLimit { get; set; }

        public virtual ICollection<CraftsOrder>? CraftsOrders{ get; set; }

    }
}

