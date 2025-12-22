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

               
                if (menu.Burger != null)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO menu_burger (menu_id, burger_id, quantite) VALUES ({0}, {1}, {2})",
                        menuId, menu.Burger.Id, 1
                    );
                }

                
                if (menu.Complements != null && menu.Complements.Any())
                {
                    foreach (var complement in menu.Complements)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO menu_complement (menu_id, complement_id, quantite) VALUES ({0}, {1}, {2})",
                            menuId, complement.Id, 1
                        );
                    }
                }

                await transaction.CommitAsync();
                Console.WriteLine($" Menu créé avec l'ID : {menuId}");
                return menu;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($" Erreur création menu: {ex.Message}");
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
            
            if (!Enum.TryParse<EtatStockEnum>(etat, true, out var etatEnum))
                return new List<Menu>();

            var menus = await _context.Menus
                .Where(m => m.EtatStock == etatEnum)
                .OrderBy(m => m.Id)
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
            var burgerId = await _context.MenuBurgers
                .Where(mb => mb.MenuId == menuId)
                .Select(mb => mb.BurgerId)
                .FirstOrDefaultAsync();

            if (burgerId != 0)
                return await _context.Burgers.FindAsync(burgerId);

            return null;
        }

        public async Task<List<Complement>> GetComplementsByMenuIdAsync(int menuId)
        {
            var complementIds = await _context.MenuComplements
                .Where(mc => mc.MenuId == menuId)
                .Select(mc => mc.ComplementId)
                .ToListAsync();

            if (!complementIds.Any())
                return new List<Complement>();

            var complements = await _context.Complements
                .Where(c => complementIds.Contains(c.Id))
                .ToListAsync();

            return complements;
        }
    }
}