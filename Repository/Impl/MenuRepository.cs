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

                // Association burger
                if (menu.Burger != null)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO menu_burger (id_menu, id_burger, quantite) VALUES ({0}, {1}, {2})",
                        menuId, menu.Burger.Id, 1
                    );
                }

                // Association compléments
                if (menu.Complements != null && menu.Complements.Any())
                {
                    foreach (var complement in menu.Complements)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO menu_complement (id_menu, id_complement, quantite) VALUES ({0}, {1}, {2})",
                            menuId, complement.Id, 1
                        );
                    }
                }

                await transaction.CommitAsync();
                return menu;
            }
            catch
            {
                await transaction.RollbackAsync();
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
            // Convert string to enum C# (ignore case)
            if (!Enum.TryParse<EtatStockEnum>(etat, true, out var etatEnum))
                return new List<Menu>();

            var menus = await _context.Menus
                .Where(m => m.EtatStock == etatEnum)  // comparer enum avec enum
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
                .Where(mb => mb.IdMenu == menuId)
                .Select(mb => mb.IdBurger)
                .FirstOrDefaultAsync();

            if (burgerId != 0)
                return await _context.Burgers.FindAsync(burgerId);

            return null;
        }

        public async Task<List<Complement>> GetComplementsByMenuIdAsync(int menuId)
        {
            var complementIds = await _context.MenuComplements
                .Where(mc => mc.IdMenu == menuId)
                .Select(mc => mc.IdComplement)
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
