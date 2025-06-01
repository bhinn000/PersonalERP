//using System;

//namespace PersonalERP.DTO
//{
//    public class GetBillPaymentCredit_DTO
//    {
//        public int Id { get; set; }
//        public decimal Amount { get; set; }
//        public decimal? PaidAmount { get; set; }
//        public decimal? RemainingAmount { get; set; }
//        public int CustomerId { get; set; }
//        public string CustomerName { get; set; }
//        public int OrgId { get; set; }
//        public string OrganizationName { get; set; }
//        public int MenuOrderId { get; set; }
//        public int? PaymentMethod { get; set; }
//    }

//    public class CustomerOnCreditDTO
//    {
//        public string CustomerName { get; set; }
//        public string CustomerPhone { get; set; }
//        public string CustomerAddress { get; set; }
//        public string CustomerEmail { get; set; }
//        public decimal TotalBillAmount { get; set; }
//        public decimal TotalRemAmount { get; set; }
//        public List<CustomerPayingOffDTO>? TransactionHistory { get; set; }

//    }

//    public class CustomerBaseDTO
//    {
//        public string FirstName { get; set; } = null!;
//        public string? LastName { get; set; }
//        public string? Email { get; set; }
//        public string Address { get; set; } = null!;
//        public string PhoneNo { get; set; } = null!;
//        public string? PhoneNo2 { get; set; }
//    }

//    public class CreateBillPaymentCredit_DTO : CustomerBaseDTO
//    {
//        public int CraftsOrderId { get; set; }
//        public decimal TotalBillAmount { get; set; }
//        public decimal? PaidAmount { get; set; }
//        public int? PaymentMethod { get; set; }
//        public decimal? CreditLimit { get; set; }
//        public int? BankId { get; set; }

//    }

//    public class UpdateBillPaymentCredit_DTO
//    {
//        public int Id { get; set; }
//        public decimal Amount { get; set; }
//        public decimal? PaidAmount { get; set; }
//        public decimal? RemainingAmount { get; set; }
//        public int OrgId { get; set; }
//    }

//    public class PayingOffCreditDTO
//    {
//        public int Id { get; set; }
//        public decimal PayingAmount { get; set; }
//        public int PaymentMethod { get; set; }
//        public string PhoneNo { get; set; }
//        public int? BankId { get; set; }
//    }

//    public class CustomerPayingOffDTO
//    {
//        public DateTime CreatedDate { get; set; }
//        public string CreatedBy { get; set; }
//        public decimal? TransactionAmout { get; set; }
//        public decimal? RemainingAmount { get; set; }
//        public int PaymentMethod { get; set; }
//        public int? BankId { get; set; }
//    }



//}

