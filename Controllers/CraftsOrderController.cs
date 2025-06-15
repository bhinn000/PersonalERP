using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalERP.DTO;
using PersonalERP.Interface;
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
        private readonly ILogger<CraftsOrderController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IBillPaymentCreditService _billPaymentCreditService;
        public CraftsOrderController(
            ICraftsOrderService craftOrderService,
            ILogger<CraftsOrderController> logger,
            IHttpContextAccessor httpContextAccessor
            //IBillPaymentCreditService billPaymentCreditService
            )
        {
            _craftOrderService = craftOrderService;
            _logger = logger;
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
                if (inputDto.ArtId <= 0)
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
    }
}


