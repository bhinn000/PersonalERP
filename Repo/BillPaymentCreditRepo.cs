using PersonalERP.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalERP.Interface;

namespace PersonalERP.Repo
{
    public class BillPaymentCreditRepo : IBillPaymentCreditRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BillPaymentCreditRepo> _logger;

        public BillPaymentCreditRepo(AppDbContext context, ILogger<BillPaymentCreditRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<BillPaymentCredit>> GetAllAsync()
        {
            try
            {
                return await _context.BillPaymentCredits
                    .Include(b => b.Customer)
                    .Include(b => b.CraftsOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BillPaymentCreditRepo.GetAllAsync");
                return new List<BillPaymentCredit>();
            }
        }

        public async Task<BillPaymentCredit?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.BillPaymentCredits
                    .Include(b => b.Customer)
                    .Include(b => b.CraftsOrder)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BillPaymentCreditRepo.GetByIdAsync for ID: {id}");
                return null;
            }
        }

        public async Task<BillPaymentCredit> AddAsync(BillPaymentCredit billPaymentCredit)
        {
            try
            {
                _context.BillPaymentCredits.Add(billPaymentCredit);
                await _context.SaveChangesAsync();
                return billPaymentCredit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BillPaymentCreditRepo.AddAsync");
                return null;
            }
        }

        public async Task<BillPaymentCredit> UpdateAsync(BillPaymentCredit billPaymentCredit)
        {
            try
            {
                _context.BillPaymentCredits.Update(billPaymentCredit);
                await _context.SaveChangesAsync();
                return billPaymentCredit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BillPaymentCreditRepo.UpdateAsync for ID: {billPaymentCredit.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.BillPaymentCredits.FindAsync(id);
                if (entity == null)
                    return false;

                _context.BillPaymentCredits.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BillPaymentCreditRepo.DeleteAsync for ID: {id}");
                return false;
            }
        }
    }
}
