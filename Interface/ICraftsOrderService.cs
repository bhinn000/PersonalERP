using PersonalERP.DTO;
using PersonalERP.Entity;

namespace PersonalERP.Interface
{
    public interface ICraftsOrderService
    {
        Task<int> CreateAsync(CreateCraftsOrderDto dto);
        Task<CraftsOrderDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CraftsOrderDTO>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
