namespace BrasilBurger.Web.Entity
{
    public class Commande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EtatCmd { get; set; }
        public double MontantTotal { get; set; }
        public string LieuConsommation { get; set; }
        public double? FraisLivraison { get; set; }
        
        
        public int IdClient { get; set; }
        public Client? Client { get; set; }
        
        public int? IdGestionnaire { get; set; }
        public int? IdLivreur { get; set; }
        public int? IdZone { get; set; }
        
        
        public List<CommandeBurger> CommandeBurgers { get; set; }
        public List<CommandeMenu> CommandeMenus { get; set; }

        public Commande()
        {
            
            Date = DateTime.UtcNow;
            EtatCmd = "NonTraiter";
            CommandeBurgers = new List<CommandeBurger>();
            CommandeMenus = new List<CommandeMenu>();
        }

        public override string ToString()
        {
            return $"Commande{{Id={Id}, Date={Date:dd/MM/yyyy}, Etat={EtatCmd}, MontantTotal={MontantTotal}}}";
        }
    }
    
    
    public class CommandeBurger
    {
        public int Id { get; set; }
        public int IdCommande { get; set; }
        public int IdBurger { get; set; }
        public int Quantite { get; set; }
        public double PrixUnitaire { get; set; }
        public Burger? Burger { get; set; }
    }

    public class CommandeMenu
    {
        public int Id { get; set; }
        public int IdCommande { get; set; }
        public int IdMenu { get; set; }
        public int Quantite { get; set; }
        public double PrixUnitaire { get; set; }
        public Menu? Menu { get; set; }
    }
}