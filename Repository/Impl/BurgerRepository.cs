using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class BurgerRepository : IBurgerRepository
    {
        private readonly AppDbContext _context;

        public BurgerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Burger> CreerAsync(Burger burger)
        {
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Burger créé avec l'ID : {burger.Id}");
            return burger;
        }

        public async Task<List<Burger>> ListerTousAsync()
        {
            return await _context.Burgers.ToListAsync();
        }

        public async Task<Burger?> TrouverParIdAsync(int id)
        {
            return await _context.Burgers.FindAsync(id);
        }

        public async Task<List<Burger>> ListerParEtatAsync(string etat)
        {
            return await _context.Burgers
                .FromSqlRaw("SELECT * FROM burger WHERE etatstock = {0}::etatstock", etat)
                .ToListAsync();
        }
    }
}