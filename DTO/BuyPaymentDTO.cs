namespace PersonalERP.DTO
{
    public class BuyPaymentDTO
    {
        public int CraftsOrderId { get; set; }
        public int PaymentMethod { get; set; } //cash or digital
        public int CustomerId { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal? PaymentReceivable { get; set; }
        public decimal? CreditLimit { get; set; }
        public int? BankId { get; set; }
        

    }
}
