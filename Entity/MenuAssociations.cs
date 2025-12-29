namespace BrasilBurger.Web.Entity
{
    public class MenuBurger
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public int IdBurger { get; set; }
        public int Quantite { get; set; }
    }

    public class MenuComplement
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public int IdComplement { get; set; }
        public int Quantite { get; set; }
    }
}
