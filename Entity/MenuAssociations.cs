
using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Entity
{
    [Table("menu_burger")]
    public class MenuBurger
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("menu_id")]
        public int MenuId { get; set; }
        
        [Column("burger_id")]
        public int BurgerId { get; set; }
        
        [Column("quantite")]
        public int Quantite { get; set; }
    }

    [Table("menu_complement")]
    public class MenuComplement
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("menu_id")]
        public int MenuId { get; set; }
        
        [Column("complement_id")]
        public int ComplementId { get; set; }
        
        [Column("quantite")]
        public int Quantite { get; set; }
    }
}