using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository
{
    public interface IQuartierRepository
    {
        Task<Quartier> CreerAsync(Quartier quartier);
        Task<List<Quartier>> ListerTousAsync();
        Task<List<Quartier>> ListerParZoneAsync(int zoneId);
        Task<Quartier?> TrouverParIdAsync(int id);
        Task<Quartier?> TrouverParNomAsync(string nom);
        Task<bool> SupprimerAsync(int id);
    }
}
