using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Entity
{
    [Table("quartier")]
    public class Quartier
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("nom")]
        public string Nom { get; set; }
        
        [Column("id_zone")]
        public int? IdZone { get; set; }
        
        [ForeignKey("IdZone")]
        public Zone? Zone { get; set; }
    }
}
