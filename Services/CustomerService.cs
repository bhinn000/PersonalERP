using Microsoft.EntityFrameworkCore;
using PersonalERP.Entity;
using PersonalERP.Interface;

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

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            try
            {
                List<Customer> customers= await _repo.GetAllAsync();

                var customerDtos = customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Name = c.Name,
                    Address = c.Address,
                    PhoneNum = c.PhoneNum,
                    TotalBillAmount = c.TotalBillAmount,
                    TotalBillPaid = c.TotalBillPaid,
                    TotalBillPayable = c.TotalBillPayable,
                    InitialCreditLimit = c.InitialCreditLimit,
                    CurrentCreditLimit = c.CurrentCreditLimit,
                    CraftsOrders = c.CraftsOrders?.Select(co => new CraftsOrderDto
                    {
                        Id = co.Id,
                        OrderRef = co.OrderRef,
                        ArtName = co.ArtName,
                        Price = co.Price,
                        Description = co.Description,
                        ArtId = co.ArtId
                    }).ToList() ?? new List<CraftsOrderDto>()
                }).ToList();

                return customerDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in GetAllAsync.");
                return new List<CustomerDto>();
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
