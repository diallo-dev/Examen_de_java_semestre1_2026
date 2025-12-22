namespace BrasilBurger.Web.Entity
{
    public class Paiement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Montant { get; set; }
        public string Mode { get; set; }
        public int IdCommande { get; set; }
        
        public Commande? Commande { get; set; }

        public Paiement()
        {
            
            Date = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Paiement{{Id={Id}, Montant={Montant}, Mode={Mode}, Date={Date:dd/MM/yyyy}}}";
        }
    }
}