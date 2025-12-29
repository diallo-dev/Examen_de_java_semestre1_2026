namespace BrasilBurger.Web.Entity
{
    public class Menu
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string? UrlImage { get; set; }
        public string? Description { get; set; }
        public double PrixTotal { get; set; }
        public EtatStockEnum EtatStock { get; set; }

        public Burger? Burger { get; set; }
        public List<Complement> Complements { get; set; }

        public Menu()
        {
            EtatStock = EtatStockEnum.disponible;
            Complements = new List<Complement>();
        }

        public Menu(string nom, string? urlImage, string? description, Burger? burger, List<Complement>? complements)
        {
            Nom = nom;
            UrlImage = urlImage;
            Description = description;
            Burger = burger;
            Complements = complements ?? new List<Complement>();
            EtatStock = EtatStockEnum.disponible;
            PrixTotal = CalculerPrixTotal();
        }

        private double CalculerPrixTotal()
        {
            double total = 0;
            
            if (Burger != null)
            {
                total += Burger.Prix;
            }
            
            foreach (var complement in Complements)
            {
                if (complement != null)
                {
                    total += complement.Prix;
                }
            }
            
            return total;
        }

        public void RecalculerPrixTotal()
        {
            PrixTotal = CalculerPrixTotal();
        }

        public void AjouterComplement(Complement complement)
        {
            if (complement != null)
            {
                Complements.Add(complement);
                RecalculerPrixTotal();
            }
        }

        public override string ToString()
        {
            return $"Menu{{Id={Id}, Nom='{Nom}', PrixTotal={PrixTotal}, EtatStock={EtatStock}}}";
        }
    }
}
