using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Burger> Burgers { get; set; }
        public DbSet<Complement> Complements { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandeBurger> CommandeBurgers { get; set; }
        public DbSet<CommandeMenu> CommandeMenus { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<MenuBurger> MenuBurgers { get; set; }
        public DbSet<MenuComplement> MenuComplements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }

            
            
            
            modelBuilder.Entity<Burger>(entity =>
            {
                entity.ToTable("burger");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
            });

            
            modelBuilder.Entity<Complement>(entity =>
            {
                entity.ToTable("complement");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
            });

            
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.PrixTotal).HasColumnName("prix_total");
                entity.Ignore(e => e.Burger);
                entity.Ignore(e => e.Complements);
            });

            
                                modelBuilder.Entity<Client>(entity =>
                {
                    entity.ToTable("client");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).HasColumnName("id_client"); 
                    entity.Property(e => e.Nom).HasColumnName("nom");
                    entity.Property(e => e.Prenom).HasColumnName("prenom");    
                    entity.Property(e => e.Adresse).HasColumnName("adresse");
                    entity.Property(e => e.Type).HasColumnName("type"); 
                    entity.Property(e => e.Email).HasColumnName("email"); 
                    entity.Property(e => e.MotDePasse).HasColumnName("mot_de_passe");
                    entity.Property(e => e.Telephone).HasColumnName("telephone");
                });

           
           
            modelBuilder.Entity<Commande>(entity =>
                {
                    entity.ToTable("commande");
                    entity.HasKey(e => e.Id);

                    // MAPPING COMPLET DES COLONNES (D'après ton export Neon)
                    entity.Property(e => e.Id).HasColumnName("id"); 
                    entity.Property(e => e.IdZone).HasColumnName("id_zone"); // <-- LA CORRECTION POUR L'ERREUR ACTUELLE
                    entity.Property(e => e.Date).HasColumnName("date");
                    entity.Property(e => e.FraisLivraison).HasColumnName("frais_livraison");
                    entity.Property(e => e.MontantTotal).HasColumnName("montant_total");
                    entity.Property(e => e.IdGestionnaire).HasColumnName("id_gestionnaire");
                    entity.Property(e => e.IdClient).HasColumnName("id_client");
                    entity.Property(e => e.IdLivreur).HasColumnName("id_livreur");
                    entity.Property(e => e.EtatCmd).HasColumnName("etat_cmd");
                    entity.Property(e => e.LieuConsommation).HasColumnName("lieu_consommation");

                    // Relations
                    entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.IdClient);
                    
                    // On ignore les listes pour éviter les cycles
                    entity.Ignore(e => e.CommandeBurgers);
                    entity.Ignore(e => e.CommandeMenus);
                });
          
          
           modelBuilder.Entity<CommandeBurger>(entity =>
                {
                    entity.ToTable("commande_burger");
                    entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                    entity.Property(e => e.BurgerId).HasColumnName("id_burger");
                    entity.Property(e => e.Quantite).HasColumnName("quantite");
                    
                    // LA CORRECTION POUR L'ERREUR ACTUELLE :
                    entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire"); 
                    
                    entity.Ignore(e => e.Burger);
                });
            
            
            modelBuilder.Entity<MenuBurger>(entity => {
                entity.ToTable("menu_burger");
                entity.Property(e => e.MenuId).HasColumnName("id_menu");
                entity.Property(e => e.BurgerId).HasColumnName("id_burger");
            });

            modelBuilder.Entity<MenuComplement>(entity => {
                entity.ToTable("menu_complement");
                entity.Property(e => e.MenuId).HasColumnName("id_menu");
                entity.Property(e => e.ComplementId).HasColumnName("id_complement");
            });




             modelBuilder.Entity<CommandeMenu>(entity =>
                    {
                        entity.ToTable("commande_menu");
                        entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                        entity.Property(e => e.MenuId).HasColumnName("id_menu");
                        entity.Property(e => e.Quantite).HasColumnName("quantite");
                        
                        // On met l'underscore par précaution ici aussi
                        entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire"); 
                    });



                            modelBuilder.Entity<Paiement>(entity =>
                {
                    entity.ToTable("paiement"); 
                    entity.HasKey(e => e.Id);
                    
                    entity.Property(e => e.Id).HasColumnName("id");
                    entity.Property(e => e.Date).HasColumnName("date");
                    entity.Property(e => e.Montant).HasColumnName("montant");
                    entity.Property(e => e.Mode).HasColumnName("mode");
                    entity.Property(e => e.IdCommande).HasColumnName("id_commande");

                  
                    entity.HasOne(p => p.Commande)
                        .WithMany()
                        .HasForeignKey(p => p.IdCommande);
                });
        }
    }
}