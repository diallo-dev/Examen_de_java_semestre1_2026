using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Service
{
    public interface IService<T> where T : class
    {
        Task<T> CreerAsync(T entity);
        Task<List<T>> ListerTousAsync();
        Task<T?> TrouverParIdAsync(int id);
        Task<List<T>> ListerParEtatAsync(string etat);
    }

    public interface IBurgerService : IService<Burger>
    {
    }

    public interface IComplementService : IService<Complement>
    {
    }

    public interface IMenuService : IService<Menu>
    {
    }

    public interface IClientService
    {
        Task<Client> InscrireAsync(Client client, string motDePasse);
        Task<Client?> ConnecterAsync(string email, string motDePasse);
        Task<Client?> TrouverParIdAsync(int id);
    }

    public interface ICommandeService
    {
        Task<Commande> CreerCommandeAsync(Commande commande);
        Task<List<Commande>> ListerCommandesClientAsync(int clientId);
        Task<Commande?> TrouverCommandeParIdAsync(int id);
        Task<CommandeBurger> AjouterCommandeBurgerAsync(CommandeBurger commandeBurger);
        Task<CommandeMenu> AjouterCommandeMenuAsync(CommandeMenu commandeMenu);
        Task<List<CommandeBurger>> GetBurgersCommandeAsync(int commandeId);
        Task<List<CommandeMenu>> GetMenusCommandeAsync(int commandeId);
        Task<bool> EstPayeeAsync(int commandeId);
    }

    public interface IPaiementService
    {
        Task<Paiement> CreerPaiementAsync(Paiement paiement);
        Task<bool> VerifierPaiementCommandeAsync(int commandeId);
    }
}
