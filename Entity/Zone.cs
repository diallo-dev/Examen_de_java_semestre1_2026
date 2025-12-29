using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Entity
{
    [Table("zone")]
    public class Zone
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("prix_livraison")]
        public double PrixLivraison { get; set; }
        
        // Navigation properties
        public List<Quartier> Quartiers { get; set; } = new();
    }
}
