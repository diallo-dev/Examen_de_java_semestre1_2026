# 🍔 BRASIL BURGER - VERSION FINALE CORRIGÉE

## 📋 PROJET L3 ISM - SEMESTRE 1

### ✅ Projet ASP.NET Core MVC complet avec gestion de livraison

---

## 🎯 CE QUI EST INCLUS

### ✨ **Fonctionnalités Client**
- ✅ Inscription / Connexion sécurisée (BCrypt)
- ✅ Catalogue de burgers et menus
- ✅ Ajout au panier avec compléments
- ✅ **Sélection du quartier pour livraison** 🚚
- ✅ **Calcul automatique des frais de livraison** 💰
- ✅ Choix du mode : Sur place / Emporter / Livraison
- ✅ Paiement Wave / Orange Money
- ✅ Suivi des commandes

### 🗺️ **Gestion Livraison (NOUVEAU)**
- ✅ 4 zones de livraison
- ✅ 22 quartiers de Dakar
- ✅ Frais selon la zone : 1500, 2000, 2500, 3000 FCFA
- ✅ Page dédiée : `/Livraison/Zones`
- ✅ API pour calcul des frais

---

## 📁 STRUCTURE DU PROJET

```
BrasilBurger_FINAL_CORRECTED/
├── Entity/
│   ├── Burger.cs
│   ├── Menu.cs
│   ├── Complement.cs
│   ├── Client.cs
│   ├── Commande.cs
│   ├── Paiement.cs
│   ├── Zone.cs                    ← NOUVEAU
│   └── Quartier.cs                ← NOUVEAU
├── Repository/
│   ├── IBurgerRepository.cs
│   ├── IMenuRepository.cs
│   ├── IZoneRepository.cs         ← NOUVEAU
│   ├── IQuartierRepository.cs     ← NOUVEAU
│   └── Impl/
│       ├── BurgerRepository.cs    ✅ CORRIGÉ
│       ├── MenuRepository.cs      ✅ CORRIGÉ
│       ├── ComplementRepository.cs ✅ CORRIGÉ
│       ├── ZoneRepository.cs      ← NOUVEAU
│       └── QuartierRepository.cs  ← NOUVEAU
├── Service/
│   ├── IZoneService.cs            ← NOUVEAU
│   ├── IQuartierService.cs        ← NOUVEAU
│   └── Impl/
│       ├── ZoneService.cs         ← NOUVEAU
│       └── QuartierService.cs     ← NOUVEAU
├── Controllers/
│   ├── CatalogueController.cs
│   ├── CommandeController.cs      ✅ À METTRE À JOUR
│   ├── PaiementController.cs
│   ├── AuthController.cs
│   └── LivraisonController.cs     ← NOUVEAU
├── Views/
│   ├── Catalogue/
│   ├── Commande/
│   ├── Paiement/
│   ├── Auth/
│   └── Livraison/                 ← NOUVEAU
│       └── Zones.cshtml
├── SQL/
│   └── Zones_Quartiers_Dakar.sql  ← NOUVEAU
├── Data/
│   └── AppDbContext.cs            ✅ MIS À JOUR
├── Program.cs                     ✅ MIS À JOUR
├── Dockerfile
├── INSTALLATION_GUIDE.md          ← NOUVEAU
└── README_COMPLET.md              ← CE FICHIER
```

---

## 🚀 INSTALLATION RAPIDE

### **1. Prérequis**
- .NET 8.0 SDK
- PostgreSQL (Neon)
- Compte Neon configuré

### **2. Configuration**

Modifiez `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=VOTRE_HOST;Database=VOTRE_DB;Username=VOTRE_USER;Password=VOTRE_PASS;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

### **3. Installation des données**

Connectez-vous à Neon Console et exécutez :
```sql
-- Copier tout le contenu de SQL/Zones_Quartiers_Dakar.sql
```

### **4. Lancement**

```bash
dotnet restore
dotnet build
dotnet run
```

Application accessible sur : **http://localhost:5000**

---

## 🗺️ ZONES DE LIVRAISON - DAKAR

### **Zone 1 : 1500 FCFA** (Centre-ville)
- Plateau
- Médina
- Point E
- HLM
- Colobane

### **Zone 2 : 2000 FCFA** (Zones intermédiaires)
- Sicap Liberté
- Mermoz
- Fann
- Amitié
- Sacré-Coeur
- Dieuppeul

### **Zone 3 : 2500 FCFA** (Zones éloignées)
- Ouakam
- Yoff
- Ngor
- Parcelles Assainies
- Grand Yoff

### **Zone 4 : 3000 FCFA** (Zones très éloignées)
- Almadies
- Golf Sud
- Keur Massar
- Pikine
- Guédiawaye
- Thiaroye

---

## 🔧 CORRECTIONS APPLIQUÉES

### ✅ **Erreur PostgreSQL ENUM résolue**
- BurgerRepository : Cast explicite `::etatstock`
- ComplementRepository : Cast explicite `::etatstock`
- MenuRepository : Cast explicite `::etatstock`

### ✅ **Erreur menu_burger corrigée**
- Utilisation de requêtes SQL brutes avec paramètres
- Récupération correcte des burgers et compléments

### ✅ **Gestion livraison ajoutée**
- Entités Zone et Quartier
- Repositories et Services complets
- Controller dédié
- Vues avec sélection de quartier

---

## 📱 UTILISATION

### **Côté Client**

1. **Voir les zones disponibles**
   - URL : `/Livraison/Zones`

2. **Passer une commande avec livraison**
   - Ajouter des produits au panier
   - Cliquer sur "Valider la commande"
   - Sélectionner "Livraison"
   - Choisir le quartier
   - Les frais s'ajoutent automatiquement

### **API Endpoints**

```
GET /Livraison/Zones
→ Liste toutes les zones avec quartiers

GET /Livraison/CalculerFrais?quartierId=5
→ Retourne les frais pour un quartier

GET /Livraison/QuartiersParZone?zoneId=1
→ Liste les quartiers d'une zone
```

---

## 🚀 DÉPLOIEMENT SUR RENDER

### **Configuration Render**

1. **Environment** : Docker
2. **Branch** : csharp
3. **Region** : Frankfurt (EU Central)

### **Variables d'environnement**

```
ConnectionStrings__DefaultConnection=Host=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true

ASPNETCORE_ENVIRONMENT=Production

DOTNET_EnableDiagnostics=0

DOTNET_EnableEventPipe=0
```

### **Commandes Git**

```bash
git add .
git commit -m "Version finale avec gestion livraison"
git push origin csharp
```

Render redéploie automatiquement.

---

## 🧪 TESTS

### **Test 1 : Voir les zones**
```
http://localhost:5000/Livraison/Zones
```
✅ Doit afficher 4 zones avec 22 quartiers

### **Test 2 : Commander sans livraison**
- Mode "Sur place"
- Total = Prix des produits uniquement

### **Test 3 : Commander avec livraison Zone 1**
- Quartier : Plateau
- Frais : +1500 FCFA

### **Test 4 : Commander avec livraison Zone 4**
- Quartier : Almadies
- Frais : +3000 FCFA

---

## 📚 TECHNOLOGIES UTILISÉES

- **Backend** : ASP.NET Core 8.0 MVC
- **Base de données** : PostgreSQL (Neon)
- **ORM** : Entity Framework Core 8.0
- **Frontend** : Bootstrap 5 + Font Awesome
- **Authentification** : BCrypt.Net
- **Conteneurisation** : Docker

---

## 🎓 PROJET ACADÉMIQUE

**Projet Semestre 1 - L3 ISM**  
Gestion de commandes pour le restaurant Brasil Burger

### **Livrables**
- ✅ Modélisation (UML)
- ✅ Projet Java Console
- ✅ Projet C# ASP.NET MVC (CE PROJET)
- ✅ Projet Symfony (à venir)
- ✅ Déploiement Render

---

## 📞 SUPPORT

Pour toute question, consultez :
- `INSTALLATION_GUIDE.md` - Guide détaillé d'installation
- `SQL/Zones_Quartiers_Dakar.sql` - Script de données

---

## ✅ CHECKLIST FINALE

- [x] Erreurs PostgreSQL ENUM corrigées
- [x] Erreur menu_burger corrigée
- [x] Gestion zones de livraison ajoutée
- [x] 22 quartiers de Dakar configurés
- [x] Calcul automatique des frais
- [x] Page dédiée zones
- [x] Documentation complète
- [x] Dockerfile optimisé
- [x] Prêt pour déploiement Render

---

## 🎉 RÉSULTAT

**Version finale, corrigée et prête pour la production !** 🚀

✅ Toutes les erreurs résolues  
✅ Fonctionnalités complètes  
✅ Gestion livraison intégrée  
✅ Code propre et documenté  

**Bon courage pour votre présentation ! 🍔**

---

*Brasil Burger - Les meilleurs burgers de Dakar 🍔*
