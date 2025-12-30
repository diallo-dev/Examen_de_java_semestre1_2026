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


        public async Task<IActionResult> Paiement(int commandeId)
        {
            var commande = await _commandeService.TrouverCommandeParIdAsync(commandeId);
            
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

        [HttpPost]
        public async Task<IActionResult> Traiter(PaiementViewModel model)
        {
            var commande = await _commandeService.TrouverCommandeParIdAsync(model.IdCommande);
            if (commande == null)
            {
                return NotFound();
            }

            var dejaPayee = await _paiementService.VerifierPaiementCommandeAsync(model.IdCommande);
            if (dejaPayee)
            {
                TempData["ErrorMessage"] = "Cette commande a déjà été payée";
                return RedirectToAction("MesCommandes", "Commande");
            }

            var paiement = new Paiement
            {
                IdCommande = model.IdCommande,
                Montant = model.Montant,
                Mode = model.ModePaiement ?? "Mobile Money",
                Date = DateTime.Now
            };

            await _paiementService.CreerPaiementAsync(paiement);

            TempData["SuccessMessage"] = $"Paiement de {model.Montant} FCFA effectué avec succès !";
            
            return RedirectToAction("Confirmation", new { id = model.IdCommande });
        }

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