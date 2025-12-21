using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class CommandeService : ICommandeService
    {
        private readonly ICommandeRepository _repository;

        public CommandeService(ICommandeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Commande> CreerCommandeAsync(Commande commande)
        {
            return await _repository.CreerAsync(commande);
        }

        public async Task<List<Commande>> ListerCommandesClientAsync(int clientId)
        {
            return await _repository.ListerParClientAsync(clientId);
        }

        public async Task<Commande?> TrouverCommandeParIdAsync(int id)
        {
            return await _repository.TrouverParIdAsync(id);
        }
    }
}
