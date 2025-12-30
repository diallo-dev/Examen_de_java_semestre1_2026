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

        // ACCÈS AU PANIER (ROUTE FORCÉE POUR ÉVITER LES ERREURS 404)
        [Route("Commande/Panier")]
        public IActionResult Panier()
        {
            var items = PanierHelper.GetPanier(HttpContext.Session) ?? new List<ItemPanier>();
            
            var viewModel = new PanierViewModel
            {
                Items = items
            };

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
            if (!items.Any())
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

            double fraisLivraison = 0;
            int? idZone = null;
            string? adresse = null;

            if (model.LieuConsommation == "Livraison")
            {
                if (string.IsNullOrWhiteSpace(model.AdresseLivraison))
                {
                    TempData["ErrorMessage"] = "L'adresse de livraison est obligatoire";
                    return RedirectToAction("Valider");
                }

                if (!model.IdZone.HasValue)
                {
                    TempData["ErrorMessage"] = "Veuillez sélectionner votre quartier";
                    return RedirectToAction("Valider");
                }

                fraisLivraison = model.FraisLivraison;
                idZone = model.IdZone;
                adresse = model.AdresseLivraison;
            }

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
                IdZone = idZone
            };

            try
            {
                var commandeCreee = await _commandeService.CreerCommandeAsync(commande);

                if (!string.IsNullOrEmpty(adresse))
                {
                    var client = await _clientService.TrouverParIdAsync(clientId.Value);
                    if (client != null)
                    {
                        client.Adresse = adresse;
                    }
                }

                foreach (var item in items.Where(i => i.Type == "burger"))
                {
                    await _commandeService.AjouterCommandeBurgerAsync(new CommandeBurger {
                        IdCommande = commandeCreee.Id,
                        IdBurger = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    });
                }

                foreach (var item in items.Where(i => i.Type == "menu"))
                {
                    await _commandeService.AjouterCommandeMenuAsync(new CommandeMenu {
                        IdCommande = commandeCreee.Id,
                        IdMenu = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    });
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
            if (!clientId.HasValue)
            {
                return RedirectToAction("Connexion", "Auth");
            }

            var commandes = await _commandeService.ListerCommandesClientAsync(clientId.Value);
            var viewModel = new MesCommandesViewModel();

            foreach (var cmd in commandes)
            {
                var commandeDetail = new CommandeDetailViewModel
                {
                    Id = cmd.Id,
                    Date = cmd.Date,
                    EtatCmd = cmd.EtatCmd,
                    MontantTotal = cmd.MontantTotal,
                    LieuConsommation = cmd.LieuConsommation
                };

                var burgers = await _commandeService.GetBurgersCommandeAsync(cmd.Id);
                foreach (var cb in burgers)
                {
                    var burger = await _burgerService.TrouverParIdAsync(cb.IdBurger);
                    if (burger != null)
                    {
                        commandeDetail.Items.Add(new ItemPanier {
                            Id = burger.Id, Nom = burger.Nom, Prix = cb.PrixUnitaire, Quantite = cb.Quantite, UrlImage = burger.UrlImage, Type = "burger"
                        });
                    }
                }

                var menus = await _commandeService.GetMenusCommandeAsync(cmd.Id);
                foreach (var cm in menus)
                {
                    var menu = await _menuService.TrouverParIdAsync(cm.IdMenu);
                    if (menu != null)
                    {
                        commandeDetail.Items.Add(new ItemPanier {
                            Id = menu.Id, Nom = menu.Nom, Prix = cm.PrixUnitaire, Quantite = cm.Quantite, UrlImage = menu.UrlImage, Type = "menu"
                        });
                    }
                }

                commandeDetail.EstPayee = await _commandeService.EstPayeeAsync(cmd.Id);
                viewModel.Commandes.Add(commandeDetail);
            }

            return View(viewModel);
        }
    }
}
