using PersonalERP.DTO;
using PersonalERP.Entity;

namespace PersonalERP.Interface
{
    public interface ICraftsOrderService
    {
        Task<int> CreateAsync(CreateCraftsOrderDto dto);
        Task<CraftsOrder?> GetByIdAsync(int id);
        Task<IEnumerable<CraftsOrderDTO>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
