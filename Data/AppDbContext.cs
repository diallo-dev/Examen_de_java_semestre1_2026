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

            // 1. Mapping global : On s'assure que tout est en minuscules par défaut
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }

            // 2. Configuration spécifique (Écrase le mapping global si nécessaire)
            
            // Burger
            modelBuilder.Entity<Burger>(entity =>
            {
                entity.ToTable("burger");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.EtatStock).HasColumnName("etatstock");
            });

            // Complement
            modelBuilder.Entity<Complement>(entity =>
            {
                entity.ToTable("complement");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
            });

            // Menu
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");
                entity.Property(e => e.UrlImage).HasColumnName("url_image");
                entity.Property(e => e.PrixTotal).HasColumnName("prix_total");
                entity.Ignore(e => e.Burger);
                entity.Ignore(e => e.Complements);
            });

            // Client (IMPORTANT : Correction de id_client)
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");
                // On garde id_client si c'est vraiment le nom dans Neon
                entity.Property(e => e.Id).HasColumnName("id_client"); 
                entity.Property(e => e.MotDePasse).HasColumnName("mot_de_passe");
                entity.Property(e => e.Email).HasColumnName("email");
            });

            // Commande
            modelBuilder.Entity<Commande>(entity =>
            {
                entity.ToTable("commande");
                entity.Property(e => e.EtatCmd).HasColumnName("etat_cmd");
                entity.Property(e => e.MontantTotal).HasColumnName("montant_total");
                entity.Property(e => e.LieuConsommation).HasColumnName("lieu_consommation");
                entity.Property(e => e.FraisLivraison).HasColumnName("frais_livraison");
                
                entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.IdClient);
                entity.Ignore(e => e.CommandeBurgers);
                entity.Ignore(e => e.CommandeMenus);
            });

            // CommandeBurger
            modelBuilder.Entity<CommandeBurger>(entity =>
            {
                entity.ToTable("commande_burger");
                entity.Property(e => e.BurgerId).HasColumnName("id_burger");
                entity.Property(e => e.IdCommande).HasColumnName("id_commande");
                entity.Ignore(e => e.Burger);
            });

            // Autres tables de liaison
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