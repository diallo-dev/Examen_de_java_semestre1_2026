# 🍔 BRASIL BURGER - ASP.NET Core MVC

Projet de gestion de commandes pour le restaurant Brasil Burger.

## 📋 Technologies

- **Backend**: ASP.NET Core 8.0 MVC
- **Base de données**: PostgreSQL (Neon)
- **ORM**: Entity Framework Core 8.0
- **Frontend**: Bootstrap 5 + Font Awesome
- **Authentification**: BCrypt.Net
- **Upload d'images**: Cloudinary

## 🏗️ Architecture

```
BrasilBurger.Web/
├── Entity/              # Entités (Burger, Menu, Commande...)
├── Data/               # DbContext
├── Repository/         # Accès données
├── Service/           # Logique métier
├── Controllers/       # Contrôleurs MVC
├── Views/            # Vues Razor
├── ViewModels/       # DTOs
├── Helpers/          # Utilitaires
└── wwwroot/          # Assets statiques
```

## 🚀 Installation

### Prérequis
- .NET 8.0 SDK
- PostgreSQL (ou compte Neon)

### Étapes

1. **Cloner le projet**
```bash
git clone <votre-repo>
cd BrasilBurger.Web
```

2. **Restaurer les packages**
```bash
dotnet restore
```

3. **Configurer la base de données**
Modifier `appsettings.json` avec vos identifiants PostgreSQL :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;Username=...;Password=..."
  }
}
```

4. **Lancer l'application**
```bash
dotnet run
```

L'application sera accessible sur `https://localhost:5001`

## 📱 Fonctionnalités

### Client
- ✅ Inscription/Connexion sécurisée
- ✅ Catalogue de burgers et menus
- ✅ Ajout au panier avec compléments
- ✅ Choix du mode de consommation (Sur place/Emporter/Livraison)
- ✅ Paiement Wave/Orange Money (simulation)
- ✅ Suivi des commandes

## 🗄️ Base de Données

Le projet utilise la base de données PostgreSQL existante créée pour le projet Java.

**Tables principales :**
- `burger`, `menu`, `complement`
- `client`, `commande`, `paiement`
- `commande_burger`, `commande_menu`
- `menu_burger`, `menu_complement`

## 🔐 Authentification

- Mots de passe hashés avec **BCrypt**
- Session pour le panier et l'utilisateur connecté

## 📦 Déploiement sur Render

1. Pusher le code sur GitHub (branche `csharp`)
2. Créer un nouveau Web Service sur Render
3. Configurer :
   - **Build Command**: `dotnet publish -c Release -o out`
   - **Start Command**: `dotnet out/BrasilBurger.Web.dll`
4. Ajouter les variables d'environnement

## 👥 Auteurs

Projet L3 ISM - Semestre 1

## 📄 Licence

Projet éducatif - L3 ISM 2025
