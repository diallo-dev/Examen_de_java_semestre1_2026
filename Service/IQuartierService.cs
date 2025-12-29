using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Service
{
    public interface IQuartierService
    {
        Task<Quartier> CreerQuartierAsync(string nom, int zoneId);
        Task<List<Quartier>> ListerQuartiersAsync();
        Task<List<Quartier>> ListerQuartiersParZoneAsync(int zoneId);
        Task<Quartier?> TrouverQuartierParIdAsync(int id);
        Task<Quartier?> TrouverQuartierParNomAsync(string nom);
        Task<bool> SupprimerQuartierAsync(int id);
    }
}
