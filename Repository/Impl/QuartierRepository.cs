using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class QuartierRepository : IQuartierRepository
    {
        private readonly AppDbContext _context;

        public QuartierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Quartier> CreerAsync(Quartier quartier)
        {
            _context.Quartiers.Add(quartier);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Quartier créé : {quartier.Nom}");
            return quartier;
        }

        public async Task<List<Quartier>> ListerTousAsync()
        {
            return await _context.Quartiers
                .Include(q => q.Zone)
                .OrderBy(q => q.Nom)
                .ToListAsync();
        }

        public async Task<List<Quartier>> ListerParZoneAsync(int zoneId)
        {
            return await _context.Quartiers
                .Where(q => q.IdZone == zoneId)
                .OrderBy(q => q.Nom)
                .ToListAsync();
        }

        public async Task<Quartier?> TrouverParIdAsync(int id)
        {
            return await _context.Quartiers
                .Include(q => q.Zone)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quartier?> TrouverParNomAsync(string nom)
        {
            return await _context.Quartiers
                .Include(q => q.Zone)
                .FirstOrDefaultAsync(q => q.Nom.ToLower() == nom.ToLower());
        }

        public async Task<bool> SupprimerAsync(int id)
        {
            var quartier = await _context.Quartiers.FindAsync(id);
            if (quartier == null) return false;

            _context.Quartiers.Remove(quartier);
            await _context.SaveChangesAsync();
            Console.WriteLine($" Quartier {quartier.Nom} supprimé");
            return true;
        }
    }
}
