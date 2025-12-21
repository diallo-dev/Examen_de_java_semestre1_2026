using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.Entity;
using BrasilBurger.Web.ViewModels;

namespace BrasilBurger.Web.Controllers
{
    public class PaiementController : Controller
    {
        private readonly IPaiementService _paiementService;
        private readonly ICommandeService _commandeService;

        public PaiementController(
            IPaiementService paiementService,
            ICommandeService commandeService)
        {
            _paiementService = paiementService;
            _commandeService = commandeService;
        }

        // POST: /Paiement/Traiter
        [HttpPost]
        public async Task<IActionResult> Traiter(PaiementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Paiement", model);
            }

            // Vérifier que la commande existe
            var commande = await _commandeService.TrouverCommandeParIdAsync(model.IdCommande);
            if (commande == null)
            {
                return NotFound();
            }

            // Vérifier que la commande n'est pas déjà payée
            var dejaPayee = await _paiementService.VerifierPaiementCommandeAsync(model.IdCommande);
            if (dejaPayee)
            {
                TempData["ErrorMessage"] = "Cette commande a déjà été payée";
                return RedirectToAction("MesCommandes", "Commande");
            }

            // Créer le paiement (simulation)
            var paiement = new Paiement
            {
                IdCommande = model.IdCommande,
                Montant = model.Montant,
                Mode = model.ModePaiement,
                Date = DateTime.Now
            };

            await _paiementService.CreerPaiementAsync(paiement);

            TempData["SuccessMessage"] = $"Paiement de {model.Montant} FCFA effectué avec succès via {model.ModePaiement} !";
            return RedirectToAction("Confirmation", new { id = model.IdCommande });
        }

        // GET: /Paiement/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var commande = await _commandeService.TrouverCommandeParIdAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }
    }
}
