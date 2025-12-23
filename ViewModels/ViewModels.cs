using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.ViewModels
{
    // ViewModel pour le catalogue
    public class CatalogueViewModel
    {
        public List<Burger> Burgers { get; set; } = new();
        public List<Menu> Menus { get; set; } = new();
        public string? Filtre { get; set; }
    }

    // ViewModel pour les détails d'un produit
    public class DetailsViewModel
    {
        public Burger? Burger { get; set; }
        public Menu? Menu { get; set; }
        public List<Complement> ComplementsDisponibles { get; set; } = new();
    }

    // ViewModel pour l'inscription
    public class InscriptionViewModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string ConfirmationMotDePasse { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
    }

   
    public class ConnexionViewModel
    {
        public string Email { get; set; }
        public string MotDePasse { get; set; }
    }

   
    public class ItemPanier
    {
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Nom { get; set; }
        public double Prix { get; set; }
        public int Quantite { get; set; }
        public string? UrlImage { get; set; }
        public List<Complement> Complements { get; set; } = new();
        
        public double Total => Prix * Quantite;
    }

    
    public class PanierViewModel
    {
        public List<ItemPanier> Items { get; set; } = new();
        public double Total => Items.Sum(i => i.Total);
        public int NombreItems => Items.Sum(i => i.Quantite);
    }

    
    public class ValiderCommandeViewModel
    {
        public PanierViewModel Panier { get; set; }
        public string LieuConsommation { get; set; } = "SurPlace";
        public string? AdresseLivraison { get; set; }
        public int? IdZone { get; set; }
    }

   
    public class PaiementViewModel
    {
        public int IdCommande { get; set; }
        public double Montant { get; set; }
        public string ModePaiement { get; set; } = "Wave";
        public string? NumeroTelephone { get; set; }
    }

    
    public class MesCommandesViewModel
    {
        public List<CommandeDetailViewModel> Commandes { get; set; } = new();
    }

   
    public class CommandeDetailViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EtatCmd { get; set; }
        public double MontantTotal { get; set; }
        public string LieuConsommation { get; set; }
        public List<ItemPanier> Items { get; set; } = new();
        public bool EstPayee { get; set; }
    }
}
