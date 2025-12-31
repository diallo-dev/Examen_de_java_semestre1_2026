using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IQuartierRepository _quartierRepository;

        public ZoneService(IZoneRepository zoneRepository, IQuartierRepository quartierRepository)
        {
            _zoneRepository = zoneRepository;
            _quartierRepository = quartierRepository;
        }

        public async Task<Zone> CreerZoneAsync(double prixLivraison)
        {
            var zone = new Zone
            {
                PrixLivraison = prixLivraison
            };

            return await _zoneRepository.CreerAsync(zone);
        }

        public async Task<List<Zone>> ListerZonesAsync()
        {
            return await _zoneRepository.ListerTousAsync();
        }

        public async Task<Zone?> TrouverZoneParIdAsync(int id)
        {
            return await _zoneRepository.TrouverParIdAsync(id);
        }

        public async Task<double> CalculerFraisLivraisonAsync(int quartierId)
        {
            var quartier = await _quartierRepository.TrouverParIdAsync(quartierId);
            
            if (quartier?.Zone == null)
            {
                return 0; 
            }

            return quartier.Zone.PrixLivraison;
        }

        public async Task<bool> SupprimerZoneAsync(int id)
        {
            return await _zoneRepository.SupprimerAsync(id);
        }
    }
}
