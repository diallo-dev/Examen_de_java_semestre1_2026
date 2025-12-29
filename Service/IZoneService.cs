using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Service
{
    public interface IZoneService
    {
        Task<Zone> CreerZoneAsync(double prixLivraison);
        Task<List<Zone>> ListerZonesAsync();
        Task<Zone?> TrouverZoneParIdAsync(int id);
        Task<double> CalculerFraisLivraisonAsync(int quartierId);
        Task<bool> SupprimerZoneAsync(int id);
    }
}
