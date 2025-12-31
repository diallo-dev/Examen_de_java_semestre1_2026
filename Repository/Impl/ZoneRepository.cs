using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly AppDbContext _context;

        public ZoneRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Zone> CreerAsync(Zone zone)
        {
            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();
            Console.WriteLine($" Zone créée avec l'ID : {zone.Id}");
            return zone;
        }

        public async Task<List<Zone>> ListerTousAsync()
        {
            return await _context.Zones
                .Include(z => z.Quartiers)
                .OrderBy(z => z.PrixLivraison)
                .ToListAsync();
        }

        public async Task<Zone?> TrouverParIdAsync(int id)
        {
            return await _context.Zones
                .Include(z => z.Quartiers)
                .FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task<Zone?> TrouverParQuartierId(int quartierId)
        {
            var quartier = await _context.Quartiers
                .Include(q => q.Zone)
                .FirstOrDefaultAsync(q => q.Id == quartierId);
            
            return quartier?.Zone;
        }

        public async Task<bool> SupprimerAsync(int id)
        {
            var zone = await _context.Zones.FindAsync(id);
            if (zone == null) return false;

            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Zone {id} supprimée");
            return true;
        }
    }
}
