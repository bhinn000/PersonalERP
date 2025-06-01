using PersonalERP.Entity;
using PersonalERP.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PersonalERP.Interface;

namespace PersonalERP.Service
{
    public class ArtPieceService : IArtPieceService
    {
        private readonly IArtPieceRepo _repo;
        private readonly ILogger<ArtPieceService> _logger;

        public ArtPieceService(IArtPieceRepo repo, ILogger<ArtPieceService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<ArtPiece>> GetAllAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in GetAllAsync.");
                return new List<ArtPiece>();
            }
        }

        public async Task<ArtPiece> GetByIdAsync(int id)
        {
            try
            {
                var artPiece = await _repo.GetByIdAsync(id);
                if (artPiece == null) 
                {
                    throw new KeyNotFoundException($"ArtPiece with ID {id} not found.");
                }
                return artPiece;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in GetByIdAsync for ID: {id}");
                throw;
            }
        }

        public async Task<ArtPiece> AddAsync(ArtPiece artPiece)
        {
            try
            {
                return await _repo.AddAsync(artPiece);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error in AddAsync.");
                return null;
            }
        }

        public async Task<ArtPiece> UpdateAsync(ArtPiece artPiece)
        {
            try
            {
                return await _repo.UpdateAsync(artPiece);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service error in UpdateAsync for ID: {artPiece.Id}");
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
    }
}
