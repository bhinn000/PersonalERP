using PersonalERP.Entity;
using PersonalERP.Interfaces;



namespace PersonalERP.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _repo;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepo repo, ILogger<CustomerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in GetAllAsync.");
                return new List<Customer>();
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _repo.GetByIdAsync(id);
                if (customer == null)
                    throw new KeyNotFoundException($"Customer with ID {id} not found.");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in GetByIdAsync for ID: {id}");
                throw;
            }
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                return await _repo.AddAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in AddAsync.");
                return null;
            }
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            try
            {
                return await _repo.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in UpdateAsync for ID: {customer.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in DeleteAsync for ID: {id}");
                return false;
            }
        }
    }
}
