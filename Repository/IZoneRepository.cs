using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository
{
    public interface IZoneRepository
    {
        Task<Zone> CreerAsync(Zone zone);
        Task<List<Zone>> ListerTousAsync();
        Task<Zone?> TrouverParIdAsync(int id);
        Task<Zone?> TrouverParQuartierId(int quartierId);
        Task<bool> SupprimerAsync(int id);
    }
}
