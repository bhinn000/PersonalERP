using PersonalERP.DTO;

namespace PersonalERP.Interface
{
    public interface IPayingOffCreditService
    {
        Task<List<PayingOffDTO>> GetAllAsync();
        Task<PayingOffDTO> GetByIdAsync(int id);
        Task<PayingOffResponseDTO> AddAsync(PayingOffDTO creditDto);
        Task<PayingOffDTO> UpdateAsync(PayingOffDTO creditDto);
        Task<bool> DeleteAsync(int id);
    }
}
