using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.Entity;
using BrasilBurger.Web.ViewModels;

namespace BrasilBurger.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IClientService _clientService;

        public AuthController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: /Auth/Inscription
        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }

        // POST: /Auth/Inscription
        [HttpPost]
        public async Task<IActionResult> Inscription(InscriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.MotDePasse != model.ConfirmationMotDePasse)
            {
                ModelState.AddModelError("", "Les mots de passe ne correspondent pas");
                return View(model);
            }

            // Vérifier si l'email existe déjà
            var existant = await _clientService.ConnecterAsync(model.Email, "");
            if (existant != null)
            {
                ModelState.AddModelError("", "Cet email est déjà utilisé");
                return View(model);
            }

            var client = new Client
            {
                Nom = model.Nom,
                Prenom = model.Prenom,
                Email = model.Email,
                Telephone = model.Telephone,
                Adresse = model.Adresse
            };

            await _clientService.InscrireAsync(client, model.MotDePasse);

            TempData["SuccessMessage"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
            return RedirectToAction("Connexion");
        }

        // GET: /Auth/Connexion
        [HttpGet]
        public IActionResult Connexion()
        {
            return View();
        }

        // POST: /Auth/Connexion
        [HttpPost]
        public async Task<IActionResult> Connexion(ConnexionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = await _clientService.ConnecterAsync(model.Email, model.MotDePasse);

            if (client == null)
            {
                ModelState.AddModelError("", "Email ou mot de passe incorrect");
                return View(model);
            }

            // Stocker l'ID du client en session
            HttpContext.Session.SetInt32("ClientId", client.Id);
            HttpContext.Session.SetString("ClientNom", $"{client.Prenom} {client.Nom}");

            TempData["SuccessMessage"] = $"Bienvenue {client.Prenom} !";
            return RedirectToAction("Index", "Catalogue");
        }

        // GET: /Auth/Deconnexion
        public IActionResult Deconnexion()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Vous êtes déconnecté";
            return RedirectToAction("Index", "Catalogue");
        }

        // Vérifier si l'utilisateur est connecté
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("ClientId").HasValue;
        }
    }
}
