using PersonalERP.DTO;
using PersonalERP.Entity;

namespace PersonalERP.Interfaces
{
    public interface ICustomerRepo
    {
        public Task<Customer?> GetByPhN(string PhoneNum);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
    }
}
