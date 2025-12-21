namespace BrasilBurger.Web.Entity
{
    public class Complement
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public double Prix { get; set; }
        public string? UrlImage { get; set; }
        public EtatStockEnum EtatStock { get; set; }

        public Complement()
        {
            EtatStock = EtatStockEnum.disponible;
        }

        public Complement(string nom, double prix, string? urlImage)
        {
            Nom = nom;
            Prix = prix;
            UrlImage = urlImage;
            EtatStock = EtatStockEnum.disponible;
        }

        public override string ToString()
        {
            return $"Complement{{Id={Id}, Nom='{Nom}', Prix={Prix}, EtatStock={EtatStock}}}";
        }
    }
}
