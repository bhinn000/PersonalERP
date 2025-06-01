//using Microsoft.AspNetCore.Mvc;
//using PersonalERP.Entity;
//using PersonalERP.Interfaces;
//using Microsoft.Extensions.Logging;
//using PersonalERP.DTO;
//using PersonalERP.Services;
//using static PersonalERP.Enum;

//namespace PersonalERP.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BuyPaymentController : ControllerBase
//    {
//        private readonly IBuyPaymentService _service;
//        private readonly ILogger<BuyPaymentController> _logger;
//        private readonly ICraftsOrderService _craftOrderService;
//        private readonly ICraftsOrderRepo _craftsOrderRepo;
//        //private readonly ICounterCashTransactionService _counterCashTransactionService;
//        //private readonly IBankService _bankService;
//        //private readonly ILogger<CraftsOrderController> _logger;
//        //private readonly AuthService _authService;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IBillPaymentCreditService _billPaymentCreditService;

//        public BuyPaymentController(IBuyPaymentService service, ILogger<BuyPaymentController> logger, ICraftsOrderService craftOrderService,
//            //AuthService authService,
//            //ICounterCashTransactionService counterCashTransactionService,
//            //IBankService bankService,
//            IHttpContextAccessor httpContextAccessor,
//            IBillPaymentCreditService billPaymentCreditService,
//            ICraftsOrderRepo craftsOrderRepo
//            )
//        {
//            _service = service;
//            _logger = logger;
//            //_authService = authService;
//            _craftOrderService = craftOrderService;
//            //_counterCashTransactionService = counterCashTransactionService;
//            //_bankService = bankService;
//            _httpContextAccessor = httpContextAccessor;
//            _billPaymentCreditService = billPaymentCreditService;
//            _craftsOrderRepo = craftsOrderRepo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var items = await _service.GetAllAsync();
//                return Ok(items);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in BoughtController.GetAll");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            try
//            {
//                var item = await _service.GetByIdAsync(id);
//                if (item == null)
//                    return NotFound($"Bought with ID {id} not found.");
//                return Ok(item);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtController.GetById for ID: {id}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add([FromBody] BuyPaymentDTO bought)
//        {
//            try
//            {
//                var created = await _service.AddAsync(bought);
//                if (created is  null)
//                    return BadRequest("Failed to add bought.");
//                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in BoughtController.Add");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] BuyPayment bought)
//        {
//            if (id != bought.Id)
//                return BadRequest("ID mismatch");

//            try
//            {
//                var updated = await _service.UpdateAsync(bought);
//                if (updated == null)
//                    return NotFound($"Bought with ID {id} not found.");
//                return Ok(updated);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtController.Update for ID: {id}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var deleted = await _service.DeleteAsync(id);
//                if (!deleted)
//                    return NotFound($"Bought with ID {id} not found.");
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in BoughtController.Delete for ID: {id}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        //        [HttpPatch("Billing/{id}/Payment")]
//        //        public async Task<IActionResult> Payment(int id, double? AddDiscount, PaymentTypeMaybeCredit type,
//        //           string? remarks, string? Name,
//        //           string? PhoneNumber, string? Address, int? BankId, CreateBillPaymentCredit_DTO? createCustomerBillDTO)
//        //        {
//        //            try
//        //            {
//        //                //string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();

//        //                var CustomerData = new CustomerDataDto();
//        //                if (Name != null)
//        //                {
//        //                    CustomerData.Name = Name;
//        //                    CustomerData.PhoneNumber = PhoneNumber;
//        //                    CustomerData.Address = Address;
//        //                }

//        //                var retData = await _craftOrderService.MakePayment(id, remarks, CustomerData, AddDiscount, type);

//        //                string salesRemark = "Sales - " + remarks;
//        //                if (type == PaymentTypeMaybeCredit.Cash)
//        //                {
//        //                    await _counterCashTransactionService.AddCashAsync(token, (decimal)retData.PaidAmount, salesRemark);
//        //                }

//        //                if (type == PaymentTypeMaybeCredit.Credit)
//        //                {
//        //                    if (Name == null || PhoneNumber == null || Address == null)
//        //                    {
//        //                        throw new Exception("Invalid customer data provided. please fill all fields !!!");
//        //                    }

//        //                    if (createCustomerBillDTO == null)
//        //                    {
//        //                        throw new Exception("Bill information required");
//        //                    }

//        //                    if (createCustomerBillDTO.MenuOrderId == 0 || createCustomerBillDTO.PaidAmount < 0)
//        //                    {
//        //                        throw new Exception("Invalid Bill Data");
//        //                    }

//        //                    var customerData = new CreateBillPaymentCredit_DTO()
//        //                    {
//        //                        FirstName = Name,
//        //                        Address = Address,
//        //                        PhoneNo = PhoneNumber,
//        //                        MenuOrderId = createCustomerBillDTO.MenuOrderId,
//        //                        TotalBillAmount = (decimal)retData.PaidAmount,
//        //                        PaidAmount = createCustomerBillDTO.PaidAmount,
//        //                        PaymentMethod = createCustomerBillDTO.PaymentMethod,
//        //                        CreditLimit = createCustomerBillDTO.CreditLimit,
//        //                        BankId = createCustomerBillDTO.BankId
//        //                    };

//        //                    await _billPaymentCreditService.CreateBillPaymentCredit(customerData, token);
//        //                }

//        //                int finalBankId = BankId ?? 0;
//        //                if (type == PaymentTypeMaybeCredit.Bank && finalBankId != 0)
//        //                {
//        //                    var insertBankData = new BankDepositDto()
//        //                    {
//        //                        BankId = finalBankId,
//        //                        DepositedBy = GeneralUtility.GetCurrentUserName(_httpContextAccessor),
//        //                        Amount = (decimal)retData.PaidAmount,
//        //                        Remarks = salesRemark,
//        //                        ValueDate = GeneralUtility.GetCurrentDateTime()
//        //                    };
//        //                    await _bankService.CreateTransactionAsync(insertBankData);
//        //                }

//        //                return Ok("updated status successfully");
//        //            }
//        //            catch (ArgumentException ex)
//        //            {
//        //                _logger.LogError(ex, $"Error occurred for CustomerOnCredit : {ex.Message}");
//        //                return BadRequest(new { message = ex.Message });
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                _logger.LogError($"Error occurred  for CustomerOnCredit : {ex.Message}", ex);
//        //                return StatusCode(500, new { message = $"An error occurred while updating  for CustomerOnCredit : {ex.Message}" });
//        //            }
//        //        }

//        //        [HttpPatch("Billing/Repayment")]
//        //        public async Task<IActionResult> PayingOffCredit([FromBody] PayingOffCreditDTO dto)
//        //        {
//        //            try
//        //            {
//        //                string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
//        //                if (dto.PayingAmount < 0.0m || dto.PaymentMethod <= 0 || string.IsNullOrWhiteSpace(dto.PhoneNo))
//        //                {
//        //                    throw new Exception("Invalid Input !!");
//        //                }
//        //                var result1 = await _billPaymentCreditService.PayingOffOnCredit(dto, token);
//        //                return Ok("Repayment done successfully");
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                return BadRequest(ex.Message);
//        //            }
//        //        }


//        //    }
//        //}

//        //    }

//        //while taking order , it is already making customer info so you dont need to bother to make customer here but has to
//        //take customer id

//        [HttpPatch("Billing/{id}/Payment")]
//        public async Task<IActionResult> Payment(int id, PaymentTypeMaybeCredit type,
//               int CustomerId, int? BankId, [FromBody] BuyPaymentDTO bought , CreateBillPaymentCredit_DTO? createCustomerBillDTO)
//        {
//            try
//            {
//                var craftsOrderInfo = await _craftsOrderRepo.GetInfoByCraftsOrder(bought.CraftsOrderId);
                
//                if (type == PaymentTypeMaybeCredit.Cash)//full payment
//                {
//                    await _service.AddAsync(bought);
//                }

//                if (type == PaymentTypeMaybeCredit.Credit)
//                {
//                    //if (Name == null || PhoneNumber == null || Address == null)
//                    //{
//                    //    throw new Exception("Invalid customer data provided. please fill all fields !!!");
//                    //}

//                    if (createCustomerBillDTO == null)
//                    {
//                        throw new Exception("Bill information required");
//                    }

//                    if (createCustomerBillDTO.CraftsOrderId == 0 || createCustomerBillDTO.PaidAmount < 0)
//                    {
//                        throw new Exception("Invalid Bill Data");
//                    }

//                    var craftsOnCreditData = new BillPaymentCredit()
//                    {
//                        //FirstName = Name,
//                        //Address = Address,
//                        //PhoneNo = PhoneNumber,
//                        CustomerId=bought.CustomerId,
//                        CraftsOrderId = bought.CraftsOrderId,
//                        BillAmount = craftsOrderInfo.Price,
//                        PaidAmount = bought.PaidAmount,
//                        PaymentReceivable = bought.PaymentReceivable,
//                        PaymentMethod = bought.PaymentMethod,
//                        CreditLimit = bought.CreditLimit,
//                        //BankId = bought.BankId
//                    };

//                    await _billPaymentCreditService.CreateBillPaymentCredit(craftsOnCreditData);
//                }

//                //int finalBankId = BankId ?? 0;
//                //if (type == PaymentTypeMaybeCredit.Bank && finalBankId != 0)
//                //{
//                //    var insertBankData = new BankDepositDto()
//                //    {
//                //        BankId = finalBankId,
//                //        DepositedBy = GeneralUtility.GetCurrentUserName(_httpContextAccessor),
//                //        Amount = (decimal)retData.PaidAmount,
//                //        Remarks = salesRemark,
//                //        ValueDate = GeneralUtility.GetCurrentDateTime()
//                //    };
//                //    await _bankService.CreateTransactionAsync(insertBankData);
//                //}

//                return Ok("updated status successfully");
//            }
//            catch (ArgumentException ex)
//            {
//                _logger.LogError(ex, $"Error occurred for CustomerOnCredit : {ex.Message}");
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error occurred  for CustomerOnCredit : {ex.Message}", ex);
//                return StatusCode(500, new { message = $"An error occurred while updating  for CustomerOnCredit : {ex.Message}" });
//            }
//        }

//        [HttpPatch("Billing/Repayment")]
//        public async Task<IActionResult> PayingOffCredit([FromBody] PayingOffCreditDTO dto)
//        {
//            try
//            {
//                string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
//                if (dto.PayingAmount < 0.0m || dto.PaymentMethod <= 0 || string.IsNullOrWhiteSpace(dto.PhoneNo))
//                {
//                    throw new Exception("Invalid Input !!");
//                }
//                var result1 = await _billPaymentCreditService.PayingOffOnCredit(dto, token);
//                return Ok("Repayment done successfully");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }


//    }
//}

