
//using Microsoft.EntityFrameworkCore;
//using PersonalERP.DTO;
//using PersonalERP.Entity;
//using PersonalERP.Helper;
//using PersonalERP.Interface;

//namespace PersonalERP.Services
//{
//    public class BillPaymentCreditService : IBillPaymentCreditService
//    {
//        private readonly IBillPaymentCreditRepo _billPaymentCreditRepo;
//        private readonly IHttpContextAccessor _accessor;
//        //private readonly IOrganizationRepository _organizationRepository;
//        //private readonly ICustomerV1Repo _customerV1Repo;
//        private readonly IPayingOffCreditRepo _payingOffCreditRepo;
//        private readonly ICustomerRepo _customerRepo;
//        //private readonly IBankRepo _bankRepo;


//        public BillPaymentCreditService(IBillPaymentCreditRepo billPaymentCreditRepo,
//            IHttpContextAccessor accessor,
//            //IOrganizationRepository organizationRepository,
//            //ICustomerV1Repo customerV1Repo,
//            IPayingOffCreditRepo payingOffCreditRepo,
//            ICustomerRepo customerRepo,
//            //IBankRepo bankRepo
//            )
//        {
//            _billPaymentCreditRepo = billPaymentCreditRepo;
//            _accessor = accessor;
//            //_organizationRepository = organizationRepository;
//            //_customerV1Repo = customerV1Repo;
//            _payingOffCreditRepo = payingOffCreditRepo;
//            _bankRepo = bankRepo;
//            _customerRepo = customerRepo;
//        }

//        public async Task<IEnumerable<GetBillPaymentCredit_DTO>> GetAllBillPaymentCredits(string token)
//        {
//            var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//            var billPayments = await _billPaymentCreditRepo.GetList()
//                .Include(x => x.Organization)
//                .Include(x => x.Customer)
//                .Where(x => x.Organization.OrgCode == tokenInfo.OrganizationCode)
//                .ToListAsync();

//            if (tokenInfo != null)
//            {
//                billPayments = billPayments.Where(x => x.Organization.OrgCode == tokenInfo.OrganizationCode).ToList();
//            }

//            return billPayments.Select(bp => new GetBillPaymentCredit_DTO
//            {
//                Id = bp.Id,
//                Amount = bp.BillAmount,
//                PaidAmount = bp.PaidAmount,
//                RemainingAmount = bp.RemainingAmount,
//                CustomerId = bp.CustomerId,
//                CustomerName = bp.Customer.firstName + " " + bp.Customer.lastName,
//                OrgId = bp.OrgId,
//                OrganizationName = bp.Organization.Name,
//                MenuOrderId = (int)bp.MenuOrderId,
//                PaymentMethod = bp.PaymentMethod
//            }).ToList();
//        }

//        public async Task<IEnumerable<CustomerOnCreditDTO>> GetCustomerOnCredit(string token, int id)
//        {
//            try
//            {
//                var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//                var organization = new Organization();
//                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.OrganizationCode))
//                {
//                    organization = _organizationRepository.GetList()
//                        .Where(x => x.OrgCode == tokenInfo.OrganizationCode)
//                        .FirstOrDefault();
//                }

//                if (organization == null)
//                {
//                    throw new Exception("Invalid token !!!");
//                }

//                var customerOnCredit = await _customerV1Repo.GetList().Include(o => o.Organization)
//                                            .Include(x => x.PayingOffCredits)
//                                            .Where(x => id == x.Id && x.OrgId == organization.Id).ToListAsync();

//                if (customerOnCredit == null || !customerOnCredit.Any())
//                {
//                    throw new Exception("Customer doesn't exist");
//                }
//                //if (tokenInfo != null)
//                //{
//                //    customerOnCredit = customerOnCredit.Where(x => x.Organization.OrgCode == tokenInfo.OrganizationCode).ToList();
//                //}

//                return customerOnCredit.Select(cc => new CustomerOnCreditDTO
//                {
//                    CustomerName = cc.firstName + " " + cc.lastName,
//                    CustomerPhone = cc.PhoneNo,
//                    CustomerAddress = cc.Address,
//                    CustomerEmail = cc.Email,
//                    TotalBillAmount = cc.TotalBillAmount,
//                    TotalRemAmount = cc.TotalBillPayable,
//                    TransactionHistory = cc.PayingOffCredits?.Select(p => new CustomerPayingOffDTO
//                    {
//                        CreatedDate = p.CreatedDate,
//                        CreatedBy = p.CreatedBy,
//                        TransactionAmout = p.TotalBillPaid,
//                        RemainingAmount = p.TotalBillRemaining,
//                        PaymentMethod = p.PaymentMethod,
//                        BankId = p.BankId

//                    }).ToList()
//                });
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<GetBillPaymentCredit_DTO?> GetBillPaymentCreditById(int id)
//        {
//            var bp = await _billPaymentCreditRepo.GetList().Include(x => x.Customer).Include(x => x.Organization).FirstOrDefaultAsync(bp => bp.Id == id);
//            if (bp == null) return null;

//            return new GetBillPaymentCredit_DTO
//            {
//                Id = bp.Id,
//                Amount = bp.BillAmount,
//                PaidAmount = bp.PaidAmount,
//                RemainingAmount = bp.RemainingAmount,
//                CustomerId = bp.CustomerId,
//                CustomerName = bp.Customer.firstName + " " + bp.Customer.lastName,
//                OrgId = bp.OrgId,
//                OrganizationName = bp.Organization.Name,
//                MenuOrderId = (int)bp.MenuOrderId,
//                PaymentMethod = bp.PaymentMethod
//            };
//        }

//        public async Task<string> CreateBillPaymentCreditOriginal_WITHOUTHELPER(CreateBillPaymentCredit_DTO dto, string token)
//        {
//            try
//            {
//                var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//                var organization = new Organization();
//                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.OrganizationCode))
//                {
//                    organization = _organizationRepository.GetList()
//                        .Where(x => x.OrgCode == tokenInfo.OrganizationCode)
//                        .FirstOrDefault();
//                }

//                if (organization == null)
//                {
//                    throw new Exception("Invalid token for organization !!!");
//                }

//                if ((dto.PaidAmount ?? 0) > 0 && (dto.PaymentMethod == 0 || dto.PaymentMethod == null))
//                {
//                    throw new Exception("PaymentMethod is required when PaidAmount is provided.");
//                }

//                var isMenuOrderIdAlreadyExists = await _customerV1Repo.GetList()
//                                                .SelectMany(c => c.BillPaymentCredits)
//                                                .AnyAsync(bpc => bpc.MenuOrderId == dto.MenuOrderId);

//                if (isMenuOrderIdAlreadyExists)
//                {
//                    throw new Exception($"MenuOrderId {dto.MenuOrderId} already exists for another BillPaymentCredit.");
//                }

//                var customerPreData = await _customerV1Repo.GetList()
//                    .Include(x => x.BillPaymentCredits)
//                    .Include(x => x.PayingOffCredits)
//                    .Where(x => x.PhoneNo == dto.PhoneNo).FirstOrDefaultAsync();

//                if (customerPreData != null)
//                {
//                    customerPreData.TotalBillAmount += dto.TotalBillAmount;
//                    customerPreData.TotalBillPaid = (customerPreData.TotalBillPaid ?? 0) + (dto.PaidAmount ?? 0);
//                    customerPreData.TotalBillPayable = customerPreData.TotalBillAmount - (customerPreData.TotalBillPaid ?? 0);

//                    var billPaymentCredit = new BillPaymentCredit
//                    {
//                        MenuOrderId = dto.MenuOrderId,
//                        BillAmount = dto.TotalBillAmount,
//                        PaidAmount = dto.PaidAmount ?? 0,
//                        RemainingAmount = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
//                        PaymentMethod = dto.PaymentMethod,
//                        CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                        CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                        //OrgId = organization.Id,
//                        CustomerId = customerPreData.Id
//                    };

//                    customerPreData.BillPaymentCredits.Add(billPaymentCredit);

//                    if (dto.PaidAmount != null)
//                    {
//                        customerPreData.PayingOffCredits.Add(
//                            new PayingOffCredit
//                            {
//                                CustomerId = customerPreData.Id,
//                                PaymentMethod = (int)dto.PaymentMethod,
//                                TotalAmount = 0,
//                                TotalBillPaid = dto.PaidAmount ?? 0,
//                                TotalBillRemaining = 0,
//                                CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                                CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                                //OrgId = organization.Id,
//                            }
//                        );
//                    }
//                }
//                else
//                {
//                    var billPaymentCredit = new List<BillPaymentCredit>
//                    {
//                        new BillPaymentCredit
//                        {
//                            BillAmount = dto.TotalBillAmount,
//                            PaidAmount = dto.PaidAmount,
//                            RemainingAmount = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
//                            CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                            CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                            //OrgId = organization.Id,
//                            MenuOrderId=dto.MenuOrderId,
//                        }
//                    };

//                    var payingOffCredit = new List<PayingOffCredit>();
//                    if (dto.PaidAmount != null)
//                    {
//                        payingOffCredit = new List<PayingOffCredit>
//                                    {
//                                        new PayingOffCredit
//                                        {
//                                            PaymentMethod=(int)dto.PaymentMethod,
//                                            TotalBillPaid = dto.PaidAmount?? 0,
//                                            TotalAmount=0,
//                                            TotalBillRemaining=0,
//                                            CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                                            CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                                            //OrgId=organization.Id,
//                                        }
//                                    };
//                    }


//                    var customerOnCredit = new Customer
//                    {
//                        UserId = null,
//                        Username = dto.PhoneNo,
//                        //firstName = dto.FirstName,
//                        //lastName = dto.LastName,
//                        //Email = dto.Email,
//                        Address = dto.Address,
//                        PhoneNo = dto.PhoneNo,
//                        //PhoneNo2 = dto.PhoneNo2,
//                        //IsActive = true,
//                        //IsUser = false,
//                        TotalBillAmount = dto.TotalBillAmount,
//                        TotalBillPaid = dto.PaidAmount ?? 0,
//                        TotalBillPayable = dto.TotalBillAmount - (dto.PaidAmount ?? 0),
//                        CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                        //OrgId = organization.Id,
//                        CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                        BillPaymentCredits = billPaymentCredit,
//                        PayingOffCredits = payingOffCredit
//                    };
//                    _customerV1Repo.Insert(customerOnCredit);
//                }

//                await _customerV1Repo.SaveAsync();

//                return "Data updated !!!";
//            }
//            catch (Exception ex)
//            {

//                throw new Exception($"An error occurred while updating the creating new bill payment on credit status: {ex.Message}");
//            }


//        }

//        public async Task<string> CreateBillPaymentCredit(CreateBillPaymentCredit_DTO dto)
//        {
//            try
//            {
//                //var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//                //var organization = new Organization();
//                //if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.OrganizationCode))
//                //{
//                //    organization = _organizationRepository.GetList()
//                //        .Where(x => x.OrgCode == tokenInfo.OrganizationCode)
//                //        .FirstOrDefault();
//                //}
//                //Bank bank = null;
//                //if (organization == null)
//                //    throw new Exception("Invalid token for organization !!!");

//                if ((dto.PaidAmount ?? 0) > 0 && (dto.PaymentMethod == 0 || dto.PaymentMethod == null))
//                    throw new Exception("PaymentMethod is required when PaidAmount is provided.");

//                //var isMenuOrderIdAlreadyExists = await _customerV1Repo.GetList()
//                //    .SelectMany(c => c.BillPaymentCredits)
//                //    .AnyAsync(bpc => bpc.MenuOrderId == dto.MenuOrderId);

//                //if (isMenuOrderIdAlreadyExists)
//                //    throw new Exception($"MenuOrderId {dto.MenuOrderId} already exists for another BillPaymentCredit.");

//                var customer = await _customerRepo.GetList()
//                    .Include(x => x.BillPaymentCredits)
//                    .Include(x => x.PayingOffCredits)
//                    .FirstOrDefaultAsync(x => x.PhoneNo == dto.PhoneNo);


//                //if (dto.PaymentMethod == 1)
//                //{
//                //    if (dto.BankId == null || dto.BankId == 0)
//                //        throw new Exception("Bank selection is required when PaymentMethod is Bank.");

//                //    bank = await _bankRepo.GetList()
//                //   .FirstOrDefaultAsync(b => b.Id == dto.BankId && b.OrgId == organization.Id);

//                //    if (bank == null)
//                //        throw new Exception("Selected bank is not valid for this organization.");
//                //}


//                var paymentOnCredit = dto.TotalBillAmount - (dto.PaidAmount ?? 0);
//                if (customer != null)//if old customer
//                {

//                    customer.TotalBillAmount += dto.TotalBillAmount;
//                    customer.TotalBillPaid = (customer.TotalBillPaid ?? 0) + (dto.PaidAmount ?? 0);
//                    customer.TotalBillPayable = customer.TotalBillAmount - (customer.TotalBillPaid ?? 0);

//                    if (customer.CurrentCreditLimit.HasValue && customer.CurrentCreditLimit.Value < dto.TotalBillAmount - (dto.PaidAmount ?? 0))
//                    {
//                        throw new Exception($"Customer does not have enough credit limit  to cover the remaining bill.Only {customer.CurrentCreditLimit}  remains");
//                    }
//                    else
//                    {
//                        customer.CurrentCreditLimit -= paymentOnCredit;
//                    }

//                    var billPayment = CustomerOnCreditHelper.CreateBillPaymentCredit_ExistingCustomer(dto, organization.Id, customer.Id, _accessor);
//                    customer.BillPaymentCredits.Add(billPayment);

//                    if (dto.PaidAmount != null)
//                    {
//                        var payingOff = CustomerOnCreditHelper.CreatePayingOffCredit_ExistingCustomer(dto, customer.Id, organization.Id, _accessor);
//                        customer.PayingOffCredits.Add(payingOff);
//                    }
//                }
            

//                if (dto.PaymentMethod == 1)
//                {
//                    bank.CurrentBalance += dto.PaidAmount ?? 0;
//                }
//                await _customerV1Repo.SaveAsync();

//                return "Data updated !!!";
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while creating new bill payment on credit status: {ex.Message}");
//            }
//        }

//        public async Task<string> PayingOffOnCredit(PayingOffCreditDTO dto, string token)
//        {
//            try
//            {
//                //var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//                //var organization = new Organization();
//                //if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.OrganizationCode))
//                //{
//                //    organization = _organizationRepository.GetList()
//                //        .Where(x => x.OrgCode == tokenInfo.OrganizationCode)
//                //        .FirstOrDefault();
//                //}

//                //if (organization == null)
//                //{
//                //    throw new Exception("Invalid token !!!");
//                //}

//                var customerData = await _customerV1Repo.GetList().Where(x => x.PhoneNo == dto.PhoneNo).FirstOrDefaultAsync();

//                if (customerData == null || customerData.TotalBillAmount <= 0)
//                {
//                    throw new Exception("Customer doesn't have credit ");
//                }

//                Bank bank = null;
//                if (dto.PaymentMethod == 1)
//                {
//                    if (dto.BankId == null || dto.BankId == 0)
//                        throw new Exception("Bank selection is required when PaymentMethod is Bank.");

//                    bank = await _bankRepo.GetList()
//                   .FirstOrDefaultAsync(b => b.Id == dto.BankId && b.OrgId == organization.Id);

//                    if (bank == null)
//                        throw new Exception("Selected bank is not valid for this organization.");
//                }

//                if (customerData.TotalBillPayable < dto.PayingAmount)
//                {
//                    throw new Exception($"Customer only has to pay {customerData.TotalBillPayable} ");
//                }

//                customerData.TotalBillPaid = dto.PayingAmount + (customerData.TotalBillPaid ?? 0);
//                customerData.TotalBillPayable = customerData.TotalBillPayable - dto.PayingAmount;

//                customerData.CurrentCreditLimit += dto.PayingAmount;

//                if (customerData.CurrentCreditLimit > customerData.InitialCreditLimit)
//                {
//                    customerData.CurrentCreditLimit = customerData.InitialCreditLimit;
//                }

//                customerData.ModifiedDate = GeneralUtility.GetCurrentDateTime();
//                customerData.ModifiedBy = GeneralUtility.GetCurrentUserName(_accessor);

//                var payingOffCredit = new PayingOffCredit
//                {
//                    CustomerId = customerData.Id,
//                    //OrgId = organization.Id,
//                    PaymentMethod = dto.PaymentMethod,
//                    TotalAmount = 0,
//                    //TotalAmount = customerData.TotalBillAmount,
//                    TotalBillPaid = dto.PayingAmount,
//                    //TotalBillRemaining = customerData.TotalBillAmount - (customerData.TotalBillPaid ?? 0),
//                    TotalBillRemaining = 0,
//                    CreatedDate = GeneralUtility.GetCurrentDateTime(),
//                    CreatedBy = GeneralUtility.GetCurrentUserName(_accessor),
//                    BankId = dto.BankId
//                };

//                decimal amountToPay = dto.PayingAmount;
//                var oldestUnpaidBills = await _billPaymentCreditRepo.GetList()
//                                .Where(x => x.CustomerId == customerData.Id && x.RemainingAmount > 0)
//                                .OrderBy(x => x.Id) // oldest first
//                                .ToListAsync();

//                foreach (var bill in oldestUnpaidBills)
//                {
//                    if (amountToPay <= 0)
//                        break;

//                    decimal amountToApply = Math.Min(bill.RemainingAmount, amountToPay);

//                    bill.PaymentMethod = dto.PaymentMethod;
//                    bill.PaidAmount = amountToApply + bill.PaidAmount ?? 0;
//                    bill.RemainingAmount -= amountToApply;
//                    bill.OrgId = organization.Id;
//                    bill.ModifiedDate = GeneralUtility.GetCurrentDateTime();
//                    bill.ModifiedBy = GeneralUtility.GetCurrentUserName(_accessor);

//                    amountToPay -= amountToApply;
//                    _billPaymentCreditRepo.Update(bill);
//                }


//                if (dto.PaymentMethod == 1)
//                {
//                    bank.CurrentBalance += dto.PayingAmount;
//                }

//                _payingOffCreditRepo.Insert(payingOffCredit);
//                await _customerV1Repo.SaveAsync();

//                return "Data updated !!!";
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }

//        }

//        public async Task<bool> UpdateBillPaymentCredit(UpdateBillPaymentCredit_DTO dto, string token)
//        {
//            var tokenInfo = GeneralUtility.extractInfoFromToken(token);

//            //var organization = new Organization();
//            //if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.OrganizationCode))
//            //{
//            //    organization = _organizationRepository.GetList()
//            //        .Where(x => x.OrgCode == tokenInfo.OrganizationCode)
//            //        .FirstOrDefault();
//            //}

//            //if (organization == null)
//            //{
//            //    throw new Exception("Invalid token for organization !!!");
//            //}

//            var existingBillPayment = await _billPaymentCreditRepo.FindAsync(dto.Id);
//            if (existingBillPayment == null) return false;

//            if (existingBillPayment.RemainingAmount < 0)
//                throw new InvalidOperationException("Cannot update a bill that is not payable.");

//            //var customer = await _customerV1Repo.FindAsync(dto.CustomerId);
//            //if (customer == null)
//            //    throw new ArgumentException("Customer does not exist.");

//            if (dto.PaidAmount.HasValue && dto.PaidAmount.Value > dto.Amount)
//            {
//                throw new ArgumentException("Paid amount cannot be more than the total amount.");
//            }

//            existingBillPayment.BillAmount = dto.Amount;
//            existingBillPayment.PaidAmount = dto.PaidAmount;
//            existingBillPayment.RemainingAmount = dto.Amount - (dto.PaidAmount ?? 0);
//            //existingBillPayment.CustomerId = dto.CustomerId;
//            existingBillPayment.ModifiedDate = GeneralUtility.GetCurrentDateTime();
//            existingBillPayment.ModifiedBy = GeneralUtility.GetCurrentUserName(_accessor);
//            existingBillPayment.OrgId = organization.Id;

//            _billPaymentCreditRepo.Update(existingBillPayment);
//            await _billPaymentCreditRepo.SaveAsync();

//            return true;
//        }

//        public async Task<bool> DeleteBillPaymentCredit(int id)
//        {
//            var billPayment = await _billPaymentCreditRepo.FindAsync(id);
//            if (billPayment == null) return false;

//            billPayment.DeletedDate = GeneralUtility.GetCurrentDateTime();
//            billPayment.DeletedBy = GeneralUtility.GetCurrentUserName(_accessor);

//            _billPaymentCreditRepo.Update(billPayment);
//            await _billPaymentCreditRepo.SaveAsync();

//            return true;
//        }
//    }
//}
