# 🚚 GUIDE D'INSTALLATION - GESTION LIVRAISON

## 📦 CE QUI A ÉTÉ AJOUTÉ

### ✅ **Nouvelles Fonctionnalités**
1. **Gestion des Zones** : 4 zones de livraison avec prix différents
2. **Gestion des Quartiers** : 22 quartiers de Dakar répartis par zones
3. **Calcul automatique des frais** de livraison
4. **Sélection du quartier** lors de la commande
5. **Page dédiée** pour voir toutes les zones

### 📁 **Nouveaux Fichiers Créés** (16 fichiers)

```
Entity/
├── Zone.cs
└── Quartier.cs

Repository/
├── IZoneRepository.cs
├── IQuartierRepository.cs
└── Impl/
    ├── ZoneRepository.cs
    └── QuartierRepository.cs

Service/
├── IZoneService.cs
├── IQuartierService.cs
└── Impl/
    ├── ZoneService.cs
    └── QuartierService.cs

Controllers/
└── LivraisonController.cs

ViewModels/
└── LivraisonViewModels.cs

Views/
├── Livraison/
│   └── Zones.cshtml
└── Commande/
    └── Valider_Avec_Livraison.cshtml

SQL/
└── Zones_Quartiers_Dakar.sql
```

---

## 🛠️ INSTALLATION (5 ÉTAPES)

### **Étape 1 : Copier les nouveaux fichiers**

Copiez tous les fichiers du dossier `BrasilBurger_Livraison` dans votre projet :

```bash
# Entity
Entity/Zone.cs
Entity/Quartier.cs

# Repository
Repository/IZoneRepository.cs
Repository/IQuartierRepository.cs
Repository/Impl/ZoneRepository.cs
Repository/Impl/QuartierRepository.cs

# Service
Service/IZoneService.cs
Service/IQuartierService.cs
Service/Impl/ZoneService.cs
Service/Impl/QuartierService.cs

# Controllers
Controllers/LivraisonController.cs

# ViewModels
ViewModels/LivraisonViewModels.cs

# Views
Views/Livraison/Zones.cshtml
Views/Commande/Valider_Avec_Livraison.cshtml (remplace l'ancien Valider.cshtml)
```

---

### **Étape 2 : Mettre à jour AppDbContext.cs**

Ouvrez `Data/AppDbContext.cs` et ajoutez :

```csharp
// Ajouter les DbSet
public DbSet<Zone> Zones { get; set; }
public DbSet<Quartier> Quartiers { get; set; }

// Dans OnModelCreating, ajouter :
modelBuilder.Entity<Zone>(entity =>
{
    entity.ToTable("zone");
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Id).HasColumnName("id");
    entity.Property(e => e.PrixLivraison).HasColumnName("prix_livraison");
    
    entity.HasMany(e => e.Quartiers)
          .WithOne(q => q.Zone)
          .HasForeignKey(q => q.IdZone)
          .OnDelete(DeleteBehavior.Cascade);
});

modelBuilder.Entity<Quartier>(entity =>
{
    entity.ToTable("quartier");
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Id).HasColumnName("id");
    entity.Property(e => e.Nom).HasColumnName("nom");
    entity.Property(e => e.IdZone).HasColumnName("id_zone");
});
```

---

### **Étape 3 : Mettre à jour Program.cs**

Ouvrez `Program.cs` et ajoutez dans la section injection de dépendances :

```csharp
// Repositories
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IQuartierRepository, QuartierRepository>();

// Services
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IQuartierService, QuartierService>();
```

---

### **Étape 4 : Mettre à jour CommandeController.cs**

Suivez les instructions dans `CommandeController_UPDATE.cs` :

1. Ajouter `IZoneService` et `IQuartierService` au constructeur
2. Modifier `Valider()` pour charger les quartiers
3. Modifier `Confirmer()` pour calculer les frais de livraison

---

### **Étape 5 : Insérer les données dans Neon**

Connectez-vous à Neon Console et exécutez le script :

```sql
-- Copier tout le contenu de SQL/Zones_Quartiers_Dakar.sql
-- et l'exécuter dans Neon Console
```

Ce script va insérer :
- **4 zones** (1500, 2000, 2500, 3000 FCFA)
- **22 quartiers** de Dakar

---

## ✅ VÉRIFICATION

### **Test 1 : Voir les zones**

Allez sur : `http://localhost:5000/Livraison/Zones`

Vous devriez voir les 4 zones avec leurs quartiers.

### **Test 2 : Commander avec livraison**

1. Ajoutez des produits au panier
2. Cliquez sur "Valider la commande"
3. Sélectionnez "Livraison"
4. Choisissez un quartier
5. **Les frais s'ajoutent automatiquement** au total

---

## 🗺️ ZONES DE DAKAR

### **Zone 1 : 1500 FCFA**
- Plateau
- Médina
- Point E
- HLM
- Colobane

### **Zone 2 : 2000 FCFA**
- Sicap Liberté
- Mermoz
- Fann
- Amitié
- Sacré-Coeur
- Dieuppeul

### **Zone 3 : 2500 FCFA**
- Ouakam
- Yoff
- Ngor
- Parcelles Assainies
- Grand Yoff

### **Zone 4 : 3000 FCFA**
- Almadies
- Golf Sud
- Keur Massar
- Pikine
- Guédiawaye
- Thiaroye

---

## 🔄 DÉPLOIEMENT SUR RENDER

Après avoir fait les modifications :

```bash
git add .
git commit -m "Ajout gestion zones et quartiers de livraison"
git push origin csharp
```

Render redéploiera automatiquement.

---

## 🎉 RÉSULTAT FINAL

✅ **Zones et quartiers** configurés  
✅ **Calcul automatique** des frais  
✅ **Sélection du quartier** à la commande  
✅ **Page dédiée** pour voir les zones  
✅ **Sans casser** le code existant  

**Principe respecté : Ouvert à l'innovation, fermé à la modification ! 🚀**

---

## 📞 SUPPORT

Si vous avez des questions, référez-vous aux fichiers :
- `AppDbContext_UPDATE.cs`
- `Program_UPDATE.cs`
- `CommandeController_UPDATE.cs`

Bon courage ! 🍔🚚
