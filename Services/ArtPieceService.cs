using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;

namespace PersonalERP.Service
{
    public class ArtPieceService : IArtPieceService
    {
        private readonly IArtPieceRepo _repo;
        private readonly ILogger<ArtPieceService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;

        public ArtPieceService(IArtPieceRepo repo, ILogger<ArtPieceService> logger,
            IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _repo = repo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
        }

        public async Task<List<ArtPieceDTO>> GetAllAsync()
        {
            try
            {
                var entities = await _repo.GetAllAsync();
                return entities.Select(e => new ArtPieceDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Price = e.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in GetAllAsync.");
                return new List<ArtPieceDTO>();
            }
        }

        public async Task<ArtPieceDTO> GetByIdAsync(int id)
        {
            try
            {
                var artPiece = await _repo.GetArtPiece(id);
                if (artPiece == null)
                    throw new KeyNotFoundException($"ArtPiece with ID {id} not found.");

                return new ArtPieceDTO
                {
                    Id = artPiece.Id,
                    Name = artPiece.Name,
                    Description = artPiece.Description,
                    Price = artPiece.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in GetByIdAsync for ID: {id}");
                throw;
            }
        }

        public async Task<ArtPieceDTO> AddAsync(CreateArtPieceDTO dto)
        {
            try
            {
                var entity = new ArtPiece
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser"
                };

                var created = await _repo.AddAsync(entity);

                return new ArtPieceDTO
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Price = created.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in AddAsync.");
                return null;
            }
        }

        public async Task<ArtPieceDTO> UpdateAsync(int id, UpdateArtPieceDTO dto)
        {
            try
            {
                var existing = await _repo.GetArtPiece(id);
                if (existing == null)
                    return null;

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.ModifiedDate = DateTime.UtcNow;
                existing.ModifiedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";

                var updated = await _repo.UpdateAsync(existing);

                return new ArtPieceDTO
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Description = updated.Description,
                    Price = updated.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in UpdateAsync for ID: {id}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var artPiece = await _repo.GetArtPiece(id);
                if (artPiece == null)
                {
                    _logger.LogWarning($"Attempted to delete ArtPiece with ID {id} but it was not found.");
                    return false;
                }

                artPiece.DeletedDate = DateTime.UtcNow;
                artPiece.DeletedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";
                await _repo.UpdateAsync(artPiece);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in DeleteAsync for ID: {id}");
                return false;
            }
        }
    }
}
