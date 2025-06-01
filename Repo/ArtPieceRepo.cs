using Microsoft.EntityFrameworkCore;
using PersonalERP.Entity;
using PersonalERP.Interface;



namespace PersonalERP.Repository
{
    public class ArtPieceRepo : IArtPieceRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ArtPieceRepo> _logger;

        public ArtPieceRepo(AppDbContext context, ILogger<ArtPieceRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ArtPiece>> GetAllAsync()
        {
            try
            {
                return await _context.ArtPieces.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all art pieces.");
                return new List<ArtPiece>();
            }
        }

        public async Task<ArtPiece> GetArtPiece(int id)
        {
            try
            {
                return await _context.ArtPieces.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching art piece with ID: {id}");
                throw;
            }
        }

        public async Task<ArtPiece> AddAsync(ArtPiece artPiece)
        {
            try
            {
                _context.ArtPieces.Add(artPiece);
                await _context.SaveChangesAsync();
                return artPiece;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding art piece.");
                return null;
            }
        }

        public async Task<ArtPiece> UpdateAsync(ArtPiece artPiece)
        {
            try
            {
                _context.ArtPieces.Update(artPiece);
                await _context.SaveChangesAsync();
                return artPiece;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating art piece with ID: {artPiece.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var artPiece = await _context.ArtPieces.FindAsync(id);
                if (artPiece == null) return false;

                _context.ArtPieces.Remove(artPiece);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting art piece with ID: {id}");
                return false;
            }
        }
    }
}
