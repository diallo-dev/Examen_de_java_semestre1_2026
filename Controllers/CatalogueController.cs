using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.ViewModels;

namespace BrasilBurger.Web.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly IBurgerService _burgerService;
        private readonly IMenuService _menuService;
        private readonly IComplementService _complementService;

        public CatalogueController(
            IBurgerService burgerService,
            IMenuService menuService,
            IComplementService complementService)
        {
            _burgerService = burgerService;
            _menuService = menuService;
            _complementService = complementService;
        }

       
        public async Task<IActionResult> Index(string? filtre)
        {
            var burgers = await _burgerService.ListerParEtatAsync("disponible");
            var menus = await _menuService.ListerParEtatAsync("disponible");

            var viewModel = new CatalogueViewModel
            {
                Burgers = burgers,
                Menus = menus,
                Filtre = filtre
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DetailsBurger(int id)
        {
            var burger = await _burgerService.TrouverParIdAsync(id);
            if (burger == null)
            {
                return NotFound();
            }

            var complements = await _complementService.ListerParEtatAsync("disponible");

            var viewModel = new DetailsViewModel
            {
                Burger = burger,
                ComplementsDisponibles = complements
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DetailsMenu(int id)
        {
            var menu = await _menuService.TrouverParIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            var viewModel = new DetailsViewModel
            {
                Menu = menu
            };

            return View(viewModel);
        }

        public IActionResult GetPanierCount()
        {
            var panier = Helpers.PanierHelper.GetPanier(HttpContext.Session);
            return Json(new { count = panier.Sum(i => i.Quantite) });
        }
    }
}
