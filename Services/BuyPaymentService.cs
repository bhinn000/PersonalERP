using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interfaces;

namespace PersonalERP.Services
{
    public class BuyPaymentService : IBuyPaymentService
    {
        private readonly IBuyPaymentRepo _repo;
        private readonly ILogger<BuyPaymentService> _logger;

        public BuyPaymentService(IBuyPaymentRepo repo, ILogger<BuyPaymentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<BuyPayment>> GetAllAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BoughtService.GetAllAsync");
                return new List<BuyPayment>();
            }
        }

        public async Task<BuyPayment> GetByIdAsync(int id)
        {
            try
            {
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BoughtService.GetByIdAsync for ID: {id}");
                return null;
            }
        }

        public async Task<BuyPayment> AddAsync(BuyPaymentDTO bought)
        {
            try
            {
                var buyPaymentInfo = await _repo.GetByCraftsOrderId(bought.CustomerId);
                var paymentReceivable = buyPaymentInfo.PaymentReceivable;

                var buyPayment = new BuyPayment
                { 
                    CraftsOrderId=bought.CraftsOrderId,
                    CustomerId=bought.CustomerId,
                    PaymentMethod=bought.PaymentMethod,
                    PaidAmount=bought.PaidAmount,
                    PaymentReceivable= paymentReceivable- bought.PaidAmount,
                    CompletelyPaid=(paymentReceivable -  bought.PaidAmount) > 0 
                };

                return await _repo.AddAsync(buyPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BoughtService.AddAsync");
                return null;
            }
        }

        public async Task<BuyPayment> UpdateAsync(BuyPayment bought)
        {
            try
            {
                return await _repo.UpdateAsync(bought);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BoughtService.UpdateAsync for ID: {bought.Id}");
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
                _logger.LogError(ex, $"Error in BoughtService.DeleteAsync for ID: {id}");
                return false;
            }
        }
    }
}
