using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalERP.DTO;
using PersonalERP.Interfaces;
using static PersonalERP.Enum;

//similar to "AddToCart"
namespace PersonalERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    //no need of talking abut payment full or half in this
    public class CraftsOrderController : ControllerBase
    {
        private readonly ICraftsOrderService _craftOrderService;
        //private readonly ICounterCashTransactionService _counterCashTransactionService;
        //private readonly IBankService _bankService;
        private readonly ILogger<CraftsOrderController> _logger;
        //private readonly AuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IBillPaymentCreditService _billPaymentCreditService;
        public CraftsOrderController(
            ICraftsOrderService craftOrderService,
            ILogger<CraftsOrderController> logger,
            //AuthService authService,
            //ICounterCashTransactionService counterCashTransactionService,
            //IBankService bankService,
            IHttpContextAccessor httpContextAccessor
            //IBillPaymentCreditService billPaymentCreditService
            )
        {
            //_authService = authService;
            _craftOrderService = craftOrderService;
            _logger = logger;
            //_counterCashTransactionService = counterCashTransactionService;
            //_bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            //_billPaymentCreditService = billPaymentCreditService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _craftOrderService.GetByIdAsync(id);
                if (order == null) return NotFound();
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching order with ID {id}");
                return StatusCode(500, "An error occurred while fetching the order.");
            }
        } 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _craftOrderService.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all orders");
                return StatusCode(500, "An error occurred while fetching orders.");
            }
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _craftOrderService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting order with ID {id}");
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }
    
    
        //this can only be done by the customer when not logged in
        //initially just a viewer , when order is made then customer is created
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateCraftsOrderDto inputDto)
        {
            // if null xaina vane check inputDto katai khali ta xaina
            try
            {
                if(inputDto.ArtId<=0)
                {
                    throw new Exception("Both Art Id is needed");
                }
                var createdOrder = await _craftOrderService.CreateAsync(inputDto);
                return CreatedAtAction(nameof(GetById), new { id = createdOrder }, new { id = createdOrder });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error occurred while creating an order.{ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating an order.{ex.Message}", ex);
                return StatusCode(500, new { message = $"An error occurred while creating the order: {ex.Message}" });
            }
        }


//        [HttpPatch("Billing/{id}/Payment")]
//        public async Task<IActionResult> Payment(int id, double? AddDiscount, PaymentTypeMaybeCredit type,
//            string? remarks, string? Name,
//            string? PhoneNumber, string? Address, int? BankId, CreateBillPaymentCredit_DTO? createCustomerBillDTO)
//        {
//            try
//            {
//                string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();

//                var CustomerData = new CustomerDataDto();
//                if (Name != null)
//                {
//                    CustomerData.Name = Name;
//                    CustomerData.PhoneNumber = PhoneNumber;
//                    CustomerData.Address = Address;
//                }

//                var retData = await _craftOrderService.MakePayment(id, remarks, CustomerData, AddDiscount, type);

//                string salesRemark = "Sales - " + remarks;
//                if (type == PaymentTypeMaybeCredit.Cash)
//                {
//                    await _counterCashTransactionService.AddCashAsync(token, (decimal)retData.PaidAmount, salesRemark);
//                }

//                if (type == PaymentTypeMaybeCredit.Credit)
//                {
//                    if (Name == null || PhoneNumber == null || Address == null)
//                    {
//                        throw new Exception("Invalid customer data provided. please fill all fields !!!");
//                    }

//                    if (createCustomerBillDTO == null)
//                    {
//                        throw new Exception("Bill information required");
//                    }

//                    if (createCustomerBillDTO.MenuOrderId == 0 || createCustomerBillDTO.PaidAmount < 0)
//                    {
//                        throw new Exception("Invalid Bill Data");
//                    }

//                    var customerData = new CreateBillPaymentCredit_DTO()
//                    {
//                        FirstName = Name,
//                        Address = Address,
//                        PhoneNo = PhoneNumber,
//                        MenuOrderId = createCustomerBillDTO.MenuOrderId,
//                        TotalBillAmount = (decimal)retData.PaidAmount,
//                        PaidAmount = createCustomerBillDTO.PaidAmount,
//                        PaymentMethod = createCustomerBillDTO.PaymentMethod,
//                        CreditLimit = createCustomerBillDTO.CreditLimit,
//                        BankId = createCustomerBillDTO.BankId
//                    };

//                    await _billPaymentCreditService.CreateBillPaymentCredit(customerData, token);
//                }

//                int finalBankId = BankId ?? 0;
//                if (type == PaymentTypeMaybeCredit.Bank && finalBankId != 0)
//                {
//                    var insertBankData = new BankDepositDto()
//                    {
//                        BankId = finalBankId,
//                        DepositedBy = GeneralUtility.GetCurrentUserName(_httpContextAccessor),
//                        Amount = (decimal)retData.PaidAmount,
//                        Remarks = salesRemark,
//                        ValueDate = GeneralUtility.GetCurrentDateTime()
//                    };
//                    await _bankService.CreateTransactionAsync(insertBankData);
//                }

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
