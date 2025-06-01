using PersonalERP.DTO;

namespace PersonalERP.Interface
{
    public interface IArtPieceService
    {
        Task<List<ArtPieceDTO>> GetAllAsync();
        Task<ArtPieceDTO> GetByIdAsync(int id);
        Task<ArtPieceDTO> AddAsync(CreateArtPieceDTO dto);
        Task<ArtPieceDTO> UpdateAsync(int id, UpdateArtPieceDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
