using PersonalERP.DTO;
using PersonalERP.Entity;

namespace PersonalERP.Interfaces
{
    public interface ICraftsOrderService
    {
        Task<int> CreateAsync(CreateCraftsOrderDto dto);
        Task<CraftsOrder?> GetByIdAsync(int id);
        Task<IEnumerable<CraftsOrder>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
