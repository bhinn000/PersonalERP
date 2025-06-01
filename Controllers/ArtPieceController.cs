using Microsoft.AspNetCore.Mvc;
using PersonalERP.Entity;
using PersonalERP.Service;

namespace PersonalERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtPieceController : ControllerBase
    {
        private readonly IArtPieceService _service;
        private readonly ILogger<ArtPieceController> _logger;

        public ArtPieceController(IArtPieceService service, ILogger<ArtPieceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var artPieces = await _service.GetAllAsync();
                return Ok(artPieces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all art pieces.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var artPiece = await _service.GetByIdAsync(id);
                //if (artPiece == null)
                //{
                //    return NotFound();
                //}
                return Ok(artPiece);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx.Message);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting art piece with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArtPiece artPiece)
        {
            if (artPiece == null)
            {
                return BadRequest("ArtPiece object is null");
            }

            try
            {
                var created = await _service.AddAsync(artPiece);
                if (created == null)
                {
                    return StatusCode(500, "Failed to create art piece");
                }
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating art piece.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArtPiece artPiece)
        {
            if (artPiece == null || artPiece.Id != id)
            {
                return BadRequest("Invalid art piece data");
            }

            try
            {
                var updated = await _service.UpdateAsync(artPiece);
                if (updated == null)
                {
                    return NotFound($"Art piece with ID {id} not found");
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating art piece with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound($"Art piece with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting art piece with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
