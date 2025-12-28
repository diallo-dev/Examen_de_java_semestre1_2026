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

            // 1. BURGER
            modelBuilder.Entity<Burger>(entity =>
            {
                entity.ToTable("burger");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
            });

            // 2. COMPLEMENT
            modelBuilder.Entity<Complement>(entity =>
            {
                entity.ToTable("complement");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
            });

            // 3. MENU
            // 3. MENU (Correction complète des colonnes pour PostgreSQL)
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");
                entity.HasKey(e => e.Id);
                
                // On force chaque propriété en minuscules pour correspondre à Neon
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nom).HasColumnName("nom");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.PrixTotal).HasColumnName("prix_total");
                
                entity.Ignore(e => e.Burger);
                entity.Ignore(e => e.Complements);
            });

            // 4. CLIENT
                        // // 4. CLIENT (Correction de la casse pour PostgreSQL)
                modelBuilder.Entity<Client>(entity =>
                {
                    entity.ToTable("client");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).HasColumnName("id_client"); 
                    
                    // On force les noms en minuscules pour correspondre à Neon
                    entity.Property(e => e.Nom).HasColumnName("nom");
                    entity.Property(e => e.Prenom).HasColumnName("prenom");
                    entity.Property(e => e.Adresse).HasColumnName("adresse"); // <--- FIX POUR TON ERREUR ACTUELLE
                    entity.Property(e => e.Telephone).HasColumnName("telephone");
                    entity.Property(e => e.Email).HasColumnName("email");
                    entity.Property(e => e.MotDePasse).HasColumnName("mot_de_passe");
                    entity.Property(e => e.Type).HasColumnName("type");
                });

            // 5. COMMANDE (Correction ID_ZONE, ID_CLIENT, ID_GESTIONNAIRE)
            modelBuilder.Entity<Commande>(entity =>
            {
                entity.ToTable("commande");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id"); 
                entity.Property(e => e.IdZone).HasColumnName("id_zone");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.FraisLivraison).HasColumnName("frais_livraison");
                entity.Property(e => e.MontantTotal).HasColumnName("montant_total");
                entity.Property(e => e.IdGestionnaire).HasColumnName("id_gestionnaire");
                entity.Property(e => e.IdClient).HasColumnName("id_client");
                entity.Property(e => e.IdLivreur).HasColumnName("id_livreur");
                entity.Property(e => e.EtatCmd).HasColumnName("etat_cmd");
                entity.Property(e => e.LieuConsommation).HasColumnName("lieu_consommation");

                entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.IdClient);
                entity.Ignore(e => e.CommandeBurgers);
                entity.Ignore(e => e.CommandeMenus);
            });

            // 6. COMMANDE_BURGER (Correction PRIX_UNITAIRE)
            modelBuilder.Entity<CommandeBurger>(entity =>
            {
                entity.ToTable("commande_burger");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Property(e => e.BurgerId).HasColumnName("id_burger");
                entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
                entity.Ignore(e => e.Burger);
            });

            // 7. COMMANDE_MENU (Correction PRIX_UNITAIRE)
            modelBuilder.Entity<CommandeMenu>(entity =>
            {
                entity.ToTable("commande_menu");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Property(e => e.MenuId).HasColumnName("id_menu");
                entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
            });

            // 8. PAIEMENT
            modelBuilder.Entity<Paiement>(entity =>
            {
                entity.ToTable("paiement"); 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.HasOne(p => p.Commande).WithMany().HasForeignKey(p => p.IdCommande);
            });

            // 9. RELATIONS MENU
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
        }
    }
}