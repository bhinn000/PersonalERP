using PersonalERP.Entity;

namespace PersonalERP.Interface
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
    }
}
