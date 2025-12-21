namespace BrasilBurger.Web.Entity
{
    public class Burger
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public double Prix { get; set; }
        public string? UrlImage { get; set; }
        public string? Description { get; set; }
        public EtatStockEnum EtatStock { get; set; }

        // Constructeur
        public Burger()
        {
            EtatStock = EtatStockEnum.disponible;
        }

        public Burger(string nom, double prix, string? urlImage, string? description)
        {
            Nom = nom;
            Prix = prix;
            UrlImage = urlImage;
            Description = description;
            EtatStock = EtatStockEnum.disponible;
        }

        public override string ToString()
        {
            return $"Burger{{Id={Id}, Nom='{Nom}', Prix={Prix}, EtatStock={EtatStock}}}";
        }
    }
}
