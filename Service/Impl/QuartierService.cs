using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class QuartierService : IQuartierService
    {
        private readonly IQuartierRepository _quartierRepository;

        public QuartierService(IQuartierRepository quartierRepository)
        {
            _quartierRepository = quartierRepository;
        }

        public async Task<Quartier> CreerQuartierAsync(string nom, int zoneId)
        {
            var quartier = new Quartier
            {
                Nom = nom,
                IdZone = zoneId
            };

            return await _quartierRepository.CreerAsync(quartier);
        }

        public async Task<List<Quartier>> ListerQuartiersAsync()
        {
            return await _quartierRepository.ListerTousAsync();
        }

        public async Task<List<Quartier>> ListerQuartiersParZoneAsync(int zoneId)
        {
            return await _quartierRepository.ListerParZoneAsync(zoneId);
        }

        public async Task<Quartier?> TrouverQuartierParIdAsync(int id)
        {
            return await _quartierRepository.TrouverParIdAsync(id);
        }

        public async Task<Quartier?> TrouverQuartierParNomAsync(string nom)
        {
            return await _quartierRepository.TrouverParNomAsync(nom);
        }

        public async Task<bool> SupprimerQuartierAsync(int id)
        {
            return await _quartierRepository.SupprimerAsync(id);
        }
    }
}
