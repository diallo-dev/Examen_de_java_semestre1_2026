namespace BrasilBurger.Web.Entity
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Adresse { get; set; }
        public string? Type { get; set; }
        public string? Telephone { get; set; }
        
        // Pour l'authentification
        public string? Email { get; set; }
        public string? MotDePasse { get; set; }

        public Client()
        {
        }

        public Client(string nom, string prenom, string? adresse, string? telephone, string? email)
        {
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Type = "client";
        }

        public override string ToString()
        {
            return $"Client{{Id={Id}, Nom='{Nom}', Prenom='{Prenom}', Email='{Email}'}}";
        }
    }
}
