using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.ViewModels
{
    public class LivraisonViewModel
    {
        public List<Zone> Zones { get; set; } = new();
        public List<Quartier> Quartiers { get; set; } = new();
    }

    public class SelectionQuartierViewModel
    {
        public int? QuartierId { get; set; }
        public string? NomQuartier { get; set; }
        public double FraisLivraison { get; set; }
    }

    public class ValiderCommandeViewModelAvecLivraison
    {
        public PanierViewModel Panier { get; set; }
        public string LieuConsommation { get; set; } = "SurPlace";
        public string? AdresseLivraison { get; set; }
        public int? QuartierId { get; set; }
        public double FraisLivraison { get; set; }
        public List<Quartier> QuartiersDisponibles { get; set; } = new();
    }
}
