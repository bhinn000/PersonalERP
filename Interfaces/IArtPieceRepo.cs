using PersonalERP.Entity;


namespace PersonalERP.Interface
{
    public interface IArtPieceRepo
    {
        Task<List<ArtPiece>> GetAllAsync();
        Task<ArtPiece> GetArtPiece(int id);
        Task<ArtPiece> AddAsync(ArtPiece artPiece);
        Task<ArtPiece> UpdateAsync(ArtPiece artPiece);
        Task<bool> DeleteAsync(int id);
    }
}
