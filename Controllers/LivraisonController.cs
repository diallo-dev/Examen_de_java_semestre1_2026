using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.ViewModels;

namespace BrasilBurger.Web.Controllers
{
    public class LivraisonController : Controller
    {
        private readonly IZoneService _zoneService;
        private readonly IQuartierService _quartierService;

        public LivraisonController(IZoneService zoneService, IQuartierService quartierService)
        {
            _zoneService = zoneService;
            _quartierService = quartierService;
        }

        // GET: /Livraison/Zones
        public async Task<IActionResult> Zones()
        {
            var zones = await _zoneService.ListerZonesAsync();
            var quartiers = await _quartierService.ListerQuartiersAsync();

            var viewModel = new LivraisonViewModel
            {
                Zones = zones,
                Quartiers = quartiers
            };

            return View(viewModel);
        }

        // GET: /Livraison/CalculerFrais?quartierId=5
        [HttpGet]
        public async Task<IActionResult> CalculerFrais(int quartierId)
        {
            var frais = await _zoneService.CalculerFraisLivraisonAsync(quartierId);
            var quartier = await _quartierService.TrouverQuartierParIdAsync(quartierId);

            return Json(new 
            { 
                success = true,
                frais = frais,
                quartier = quartier?.Nom
            });
        }

        // GET: /Livraison/QuartiersParZone?zoneId=1
        [HttpGet]
        public async Task<IActionResult> QuartiersParZone(int zoneId)
        {
            var quartiers = await _quartierService.ListerQuartiersParZoneAsync(zoneId);
            return Json(quartiers);
        }
    }
}
