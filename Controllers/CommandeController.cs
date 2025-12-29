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

        public IActionResult Panier()
        {
            var items = PanierHelper.GetPanier(HttpContext.Session);
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

                Console.WriteLine($"✅ Livraison: Zone {idZone}, Frais {fraisLivraison} FCFA");
            }

            double sousTotal = items.Sum(i => i.Total);
            double montantTotal = sousTotal + fraisLivraison;

            Console.WriteLine($"📊 Sous-total: {sousTotal}, Frais: {fraisLivraison}, Total: {montantTotal}");

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
                Console.WriteLine($"✅ Commande créée: ID {commandeCreee.Id}");

                if (!string.IsNullOrEmpty(adresse))
                {
                    var client = await _clientService.TrouverParIdAsync(clientId.Value);
                    if (client != null)
                    {
                        client.Adresse = adresse;
                        Console.WriteLine($"✅ Adresse sauvegardée: {adresse}");
                    }
                }

                foreach (var item in items.Where(i => i.Type == "burger"))
                {
                    var commandeBurger = new CommandeBurger
                    {
                        IdCommande = commandeCreee.Id,
                        IdBurger = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    };
                    await _commandeService.AjouterCommandeBurgerAsync(commandeBurger);
                }

                foreach (var item in items.Where(i => i.Type == "menu"))
                {
                    var commandeMenu = new CommandeMenu
                    {
                        IdCommande = commandeCreee.Id,
                        IdMenu = item.Id,
                        Quantite = item.Quantite,
                        PrixUnitaire = item.Prix
                    };
                    await _commandeService.AjouterCommandeMenuAsync(commandeMenu);
                }

                PanierHelper.ViderPanier(HttpContext.Session);
                return RedirectToAction("Paiement", "Paiement", new { commandeId = commandeCreee.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur: {ex.Message}");
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
            var viewModel = new MesCommandesViewModel
            {
                Commandes = new List<CommandeDetailViewModel>()
            };

            foreach (var cmd in commandes)
            {
                var commandeDetail = new CommandeDetailViewModel
                {
                    Id = cmd.Id,
                    Date = cmd.Date,
                    EtatCmd = cmd.EtatCmd,
                    MontantTotal = cmd.MontantTotal,
                    LieuConsommation = cmd.LieuConsommation,
                    Items = new List<ItemPanier>()
                };

                var burgers = await _commandeService.GetBurgersCommandeAsync(cmd.Id);
                foreach (var cb in burgers)
                {
                    var burger = await _burgerService.TrouverParIdAsync(cb.IdBurger);
                    if (burger != null)
                    {
                        commandeDetail.Items.Add(new ItemPanier
                        {
                            Id = burger.Id,
                            Type = "burger",
                            Nom = burger.Nom,
                            Prix = cb.PrixUnitaire,
                            Quantite = cb.Quantite,
                            UrlImage = burger.UrlImage
                        });
                    }
                }

                var menus = await _commandeService.GetMenusCommandeAsync(cmd.Id);
                foreach (var cm in menus)
                {
                    var menu = await _menuService.TrouverParIdAsync(cm.IdMenu);
                    if (menu != null)
                    {
                        commandeDetail.Items.Add(new ItemPanier
                        {
                            Id = menu.Id,
                            Type = "menu",
                            Nom = menu.Nom,
                            Prix = cm.PrixUnitaire,
                            Quantite = cm.Quantite,
                            UrlImage = menu.UrlImage
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
