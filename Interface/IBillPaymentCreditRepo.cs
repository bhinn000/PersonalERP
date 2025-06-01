using PersonalERP.Entity;

namespace PersonalERP.Interface
{
    public interface IBillPaymentCreditRepo
    {
        Task<List<BillPaymentCredit>> GetAllAsync();
        Task<BillPaymentCredit?> GetByIdAsync(int id);
        Task<BillPaymentCredit> AddAsync(BillPaymentCredit billPaymentCredit);
        Task<BillPaymentCredit> UpdateAsync(BillPaymentCredit billPaymentCredit);
        Task<bool> DeleteAsync(int id);
    }
}

