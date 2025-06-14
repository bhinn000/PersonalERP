using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;

namespace PersonalERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PayingOffCreditController : ControllerBase
    {
        private readonly IPayingOffCreditService _service;
        private readonly ILogger<PayingOffCreditController> _logger;

        public PayingOffCreditController(IPayingOffCreditService service, ILogger<PayingOffCreditController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _service.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching PayingOffCredits.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching PayingOffCredit with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PayingOffDTO credit)
        {
            if (credit == null)
                return BadRequest("Request body is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //var created = await _service.AddAsync(credit);
                //return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
                var response = await _service.AddAsync(credit);

                if (!string.IsNullOrEmpty(response.Message))
                {
                    return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, new
                    {
                        response.Data.Id,
                        response.Data.BPId,
                        response.Data.TotalBillPaid,
                        response.Data.BankId,
                        response.Data.PaymentMethod,
                        Message = response.Message
                    });
                }

                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding PayingOffCredit.");
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PayingOffDTO credit)
        {
            if (id != credit.Id) return BadRequest("ID mismatch");

            try
            {
                var updated = await _service.UpdateAsync(credit);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating PayingOffCredit with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting PayingOffCredit with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
