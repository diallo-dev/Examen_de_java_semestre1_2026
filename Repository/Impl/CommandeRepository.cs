using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Commande> CreerAsync(Commande commande)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                
                _context.Commandes.Add(commande);
                await _context.SaveChangesAsync();
                
                int commandeId = commande.Id;
                Console.WriteLine($" Commande créée avec l'ID : {commandeId}");
                
                
                foreach (var cb in commande.CommandeBurgers)
                {
                    cb.IdCommande = commandeId;
                    _context.CommandeBurgers.Add(cb);
                }
                
                
                foreach (var cm in commande.CommandeMenus)
                {
                    cm.IdCommande = commandeId;
                    _context.CommandeMenus.Add(cm);
                }
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return commande;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($" Erreur création commande: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Commande>> ListerParClientAsync(int clientId)
        {
            var commandes = await _context.Commandes
                .Where(c => c.IdClient == clientId)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            
            
            foreach (var cmd in commandes)
            {
                cmd.CommandeBurgers = await _context.CommandeBurgers
                    .Where(cb => cb.IdCommande == cmd.Id)
                    .ToListAsync();
                    
                cmd.CommandeMenus = await _context.CommandeMenus
                    .Where(cm => cm.IdCommande == cmd.Id)
                    .ToListAsync();
            }
            
            return commandes;
        }

        public async Task<Commande?> TrouverParIdAsync(int id)
        {
            var commande = await _context.Commandes
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (commande != null)
            {
                commande.CommandeBurgers = await _context.CommandeBurgers
                    .Where(cb => cb.IdCommande == id)
                    .ToListAsync();
                    
                commande.CommandeMenus = await _context.CommandeMenus
                    .Where(cm => cm.IdCommande == id)
                    .ToListAsync();
            }
            
            return commande;
        }

        public async Task<List<Commande>> ListerToutesAsync()
        {
            return await _context.Commandes
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }
    }
}
