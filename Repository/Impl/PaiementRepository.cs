using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class PaiementRepository : IPaiementRepository
    {
        private readonly AppDbContext _context;

        public PaiementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Paiement> CreerAsync(Paiement paiement)
        {
            _context.Paiements.Add(paiement);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Paiement créé avec l'ID : {paiement.Id}");
            return paiement;
        }

        public async Task<Paiement?> TrouverParCommandeIdAsync(int commandeId)
        {
            return await _context.Paiements
                .Where(p => p.IdCommande == commandeId)
                .FirstOrDefaultAsync();
        }
    }
}
