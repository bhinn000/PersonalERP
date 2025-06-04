namespace PersonalERP.DTO
{

    public class CreateCraftsOrderDto
    {
        //has to check if login
        public int ArtId { get; set; }
        public string OrderRef { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string PhoneNum { get; set; }
        public int? BillPaymentId { get; set; }
        public string? Description { get; set; }
        public decimal? InitialCreditLimit { get; set; }
    }

    public class CraftsOrderDTO
    {
        public int Id { get; set; }
        public string OrderRef { get; set; }
        public string ArtName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int ArtId { get; set; }
        public string ArtTitle { get; set; }
    }


}

