using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class ComplementRepository : IComplementRepository
    {
        private readonly AppDbContext _context;

        public ComplementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Complement> CreerAsync(Complement complement)
        {
            _context.Complements.Add(complement);
            await _context.SaveChangesAsync();
            Console.WriteLine($" Complément créé avec l'ID : {complement.Id}");
            return complement;
        }

        public async Task<List<Complement>> ListerTousAsync()
        {
            return await _context.Complements.ToListAsync();
        }

        public async Task<Complement?> TrouverParIdAsync(int id)
        {
            return await _context.Complements.FindAsync(id);
        }

        public async Task<List<Complement>> ListerParEtatAsync(string etat)
        {
            return await _context.Complements
                .FromSqlRaw("SELECT * FROM complement WHERE etatstock = {0}::etatstock", etat)
                .ToListAsync();
        }
    }
}