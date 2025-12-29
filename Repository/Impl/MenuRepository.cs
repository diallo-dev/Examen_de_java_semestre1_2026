using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Menu> CreerAsync(Menu menu)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();
                
                int menuId = menu.Id;
                Console.WriteLine($"✅ Menu créé avec l'ID : {menuId}");
                
                if (menu.Burger != null)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO menu_burger (id_menu, id_burger, quantite) VALUES ({0}, {1}, {2})",
                        menuId, menu.Burger.Id, 1
                    );
                    Console.WriteLine("✅ Burger associé au menu");
                }
                
                if (menu.Complements != null && menu.Complements.Any())
                {
                    foreach (var complement in menu.Complements)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO menu_complement (id_menu, id_complement, quantite) VALUES ({0}, {1}, {2})",
                            menuId, complement.Id, 1
                        );
                    }
                    Console.WriteLine($"✅ {menu.Complements.Count} complément(s) associé(s)");
                }
                
                await transaction.CommitAsync();
                return menu;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Erreur création menu: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Menu>> ListerTousAsync()
        {
            var menus = await _context.Menus.ToListAsync();
            
            foreach (var menu in menus)
            {
                menu.Burger = await GetBurgerByMenuIdAsync(menu.Id);
                menu.Complements = await GetComplementsByMenuIdAsync(menu.Id);
            }
            
            return menus;
        }

        public async Task<Menu?> TrouverParIdAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            
            if (menu != null)
            {
                menu.Burger = await GetBurgerByMenuIdAsync(menu.Id);
                menu.Complements = await GetComplementsByMenuIdAsync(menu.Id);
            }
            
            return menu;
        }

        public async Task<List<Menu>> ListerParEtatAsync(string etat)
        {
            var menus = await _context.Menus
                .FromSqlRaw("SELECT * FROM menu WHERE etatstock = {0}::etatstock ORDER BY id", etat)
                .ToListAsync();
            
            foreach (var menu in menus)
            {
                menu.Burger = await GetBurgerByMenuIdAsync(menu.Id);
                menu.Complements = await GetComplementsByMenuIdAsync(menu.Id);
            }
            
            return menus;
        }

        public async Task<Burger?> GetBurgerByMenuIdAsync(int menuId)
        {
            try
            {
                var burgers = await _context.Burgers
                    .FromSqlRaw(@"
                        SELECT b.* 
                        FROM burger b
                        INNER JOIN menu_burger mb ON b.id = mb.id_burger
                        WHERE mb.id_menu = {0}
                        LIMIT 1
                    ", menuId)
                    .ToListAsync();
                
                return burgers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erreur GetBurgerByMenuIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Complement>> GetComplementsByMenuIdAsync(int menuId)
        {
            try
            {
                var complements = await _context.Complements
                    .FromSqlRaw(@"
                        SELECT c.* 
                        FROM complement c
                        INNER JOIN menu_complement mc ON c.id = mc.id_complement
                        WHERE mc.id_menu = {0}
                        ORDER BY mc.id
                    ", menuId)
                    .ToListAsync();
                
                return complements;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erreur GetComplementsByMenuIdAsync: {ex.Message}");
                return new List<Complement>();
            }
        }
    }
}
