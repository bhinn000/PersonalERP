namespace PersonalERP.DTO
{
    public class PayingOffDTO
    {
        public int Id { get; set; }
        public int PaymentMethod { get; set; }
        //public decimal TotalAmount { get; set; } //take from BP
        public decimal TotalBillPaid { get; set; }
        //public decimal TotalBillRemaining { get; set; }
        public int CustomerId { get; set; }
        public int? BankId { get; set; }
        public int BPId { get; set; }
    }

    public class PayingOffResponseDTO
    {
        public PayingOffDTO Data { get; set; } = null!;
        public string? Message { get; set; }
    }

}
