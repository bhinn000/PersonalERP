using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;

namespace PersonalERP.Services
{
    public class PayingOffCreditService : IPayingOffCreditService
    {
        private readonly IPayingOffCreditRepo _repo;
        private readonly IBillPaymentCreditRepo _billPaymentCreditRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<PayingOffCreditService> _logger;

        public PayingOffCreditService(IPayingOffCreditRepo repo, ILogger<PayingOffCreditService> logger, IUserContextService userContextService,
            IBillPaymentCreditRepo billPaymentCreditRepo, ICustomerRepo customerRepo
            )
        {
            _repo = repo;
            _logger = logger;
            _customerRepo = customerRepo;
            _billPaymentCreditRepo = billPaymentCreditRepo;
            _userContextService = userContextService;
        }

        public async Task<List<PayingOffDTO>> GetAllAsync()
        {
            try
            {
                var data = await _repo.GetAllAsync();
                return data.Select(p => MapToDTO(p)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in GetAllAsync.");
                return new List<PayingOffDTO>();
            }
        }

        public async Task<PayingOffDTO> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                return entity != null ? MapToDTO(entity) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in GetByIdAsync for ID: {id}");
                return null;
            }
        }

        public async Task<PayingOffResponseDTO> AddAsync(PayingOffDTO creditDto)
        {
            try
            {
                var BP = await _billPaymentCreditRepo.GetByIdAsync(creditDto.BPId);
                if(BP is null)
                {
                    throw new Exception("Sorry the given BP doesnt exist");
                }
                
                if(BP.CustomerId != creditDto.CustomerId)
                {
                    throw new Exception("For that BP , there is no such customer id");
                }

                var customer = await _customerRepo.GetByIdAsync(creditDto.CustomerId);
                if (customer == null)
                {
                    throw new Exception("Customer does not exist.");
                }

                decimal paidAmount = creditDto.TotalBillPaid;
                decimal overPayment = 0;

                if (BP.PaymentReceivable < paidAmount)
                {
                    overPayment = paidAmount - BP.PaymentReceivable.Value;
                    creditDto.TotalBillPaid = BP.PaymentReceivable.Value; // Pay only what is required for this BP
                }

                var entity = MapToEntity(creditDto, BP);
                var result = await _repo.AddAsync(entity);

                BP.PaidAmount = (BP.PaidAmount ?? 0) + creditDto.TotalBillPaid;
                BP.PaymentReceivable = BP.PaymentReceivable - creditDto.TotalBillPaid;
                BP.IsInitialPayment = false;
                BP.PaymentMethod = creditDto.PaymentMethod;
                BP.CompletelyPaid = BP.PaymentReceivable <= 0;
                BP.ModifiedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";
                BP.ModifiedDate = DateTime.UtcNow;

                await _billPaymentCreditRepo.UpdateAsync(BP);

                

                if (overPayment > 0)
                {
                    var otherBPs = customer.BillPaymentCredits
                        .Where(x => x.Id != BP.Id && !x.CompletelyPaid)
                        .OrderBy(x => x.Id)
                        .ToList();

                    foreach (var otherBP in otherBPs)
                    {
                        if (overPayment <= 0)
                            break;

                        decimal receivable = otherBP.PaymentReceivable ?? 0;
                        if (receivable <= 0)
                            continue;

                        decimal amountToPay = Math.Min(receivable, overPayment);

                        var extraPay = new PayingOffCredit
                        {
                            PaymentMethod = creditDto.PaymentMethod,
                            TotalAmount = otherBP.BillAmount,
                            TotalBillPaid = amountToPay,
                            TotalBillRemaining = receivable - amountToPay,
                            BankId = creditDto.BankId,
                            BPId = otherBP.Id
                        };

                        await _repo.AddAsync(extraPay);

                        otherBP.PaidAmount = (otherBP.PaidAmount ?? 0) + amountToPay;
                        otherBP.PaymentReceivable -= amountToPay;
                        otherBP.CompletelyPaid = otherBP.PaymentReceivable <= 0;
                        otherBP.IsInitialPayment = false;
                        otherBP.PaymentMethod = creditDto.PaymentMethod;
                        otherBP.ModifiedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";
                        otherBP.ModifiedDate = DateTime.UtcNow;

                        await _billPaymentCreditRepo.UpdateAsync(otherBP);

                        overPayment -= amountToPay;
                    }
                }

                customer.TotalBillPaid = (customer.TotalBillPaid ?? 0) + creditDto.TotalBillPaid;
                customer.TotalBillPayable -= creditDto.TotalBillPaid;
                customer.CurrentCreditLimit = Math.Min(
                    (customer.CurrentCreditLimit ?? 0) + creditDto.TotalBillPaid,
                    customer.InitialCreditLimit
                );

                customer.ModifiedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";
                customer.ModifiedDate = DateTime.UtcNow;

                await _customerRepo.UpdateAsync(customer);
                var response = new PayingOffResponseDTO
                {
                    Data = MapToDTO(result),
                    Message = overPayment > 0
                ? $"Note: Rs. {overPayment} remains after adjusting all pending credits."
                : null
                        };

                return response;


                //return MapToDTO(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in AddAsync.");
                throw new Exception(ex.Message);
                //return null;
            }
        }

        public async Task<PayingOffDTO> UpdateAsync(PayingOffDTO creditDto)
        {
            try
            {
                var BP = await _billPaymentCreditRepo.GetByIdAsync(creditDto.BPId);
                if (BP is null)
                {
                    throw new Exception("Sorry the given BP doesnt exist");
                }

                var entity = MapToEntity(creditDto, BP);
                var result = await _repo.UpdateAsync(entity);
                return MapToDTO(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in UpdateAsync for ID: {creditDto.Id}");
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

        //Helper methods for mapping
        private PayingOffDTO MapToDTO(PayingOffCredit entity)
        {
            var PODTO= new PayingOffDTO
            {
                Id = entity.Id,
                PaymentMethod = entity.PaymentMethod,
                //TotalAmount = entity.TotalAmount,
                TotalBillPaid = entity.TotalBillPaid,
                BankId = entity.BankId,
                BPId = entity.BPId
            };
            return PODTO;
        }

        private PayingOffCredit MapToEntity(PayingOffDTO dto , BillPaymentCredit bp)
        {
            if( bp.PaymentReceivable is null || bp.PaymentReceivable <= 0)
            {
                throw new Exception("There is nothing to pay for this BillPayment Credit Row");
            }

            if (dto.TotalBillPaid <= 0)
            {
                throw new Exception("Paid amount must be greater than zero.");
            }

            //if (dto.TotalBillPaid > bp.PaymentReceivable)
            //{
            //    throw new Exception($"Paid amount ({dto.TotalBillPaid}) cannot exceed the remaining receivable ({bp.PaymentReceivable}).");
            //}

            var totalAmount = bp.BillAmount;
            var remPayingAmount = bp.PaymentReceivable - dto.TotalBillPaid;

            var POCredit= new PayingOffCredit
            {
                Id = dto.Id,
                PaymentMethod = dto.PaymentMethod,
                TotalAmount = totalAmount,
                TotalBillPaid = dto.TotalBillPaid,
                TotalBillRemaining = (decimal)remPayingAmount,
                BankId = dto.BankId,
                BPId = dto.BPId
            };
            return POCredit;
        }
    }
}
