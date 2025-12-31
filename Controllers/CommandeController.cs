using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.Entity;
using BrasilBurger.Web.ViewModels;
using BrasilBurger.Web.Helpers;

namespace BrasilBurger.Web.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ICommandeService _commandeService;
        private readonly IBurgerService _burgerService;
        private readonly IMenuService _menuService;
        private readonly IComplementService _complementService;
        private readonly IClientService _clientService;

        public CommandeController(
            ICommandeService commandeService,
            IBurgerService burgerService,
            IMenuService menuService,
            IComplementService complementService,
            IClientService clientService)
        {
            _commandeService = commandeService;
            _burgerService = burgerService;
            _menuService = menuService;
            _complementService = complementService;
            _clientService = clientService;
        }

        [HttpPost]
public async Task<IActionResult> AjouterBurger(int id, int quantite, List<int> complementIds)
{
    var burger = await _burgerService.TrouverParIdAsync(id);
    if (burger != null)
    {
        
        double prixTotalUnitaire = burger.Prix;
        List<Complement> complementsChoisis = new List<Complement>();

        
        if (complementIds != null && complementIds.Any())
        {
            foreach (var compId in complementIds)
            {
                var complement = await _complementService.TrouverParIdAsync(compId);
                if (complement != null)
                {
                    prixTotalUnitaire += complement.Prix;
                    complementsChoisis.Add(complement);
                }
            }
        }

        var item = new ItemPanier
        {
            Id = burger.Id,
            Nom = burger.Nom,
            Prix = prixTotalUnitaire,  
            Quantite = quantite > 0 ? quantite : 1,
            UrlImage = burger.UrlImage,
            Type = "burger",
            Complements = complementsChoisis  
        };

        PanierHelper.AjouterItem(HttpContext.Session, item);
    }
    return RedirectToAction("Panier");
}

        [HttpPost]
        public async Task<IActionResult> AjouterMenu(int id, int quantite)
        {
            var menu = await _menuService.TrouverParIdAsync(id);
            if (menu != null)
            {
                var item = new ItemPanier
                {
                    Id = menu.Id,
                    Nom = menu.Nom,
                    Prix = menu.PrixTotal,
                    Quantite = quantite > 0 ? quantite : 1,
                    UrlImage = menu.UrlImage,
                    Type = "menu"
                };
                PanierHelper.AjouterItem(HttpContext.Session, item);
            }
            return RedirectToAction("Panier");
        }

        [Route("Commande/Panier")]
        public IActionResult Panier()
        {
            var items = PanierHelper.GetPanier(HttpContext.Session) ?? new List<ItemPanier>();
            var viewModel = new PanierViewModel { Items = items };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RetirerItem(int id, string type)
        {
            PanierHelper.RetirerItem(HttpContext.Session, id, type);
            return RedirectToAction("Panier");
        }

        [HttpPost]
        public IActionResult ViderPanier()
        {
            PanierHelper.ViderPanier(HttpContext.Session);
            return RedirectToAction("Panier");
        }

        public async Task<IActionResult> Valider()
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue)
            {
                TempData["ErrorMessage"] = "Vous devez être connecté pour passer commande";
                return RedirectToAction("Connexion", "Auth");
            }

            var items = PanierHelper.GetPanier(HttpContext.Session);
            if (items == null || !items.Any())
            {
                TempData["ErrorMessage"] = "Votre panier est vide";
                return RedirectToAction("Index", "Catalogue");
            }

            var client = await _clientService.TrouverParIdAsync(clientId.Value);
            var viewModel = new ValiderCommandeViewModel
            {
                Panier = new PanierViewModel { Items = items },
                AdresseLivraison = client?.Adresse
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmer(ValiderCommandeViewModel model)
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue) return RedirectToAction("Connexion", "Auth");

            var items = PanierHelper.GetPanier(HttpContext.Session);
            if (items == null || !items.Any()) return RedirectToAction("Index", "Catalogue");

            double fraisLivraison = model.LieuConsommation == "Livraison" ? model.FraisLivraison : 0;
            double sousTotal = items.Sum(i => i.Total);
            double montantTotal = sousTotal + fraisLivraison;

            var commande = new Commande
            {
                IdClient = clientId.Value,
                Date = DateTime.Now,
                EtatCmd = "NonTraiter",
                LieuConsommation = model.LieuConsommation,
                MontantTotal = montantTotal,
                FraisLivraison = fraisLivraison,
                IdZone = model.IdZone
            };

            try
            {
                var commandeCreee = await _commandeService.CreerCommandeAsync(commande);

                foreach (var item in items)
                {
                    if (item.Type == "burger")
                    {
                        await _commandeService.AjouterCommandeBurgerAsync(new CommandeBurger {
                            IdCommande = commandeCreee.Id, IdBurger = item.Id, Quantite = item.Quantite, PrixUnitaire = item.Prix
                        });
                    }
                    else if (item.Type == "menu")
                    {
                        await _commandeService.AjouterCommandeMenuAsync(new CommandeMenu {
                            IdCommande = commandeCreee.Id, IdMenu = item.Id, Quantite = item.Quantite, PrixUnitaire = item.Prix
                        });
                    }
                }

                PanierHelper.ViderPanier(HttpContext.Session);
                return RedirectToAction("Paiement", "Paiement", new { commandeId = commandeCreee.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erreur: {ex.Message}";
                return RedirectToAction("Valider");
            }
        }

        public async Task<IActionResult> MesCommandes()
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue) return RedirectToAction("Connexion", "Auth");

            var commandes = await _commandeService.ListerCommandesClientAsync(clientId.Value);
            var viewModel = new MesCommandesViewModel();

            foreach (var cmd in commandes)
            {
                var detail = new CommandeDetailViewModel {
                    Id = cmd.Id, Date = cmd.Date, EtatCmd = cmd.EtatCmd, MontantTotal = cmd.MontantTotal, LieuConsommation = cmd.LieuConsommation
                };
                viewModel.Commandes.Add(detail);
            }
            return View(viewModel);
        }
    }
}