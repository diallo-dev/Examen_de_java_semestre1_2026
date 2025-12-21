using Microsoft.EntityFrameworkCore;
using Npgsql;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets (Tables)
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

            // 1. Déclaration de l'Enum PostgreSQL
            modelBuilder.HasPostgresEnum<EtatStockEnum>("etatstock");

            // 🔹 Table : burger
            modelBuilder.Entity<Burger>(entity =>
            {
                entity.ToTable("burger");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nom).HasColumnName("nom").IsRequired();
                entity.Property(e => e.Prix).HasColumnName("prix").IsRequired();
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
            });

            // 🔹 Table : complement
            modelBuilder.Entity<Complement>(entity =>
            {
                entity.ToTable("complement");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nom).HasColumnName("nom");
                entity.Property(e => e.Prix).HasColumnName("prix");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
            });

            // 🔹 Table : menu
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nom).HasColumnName("nom");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.PrixTotal).HasColumnName("prix_total");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
                
                // Ignorer les propriétés calculées en C# qui ne sont pas des colonnes simples
                entity.Ignore(e => e.Burger);
                entity.Ignore(e => e.Complements);
            });

            // 🔹 Table : client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_client");
                entity.Property(e => e.Nom).HasColumnName("nom");
                entity.Property(e => e.Prenom).HasColumnName("prenom");
                entity.Property(e => e.Adresse).HasColumnName("adresse");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Telephone).HasColumnName("telephone");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.MotDePasse).HasColumnName("mot_de_passe");
            });

            // 🔹 Table : commande
            modelBuilder.Entity<Commande>(entity =>
            {
                entity.ToTable("commande");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.EtatCmd).HasColumnName("etat_cmd");
                entity.Property(e => e.MontantTotal).HasColumnName("montant_total");
                entity.Property(e => e.LieuConsommation).HasColumnName("lieu_consommation");
                entity.Property(e => e.FraisLivraison).HasColumnName("frais_livraison");
                entity.Property(e => e.IdClient).HasColumnName("id_client");
                entity.Property(e => e.IdGestionnaire).HasColumnName("id_gestionnaire");
                entity.Property(e => e.IdLivreur).HasColumnName("id_livreur");
                entity.Property(e => e.IdZone).HasColumnName("id_zone");

                // Relation avec le Client
                entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.IdClient);
                
                entity.Ignore(e => e.CommandeBurgers);
                entity.Ignore(e => e.CommandeMenus);
            });

            // 🔹 Table : commande_burger
            modelBuilder.Entity<CommandeBurger>(entity =>
            {
                entity.ToTable("commande_burger");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Property(e => e.IdBurger).HasColumnName("id_burger");
                entity.Property(e => e.Quantite).HasColumnName("quantite");
                entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
                entity.Ignore(e => e.Burger);
            });

            // 🔹 Table : commande_menu
            modelBuilder.Entity<CommandeMenu>(entity =>
            {
                entity.ToTable("commande_menu");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Property(e => e.IdMenu).HasColumnName("id_menu");
                entity.Property(e => e.Quantite).HasColumnName("quantite");
                entity.Property(e => e.PrixUnitaire).HasColumnName("prix_unitaire");
                entity.Ignore(e => e.Menu);
            });

            // 🔹 Table : paiement
            modelBuilder.Entity<Paiement>(entity =>
            {
                entity.ToTable("paiement");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Montant).HasColumnName("montant");
                entity.Property(e => e.Mode).HasColumnName("mode");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Ignore(e => e.Commande);
            });

            // 🔹 Table : menu_burger
            modelBuilder.Entity<MenuBurger>(entity =>
            {
                entity.ToTable("menu_burger");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdMenu).HasColumnName("id_menu");
                entity.Property(e => e.IdBurger).HasColumnName("id_burger");
                entity.Property(e => e.Quantite).HasColumnName("quantite");
            });

            // 🔹 Table : menu_complement
            modelBuilder.Entity<MenuComplement>(entity =>
            {
                entity.ToTable("menu_complement");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdMenu).HasColumnName("id_menu");
                entity.Property(e => e.IdComplement).HasColumnName("id_complement");
                entity.Property(e => e.Quantite).HasColumnName("quantite");
            });
        }
    }
}