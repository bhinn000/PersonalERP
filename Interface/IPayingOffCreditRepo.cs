using PersonalERP.DTO;
using PersonalERP.Entity;

namespace PersonalERP.Interface
{
    public interface IPayingOffCreditRepo
    {
        Task<List<PayingOffCredit>> GetAllAsync();
        Task<PayingOffCredit> GetByIdAsync(int id);
        Task<PayingOffCredit> AddAsync(PayingOffCredit credit);
        Task<PayingOffCredit> UpdateAsync(PayingOffCredit credit);
        Task<bool> DeleteAsync(int id);
    }
}
