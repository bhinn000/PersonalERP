using System.ComponentModel.DataAnnotations;

namespace PersonalERP.Entity
{
    public class CraftsOrder : DateTimeEntity
    {
        //MenuOrder
        //public CraftsOrder()
        //{
        //    BillPaymentCredit = new HashSet<BillPaymentCredit>();
        //}
        public int Id { get; set; }
        public string OrderRef { get; set; } = null!;
        public string ArtName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int ArtId { get; set; }
        public virtual ArtPiece Art { get; set; }
        //public virtual ICollection<BillPaymentCredit> BillPaymentCredit { get; set; }
        public int? BillPaymentId { get;set; }
        public virtual BillPaymentCredit? BillPaymentCredit { get; set; }


    }
}


//    else//new customer
//{
//    if (dto.CreditLimit <= 0.0m || !dto.CreditLimit.HasValue)
//    {
//        throw new Exception("What is the credit limit for this customer?");
//    }

//    var billCredits = new List<BillPaymentCredit>
//                        {
//                            CustomerOnCreditHelper.CreateBillPaymentCredit_NewCustomer(dto, organization.Id, 0, _accessor)
//                        };

//    var payingCredits = dto.PaidAmount != null
//        ? new List<PayingOffCredit> { CustomerOnCreditHelper.CreatePayingOffCredit_NewCustomer(dto, 0, organization.Id, _accessor) }
//        : new List<PayingOffCredit>();

//    var newCustomerOnCredit = CustomerOnCreditHelper.CreateNewCustomer(dto, organization.Id, _accessor, billCredits, payingCredits);

//    newCustomerOnCredit.InitialCreditLimit = dto.CreditLimit ?? 0;
//    newCustomerOnCredit.CurrentCreditLimit = newCustomerOnCredit.InitialCreditLimit;

//    if (newCustomerOnCredit.CurrentCreditLimit.Value < paymentOnCredit)
//    {
//        throw new Exception("Customer does not have enough credit limit to cover the remaining bill.");
//    }
//    else
//    {
//        newCustomerOnCredit.CurrentCreditLimit -= paymentOnCredit;
//    }

//    _customerV1Repo.Insert(newCustomerOnCredit);
//}