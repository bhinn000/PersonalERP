using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP;


namespace PersonalERP.Helper
{
    public static class CustomerOnCreditHelper
    {

        public static BillPaymentCredit CreateBillPaymentCredit_ExistingCustomer(CreateBillPaymentCredit_DTO dto, int orgId, int customerId, IHttpContextAccessor accessor)
        {
            return new BillPaymentCredit
            {
                BillAmount = dto.TotalBillAmount,
                PaidAmount = dto.PaidAmount ?? 0,
                RemainingAmount = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
                PaymentMethod = dto.PaymentMethod,
                CreatedDate = GeneralUtility.GetCurrentDateTime(),
                CreatedBy = GeneralUtility.GetCurrentUserName(accessor),
                //OrgId = orgId,
                MenuOrderId = dto.MenuOrderId,
                CustomerId = customerId
            };
        }

        public static BillPaymentCredit CreateBillPaymentCredit_NewCustomer(CreateBillPaymentCredit_DTO dto, int orgId, int customerId, IHttpContextAccessor accessor)
        {
            return new BillPaymentCredit
            {
                MenuOrderId = dto.MenuOrderId,
                BillAmount = dto.TotalBillAmount,
                PaidAmount = dto.PaidAmount ?? 0,
                RemainingAmount = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
                PaymentMethod = dto.PaymentMethod,
                CreatedDate = GeneralUtility.GetCurrentDateTime(),
                CreatedBy = GeneralUtility.GetCurrentUserName(accessor),
                //OrgId = orgId,
                //CreditLimit=dto.CreditLimit
            };
        }

        public static PayingOffCredit CreatePayingOffCredit_ExistingCustomer(CreateBillPaymentCredit_DTO dto, int customerId, int orgId, IHttpContextAccessor accessor)
        {
            return new PayingOffCredit
            {
                CustomerId = customerId,
                PaymentMethod = (int)dto.PaymentMethod,
                TotalAmount = 0,
                TotalBillPaid = dto.PaidAmount ?? 0,
                TotalBillRemaining = 0,
                CreatedDate = GeneralUtility.GetCurrentDateTime(),
                CreatedBy = GeneralUtility.GetCurrentUserName(accessor),
                //OrgId = orgId,
                BankId = dto.BankId
            };
        }

        public static PayingOffCredit CreatePayingOffCredit_NewCustomer(CreateBillPaymentCredit_DTO dto, int customerId, int orgId, IHttpContextAccessor accessor)
        {
            return new PayingOffCredit
            {
                PaymentMethod = (int)dto.PaymentMethod,
                TotalBillPaid = dto.PaidAmount ?? 0,
                TotalAmount = 0,
                TotalBillRemaining = 0,
                CreatedDate = GeneralUtility.GetCurrentDateTime(),
                CreatedBy = GeneralUtility.GetCurrentUserName(accessor),
                //OrgId = orgId,
                BankId = dto.BankId
            };
        }

        public static Customer CreateNewCustomer(CreateBillPaymentCredit_DTO dto, int orgId, IHttpContextAccessor accessor, List<BillPaymentCredit> billPaymentCredits, List<PayingOffCredit> payingOffCredits)
        {
            return new Customer
            {
                UserId = null,
                Username = dto.PhoneNo,
                //firstName = dto.FirstName,
                //lastName = dto.LastName,
                //Email = dto.Email,
                Address = dto.Address,
                PhoneNo = dto.PhoneNo,
                //PhoneNo2 = dto.PhoneNo2,
                //IsActive = true,
                //IsUser = false,
                TotalBillAmount = dto.TotalBillAmount,
                TotalBillPaid = dto.PaidAmount ?? 0,
                TotalBillPayable = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
                CreatedDate = GeneralUtility.GetCurrentDateTime(),
                //OrgId = orgId,
                CreatedBy = GeneralUtility.GetCurrentUserName(accessor),
                BillPaymentCredits = billPaymentCredits,
                PayingOffCredits = payingOffCredits
            };
        }
    }
}
