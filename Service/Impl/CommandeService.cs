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

        public async Task<CommandeBurger> AjouterCommandeBurgerAsync(CommandeBurger commandeBurger)
        {
            return await _repository.AjouterCommandeBurgerAsync(commandeBurger);
        }

        public async Task<CommandeMenu> AjouterCommandeMenuAsync(CommandeMenu commandeMenu)
        {
            return await _repository.AjouterCommandeMenuAsync(commandeMenu);
        }

        public async Task<List<CommandeBurger>> GetBurgersCommandeAsync(int commandeId)
        {
            return await _repository.GetBurgersCommandeAsync(commandeId);
        }

        public async Task<List<CommandeMenu>> GetMenusCommandeAsync(int commandeId)
        {
            return await _repository.GetMenusCommandeAsync(commandeId);
        }

        public async Task<bool> EstPayeeAsync(int commandeId)
        {
            return await _repository.EstPayeeAsync(commandeId);
        }
    }
}
