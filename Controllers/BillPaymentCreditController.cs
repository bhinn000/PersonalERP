using Microsoft.AspNetCore.Mvc;
using PersonalERP.DTO;
using PersonalERP.Interfaces;


namespace PersonalERP.Controllers
{
    [ApiController]
    [Route("api/bill-payment-credit")]
    public class BillPaymentCreditController : ControllerBase
    {
        private readonly IBillPaymentCreditService _billPaymentCreditService;
        private readonly ILogger<BillPaymentCreditController> _logger;

        public BillPaymentCreditController(IBillPaymentCreditService billPaymentCreditService, ILogger<BillPaymentCreditController> logger)
        {
            _billPaymentCreditService = billPaymentCreditService;
            _logger = logger;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllBillPaymentCredits()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
                var billPayments = await _billPaymentCreditService.GetAllBillPaymentCredits(token);
                return Ok(billPayments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all bill payment credits.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBillPaymentCreditById(int id)
        {
            try
            {
                var billPayment = await _billPaymentCreditService.GetBillPaymentCreditById(id);
                if (billPayment == null)
                    return NotFound($"Bill payment credit {id} not found");

                return Ok(billPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving bill payment credit with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> CreateBillPaymentCredit([FromBody] CreateBillPaymentCredit_DTO dto)
        //{
        //    try
        //    {
        //        string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        //        var created = await _billPaymentCreditService.CreateBillPaymentCredit(dto, token);
        //        return Ok(created);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogError(ex, "Validation error during creation");
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating bill payment credit.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBillPaymentCredit(int id, [FromBody] UpdateBillPaymentCredit_DTO dto)
        {
            string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
            try
            {
                if (id != dto.Id)
                    return BadRequest("Route ID and DTO ID do not match.");

                var updated = await _billPaymentCreditService.UpdateBillPaymentCredit(dto, token);
                if (!updated)
                    return NotFound("Bill payment credit not found");

                return Ok("Updated successfully");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating bill payment credit with ID {dto.Id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBillPaymentCredit(int id)
        {
            try
            {
                var deleted = await _billPaymentCreditService.DeleteBillPaymentCredit(id);
                if (!deleted)
                    return NotFound("Bill payment credit not found");

                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting bill payment credit with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-customers-on-credit")]
        public async Task<IActionResult> CustomerOnCredit(int id)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
                var customerOnCredit = await _billPaymentCreditService.GetCustomerOnCredit(token, id);
                //return Ok($"Customers {id} withcredit shown : {customerOnCredit}");
                return Ok(new
                {
                    Message = $"Customers {id} with credit shown",
                    Data = customerOnCredit
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching customers with  bill payment credit with ID {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
