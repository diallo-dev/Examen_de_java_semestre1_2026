using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class PaiementService : IPaiementService
    {
        private readonly IPaiementRepository _repository;

        public PaiementService(IPaiementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Paiement> CreerPaiementAsync(Paiement paiement)
        {
            return await _repository.CreerAsync(paiement);
        }

        public async Task<bool> VerifierPaiementCommandeAsync(int commandeId)
        {
            var paiement = await _repository.TrouverParCommandeIdAsync(commandeId);
            return paiement != null;
        }
    }
}
