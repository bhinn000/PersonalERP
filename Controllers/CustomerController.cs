
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalERP.Interface;

namespace PersonalERP.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController: ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService , ILogger<CustomerController> logger)
        {
            _customerService= customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customerDto= await _customerService.GetAllAsync();
                return Ok(customerDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while fetching all orders");
                return StatusCode(500, "An error occurred while fetching orders.");
            }
        
        } 


    }
}
