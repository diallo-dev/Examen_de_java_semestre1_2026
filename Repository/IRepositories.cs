using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository
{
    public interface IComplementRepository : IRepository<Complement>
    {
    }

    public interface IMenuRepository : IRepository<Menu>
    {
        Task<Burger?> GetBurgerByMenuIdAsync(int menuId);
        Task<List<Complement>> GetComplementsByMenuIdAsync(int menuId);
    }

    public interface IClientRepository
    {
        Task<Client> CreerAsync(Client client);
        Task<Client?> TrouverParEmailAsync(string email);
        Task<Client?> TrouverParIdAsync(int id);
    }

    public interface ICommandeRepository
    {
        Task<Commande> CreerAsync(Commande commande);
        Task<List<Commande>> ListerParClientAsync(int clientId);
        Task<Commande?> TrouverParIdAsync(int id);
        Task<List<Commande>> ListerToutesAsync();
        Task<CommandeBurger> AjouterCommandeBurgerAsync(CommandeBurger commandeBurger);
        Task<CommandeMenu> AjouterCommandeMenuAsync(CommandeMenu commandeMenu);
        Task<List<CommandeBurger>> GetBurgersCommandeAsync(int commandeId);
        Task<List<CommandeMenu>> GetMenusCommandeAsync(int commandeId);
        Task<bool> EstPayeeAsync(int commandeId);
    }

    public interface IPaiementRepository
    {
        Task<Paiement> CreerAsync(Paiement paiement);
        Task<Paiement?> TrouverParCommandeIdAsync(int commandeId);
    }
}
