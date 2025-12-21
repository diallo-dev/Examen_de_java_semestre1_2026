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

        public CommandeController(
            ICommandeService commandeService,
            IBurgerService burgerService,
            IMenuService menuService,
            IComplementService complementService)
        {
            _commandeService = commandeService;
            _burgerService = burgerService;
            _menuService = menuService;
            _complementService = complementService;
        }

        // POST: /Commande/AjouterBurger
        [HttpPost]
        public async Task<IActionResult> AjouterBurger(int id, int quantite = 1, List<int>? complementIds = null)
        {
            var burger = await _burgerService.TrouverParIdAsync(id);
            if (burger == null)
            {
                return NotFound();
            }

            var item = new ItemPanier
            {
                Id = burger.Id,
                Type = "burger",
                Nom = burger.Nom,
                Prix = burger.Prix,
                Quantite = quantite,
                UrlImage = burger.UrlImage
            };

            // Ajouter les compléments si sélectionnés
            if (complementIds != null && complementIds.Any())
            {
                foreach (var complementId in complementIds)
                {
                    var complement = await _complementService.TrouverParIdAsync(complementId);
                    if (complement != null)
                    {
                        item.Complements.Add(complement);
                        item.Prix += complement.Prix;
                    }
                }
            }

            PanierHelper.AjouterItem(HttpContext.Session, item);

            TempData["SuccessMessage"] = $"{burger.Nom} ajouté au panier !";
            return RedirectToAction("DetailsBurger", "Catalogue", new { id });
        }

        // POST: /Commande/AjouterMenu
        [HttpPost]
        public async Task<IActionResult> AjouterMenu(int id, int quantite = 1)
        {
            var menu = await _menuService.TrouverParIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            var item = new ItemPanier
            {
                Id = menu.Id,
                Type = "menu",
                Nom = menu.Nom,
                Prix = menu.PrixTotal,
                Quantite = quantite,
                UrlImage = menu.UrlImage
            };

            PanierHelper.AjouterItem(HttpContext.Session, item);

            TempData["SuccessMessage"] = $"{menu.Nom} ajouté au panier !";
            return RedirectToAction("DetailsMenu", "Catalogue", new { id });
        }

        // GET: /Commande/Panier
        public IActionResult Panier()
        {
            var items = PanierHelper.GetPanier(HttpContext.Session);
            
            var viewModel = new PanierViewModel
            {
                Items = items
            };

            return View(viewModel);
        }

        // POST: /Commande/RetirerItem
        [HttpPost]
        public IActionResult RetirerItem(int id, string type)
        {
            PanierHelper.RetirerItem(HttpContext.Session, id, type);
            TempData["SuccessMessage"] = "Article retiré du panier";
            return RedirectToAction("Panier");
        }

        // GET: /Commande/Valider
        public IActionResult Valider()
        {
            // Vérifier si connecté
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue)
            {
                TempData["ErrorMessage"] = "Vous devez être connecté pour passer commande";
                return RedirectToAction("Connexion", "Auth");
            }

            var items = PanierHelper.GetPanier(HttpContext.Session);
            if (!items.Any())
            {
                TempData["ErrorMessage"] = "Votre panier est vide";
                return RedirectToAction("Index", "Catalogue");
            }

            var viewModel = new ValiderCommandeViewModel
            {
                Panier = new PanierViewModel { Items = items }
            };

            return View(viewModel);
        }

        // POST: /Commande/Confirmer
        [HttpPost]
        public async Task<IActionResult> Confirmer(ValiderCommandeViewModel model)
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue)
            {
                return RedirectToAction("Connexion", "Auth");
            }

            var items = PanierHelper.GetPanier(HttpContext.Session);
            if (!items.Any())
            {
                return RedirectToAction("Index", "Catalogue");
            }

            // Créer la commande
            var commande = new Commande
            {
                IdClient = clientId.Value,
                Date = DateTime.Now,
                EtatCmd = "NonTraiter",
                LieuConsommation = model.LieuConsommation,
                MontantTotal = items.Sum(i => i.Total)
            };

            // Ajouter les lignes de commande
            foreach (var item in items)
            {
                if (item.Type == "burger")
                {
                    commande.CommandeBurgers.Add(new CommandeBurger
                    {
                        IdBurger = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    });
                }
                else if (item.Type == "menu")
                {
                    commande.CommandeMenus.Add(new CommandeMenu
                    {
                        IdMenu = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    });
                }
            }

            var commandeCreee = await _commandeService.CreerCommandeAsync(commande);

            // Vider le panier
            PanierHelper.ViderPanier(HttpContext.Session);

            TempData["SuccessMessage"] = "Commande créée avec succès !";
            return RedirectToAction("Paiement", new { id = commandeCreee.Id });
        }

        // GET: /Commande/Paiement/5
        public async Task<IActionResult> Paiement(int id)
        {
            var commande = await _commandeService.TrouverCommandeParIdAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            var viewModel = new PaiementViewModel
            {
                IdCommande = commande.Id,
                Montant = commande.MontantTotal
            };

            return View(viewModel);
        }

        // GET: /Commande/MesCommandes
        public async Task<IActionResult> MesCommandes()
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (!clientId.HasValue)
            {
                return RedirectToAction("Connexion", "Auth");
            }

            var commandes = await _commandeService.ListerCommandesClientAsync(clientId.Value);

            var viewModel = new MesCommandesViewModel
            {
                Commandes = commandes.Select(c => new CommandeDetailViewModel
                {
                    Id = c.Id,
                    Date = c.Date,
                    EtatCmd = c.EtatCmd,
                    MontantTotal = c.MontantTotal,
                    LieuConsommation = c.LieuConsommation
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
