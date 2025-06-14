using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;
using PersonalERP.Services;

namespace PersonalERP.Repo
{
    public class PayingOffCreditRepo : IPayingOffCreditRepo
    {
        private readonly IUserContextService _userContextService;
        private readonly AppDbContext _context;
        private readonly ILogger<PayingOffCreditRepo> _logger;

        public PayingOffCreditRepo(AppDbContext context, ILogger<PayingOffCreditRepo> logger, IUserContextService userContextService)
        {
            _context = context;
            _logger = logger;
            _userContextService = userContextService;
        }

        public async Task<List<PayingOffCredit>> GetAllAsync()
        {
            try
            {
                return await _context.PayingOffCredits
                    .Include(p => p.BillPaymentCredit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all PayingOffCredits.");
                return new List<PayingOffCredit>();
            }
        }

        public async Task<PayingOffCredit> GetByIdAsync(int id)
        {
            try
            {
                return await _context.PayingOffCredits
                    .Include(p => p.BillPaymentCredit)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching PayingOffCredit with ID {id}");
                throw;
            }
        }

        public async Task<PayingOffCredit> AddAsync(PayingOffCredit credit)
        {
            try
            {
                var entity = new PayingOffCredit
                {
                    PaymentMethod = credit.PaymentMethod,
                    TotalAmount = credit.TotalAmount,
                    TotalBillPaid = credit.TotalBillPaid,
                    TotalBillRemaining = credit.TotalBillRemaining,
                    BankId = credit.BankId,
                    BPId = credit.BPId,
                    CreatedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser",
                    CreatedDate = DateTime.UtcNow,
                };

                _context.PayingOffCredits.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding PayingOffCredit.");
                return null;
            }
        }

        public async Task AddInitialAsync(PayingOffCredit credit)
        {
            _context.PayingOffCredits.Add(credit);
            await _context.SaveChangesAsync();

        }

        public async Task<PayingOffCredit> UpdateAsync(PayingOffCredit credit)
        {
            try
            {
                var existing = await _context.PayingOffCredits.FindAsync(credit.Id);
                if (existing == null) return null;

                existing.PaymentMethod = credit.PaymentMethod;
                existing.TotalAmount = credit.TotalAmount;
                existing.TotalBillPaid = credit.TotalBillPaid;
                existing.TotalBillRemaining = credit.TotalBillRemaining;
                existing.BankId = credit.BankId;
                existing.BPId = credit.BPId;

                _context.PayingOffCredits.Update(existing);
                await _context.SaveChangesAsync();
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating PayingOffCredit with ID {credit.Id}");
                return null;
            }
        }



        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var credit = await _context.PayingOffCredits.FindAsync(id);
                if (credit == null) return false;

                _context.PayingOffCredits.Remove(credit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting PayingOffCredit with ID {id}");
                return false;
            }
        }
    }
}
