using Microsoft.EntityFrameworkCore;
using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;

namespace PersonalERP.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerRepo> _logger;

        public CustomerRepo(AppDbContext context, ILogger<CustomerRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer?> GetByPhN(string PhoneNum)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.PhoneNum == PhoneNum);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                return await _context.Customers.Include(c=>c.CraftsOrders).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all customers.");
                return new List<Customer>();
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Customers.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching customer with ID: {id}");
                throw;
            }
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer.");
                return null;
            }
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating customer with ID: {customer.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return false;

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with ID: {id}");
                return false;
            }
        }
    }
}
