using PersonalERP.Entity;

namespace PersonalERP.Service
{
    public interface IArtPieceService
    {
        Task<List<ArtPiece>> GetAllAsync();
        Task<ArtPiece> GetByIdAsync(int id);
        Task<ArtPiece> AddAsync(ArtPiece artPiece);
        Task<ArtPiece> UpdateAsync(ArtPiece artPiece);
        Task<bool> DeleteAsync(int id);
    }
}
