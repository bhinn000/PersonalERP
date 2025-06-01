//namespace PersonalERP.DTO
//{
//    //public partial class MenuOrderDetailCreateDto
//    //{
//    //    public int MenuItemId { get; set; }
//    //    public int Quantity { get; set; }
//    //    public string? Notes { get; set; }
//    //}

//    //public class MenuOrderDetailUpdateDto : MenuOrderDetailCreateDto
//    //{
//    //    public int MenuOrderDetailId { get; set; }
//    //}

//    //public partial class MenuOrderDetailDto
//    //{
//    //    public int MenuOrderDetailId { get; set; }
//    //    public int Quantity { get; set; }
//    //    public int MenuItemId { get; set; }
//    //    public bool SendForPrint { get; set; }
//    //    public MenuItemDetailDto MenuItemDetail { get; set; }
//    //    public double Price { get; set; }
//    //    public string? Note { get; set; }
//    //    //public DiscountDetailDto? DiscountDetail { get; set; }
//    //}

//    //public partial class MenuOrderStatusLogDto
//    //{
//    //    public int Id { get; set; }
//    //    public string PreviousStatus { get; set; }
//    //    public string CurrentStatus { get; set; }
//    //    public DateTime CreatedDate { get; set; }
//    //    public string? CreatedBy { get; set; }
//    //    public string? Remarks { get; set; }
//    //}

//    //public partial class MenuBillingStatusLogDto
//    //{
//    //    public int Id { get; set; }
//    //    public string PreviousStatus { get; set; }
//    //    public string CurrentStatus { get; set; }
//    //    public DateTime CreatedDate { get; set; }
//    //    public string? CreatedBy { get; set; }
//    //    public string? Remarks { get; set; }
//    //}

//    //public class MenuOrderDto
//    //{
//    //    public int Id { get; set; }
//    //    public string OrderRef { get; set; }
//    //    public double? TotalAmount { get; set; }
//    //    public double? DiscountAmount { get; set; }
//    //    public double FinalAmount { get; set; }
//    //    public double? TaxAmount { get; set; }
//    //    public double? TaxPercentage { get; set; }
//    //    public double? ServiceCharge { get; set; }
//    //    public double? ServiceChargePercentage { get; set; }
//    //    public string? OrderStatus { get; set; }
//    //    public string OrderType { get; set; } = null!;
//    //    public string? BillingStatus { get; set; }
//    //    public DateTime CreatedDate { get; set; }
//    //    public string? CustomerContact { get; set; }
//    //    public int ItemCount => menuOrderDetails.Count;
//    //    public IList<MenuOrderDetailDto> menuOrderDetails { get; set; } = new List<MenuOrderDetailDto>();
//    //    public IList<MenuOrderStatusLogDto>? statusLogDtos { get; set; } = new List<MenuOrderStatusLogDto>();
//    //    //public IList<MenuBillingStatusLogDto>? billingStatusLogDtos { get; set; } = new List<MenuBillingStatusLogDto>();
//    //    public MenuBillingStatusLogDto? billingStatusLogDtos { get; set; } = new MenuBillingStatusLogDto();
//    //    public ViewTableData? TableData { get; set; }
//    //    public IList<CustomerDataDto>? CustomerData { get; set; }
//    //    public string? Notes { get; set; }
//    //}

//    //public partial class CreateMenuOrderDto
//    //{
//    //    public string? PhoneNumber { get; set; }
//    //    public string? Email { get; set; }
//    //    public string OTP { get; set; }
//    //    public IList<MenuOrderDetailCreateDto> Items { get; set; } = new List<MenuOrderDetailCreateDto>();
//    //}

//    public class CreateCraftsOrderDto
//    {
//        //has to check if login
//        public int ArtId { get; set; }
//        public string OrderRef { get; set; } = null!;
//        public string? CustomerId { get; set; }
//        public string? CustomerName { get; set; }
//        public string? Address { get; set; }
//        public string? PhoneNum { get; set; }
//    }

//    //public partial class CreateBillingMenuOrderDto
//    //{
//    //    public int? TableId { get; set; }
//    //    public CustomerDataDto? Customer { get; set; }
//    //    public IList<MenuOrderDetailCreateDto> Items { get; set; } = new List<MenuOrderDetailCreateDto>();
//    //}

//    public class CustomerDataDto
//    {
//        public int Id { get; set; }
//        public string Name { get; set; } = null!;
//        public string? PhoneNumber { get; set; }
//        public string? Address { get; set; }
//        //public string? Email { get; set; }
//    }


//    //public class ChargeData
//    //{
//    //    public decimal VAT { get; set; }
//    //    public decimal ServiceCharge { get; set; }
//    //}

//    //public partial class UpdateMenuOrderDto
//    //{
//    //    public IList<MenuOrderDetailUpdateDto> Items { get; set; } = new List<MenuOrderDetailUpdateDto>();
//    //}

//    //public class PaymentResultDto
//    //{
//    //    public int OrderId { get; set; }
//    //    public double PaidAmount { get; set; }
//    //}


//    //public class OrderItemWiseView
//    //{
//    //    public IList<ViewMenuOrderDetailDto>? KOT { get; set; }
//    //    public IList<ViewMenuOrderDetailDto>? BOT { get; set; }

//    //}

//    //public class ViewMenuOrderDetailDto
//    //{
//    //    public int Id { get; set; }
//    //    public string ItemName { get; set; }
//    //    public string TableName { get; set; }
//    //    public EnumOrderDetailType Type { get; set; } // "KOT" or "BOT"
//    //    public int Quantity { get; set; }
//    //    public string Unit { get; set; }
//    //    public bool Printed { get; set; }
//    //    public bool SendForPrint { get; set; }
//    //}

//    //public class GroupedMenuOrderDetailDto
//    //{
//    //    public string TableName { get; set; } = null!;
//    //    public int KotNumber { get; set; }
//    //    public List<ViewMenuOrderDetailDto> Data { get; set; } = new List<ViewMenuOrderDetailDto>();
//    //}



//}
