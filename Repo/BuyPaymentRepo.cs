//using PersonalERP.Entity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using PersonalERP.DTO;
//using PersonalERP.Interface;

//namespace PersonalERP.Repo
//{
//    public class BuyPaymentRepo : IBuyPaymentRepo
//    {
//        private readonly AppDbContext _context;
//        private readonly ILogger<BuyPaymentRepo> _logger;

//        public BuyPaymentRepo(AppDbContext context, ILogger<BuyPaymentRepo> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<List<BuyPayment>> GetAllAsync()
//        {
//            try
//            {
//                return await _context.BuyPayments.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in BoughtRepo.GetAllAsync");
//                return new List<BuyPayment>();
//            }
//        }

//        public async Task<BuyPayment> GetByIdAsync(int id)
//        {
//            try
//            {
//                return await _context.BuyPayments.FindAsync(id);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtRepo.GetByIdAsync for ID: {id}");
//                return null;
//            }
//        }

//        public async Task<bool> AddAsync(BuyPayment bought)
//        {
//            try
//            {
//                _context.BuyPayments.Add(bought);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in BoughtRepo.AddAsync");
//                return false;
//            }
//        }

//        public async Task<BuyPayment> UpdateAsync(BuyPayment bought)
//        {
//            try
//            {
//                _context.BuyPayments.Update(bought);
//                await _context.SaveChangesAsync();
//                return bought;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtRepo.UpdateAsync for ID: {bought.Id}");
//                return null;
//            }
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            try
//            {
//                var entity = await _context.BuyPayments.FindAsync(id);
//                if (entity == null)
//                    return false;

//                _context.BuyPayments.Remove(entity);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtRepo.DeleteAsync for ID: {id}");
//                return false;
//            }
//        }

//        public async Task<BuyPayment?> GetByCraftsOrderId(int customerId)
//        {
//            try
//            {
//                var buyPaymentInfo=await _context.BuyPayments.FindAsync(customerId);
//                return buyPaymentInfo;
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtRepo.GetByCraftsOrderId for ID");
//                return null;
//            }
//        }
//    }
//}
