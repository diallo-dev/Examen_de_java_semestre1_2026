# 🍔 BRASIL BURGER - GESTIONNAIRE SYMFONY 7.3

Application de gestion pour le restaurant Brasil Burger.

## ✅ FONCTIONNALITÉS IMPLÉMENTÉES

### 🔐 Authentification
- Connexion gestionnaire (matricule + mot de passe)
- Sessions sécurisées
- Déconnexion

### 🍔 Gestion Produits
- **Burgers** : Lister, Modifier, Archiver
- **Menus** : Lister, Modifier, Archiver
- **Compléments** : Lister, Modifier, Archiver
- Changement d'état (disponible/indisponible/archivé)

### 📦 Gestion Commandes
- Lister toutes les commandes
- Filtrer par : état, date, client
- Voir détails d'une commande
- **Terminer** une commande (NonTraiter → Terminer)
- **Annuler** une commande
- Regrouper par zone
- Affecter un livreur

### 📊 Statistiques
- Commandes en cours du jour
- Commandes validées du jour
- Commandes annulées du jour
- Recettes journalières
- Top 5 burgers les plus vendus
- Top 5 menus les plus vendus

---

## 🚀 INSTALLATION

### Prérequis
- PHP 8.2+
- Composer
- PostgreSQL (Neon)

### 1. Configuration Base de Données

Exécuter dans Neon Console :

```sql
-- Ajouter colonne mot_de_passe
ALTER TABLE gestionnaire 
ADD COLUMN IF NOT EXISTS mot_de_passe VARCHAR(255);

-- Étendre ENUM etatstock
ALTER TYPE etatstock ADD VALUE IF NOT EXISTS 'archiver';

-- Créer un gestionnaire de test
INSERT INTO gestionnaire (matricule, nom, prenom, mot_de_passe)
VALUES (
    'GEST001',
    'Admin',
    'Brasil',
    '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi'
);
-- Mot de passe : "password"
```

### 2. Configuration Projet

Modifier `.env` avec vos credentials Neon :

```env
DATABASE_URL="postgresql://user:password@host/neondb?sslmode=require"
```

### 3. Installation

```bash
composer install
php bin/console cache:clear
```

### 4. Lancer en local

```bash
symfony server:start
# OU
php -S localhost:8000 -t public
```

Ouvrir : http://localhost:8000

**Connexion** :
- Matricule : `GEST001`
- Mot de passe : `password`

---

## 📤 DÉPLOIEMENT SUR RENDER

### 1. Push sur GitHub

```bash
git init
git remote add origin https://github.com/VOTRE-REPO/Brasil_Burger_V2.0.git
git checkout -b symfony
git add .
git commit -m "Projet Symfony Gestionnaire complet"
git push -u origin symfony
```

### 2. Créer Web Service sur Render

- **Name** : brasil-burger-symfony
- **Environment** : Docker
- **Branch** : symfony
- **Build Command** : (vide, géré par Dockerfile)
- **Start Command** : (vide, géré par Dockerfile)

### 3. Variables d'environnement

Ajouter dans Render :

```
APP_ENV=prod
APP_SECRET=VOTRE_SECRET_ALEATOIRE
DATABASE_URL=postgresql://...
```

### 4. Déployer

Render détecte le Dockerfile et déploie automatiquement.

**URL** : https://brasil-burger-symfony.onrender.com

---

## 📁 STRUCTURE DU PROJET

```
symfony-brasil-burger/
├── config/
│   └── packages/
│       ├── doctrine.yaml
│       ├── security.yaml
│       ├── framework.yaml
│       └── twig.yaml
├── src/
│   ├── Controller/       # 6 controllers
│   ├── Entity/           # 10 entités
│   ├── Repository/       # 7 repositories
│   ├── Form/             # 3 formulaires
│   └── Kernel.php
├── templates/
│   ├── base.html.twig
│   ├── security/         # Login
│   ├── dashboard/        # Dashboard
│   ├── burger/           # Gestion burgers
│   ├── menu/             # Gestion menus
│   ├── complement/       # Gestion compléments
│   └── commande/         # Gestion commandes
├── public/
│   ├── index.php
│   └── css/style.css
├── .env
├── composer.json
├── Dockerfile
└── README.md
```

---

## 🎨 DESIGN

- **Couleur principale** : Rouge `#DC3545`
- **Framework CSS** : Bootstrap 5
- **Icons** : Font Awesome 6
- **Responsive** : Mobile-first

---

## 🧪 TESTS

### Test Login
1. Ouvrir `/login`
2. Matricule : `GEST001`
3. Mot de passe : `password`
4. ✅ Redirigé vers dashboard

### Test Modification Burger
1. Menu : Produits → Burgers
2. Cliquer "Modifier" sur un burger
3. Changer le prix
4. Enregistrer
5. ✅ Prix mis à jour en BDD

### Test Archivage Menu
1. Menu : Produits → Menus
2. Cliquer "Archiver" sur un menu
3. ✅ État changé à "archiver"

### Test Commandes
1. Menu : Commandes
2. Filtrer par état "NonTraiter"
3. Cliquer "Terminer" sur une commande
4. ✅ État changé à "Terminer"

### Test Statistiques
1. Dashboard
2. ✅ Voir les stats du jour
3. ✅ Top burgers/menus vendus

---

## 🔧 DÉPANNAGE

### Erreur connexion BDD
- Vérifier `DATABASE_URL` dans `.env`
- Tester la connexion Neon

### Erreur "column does not exist"
- Vérifier que le SQL d'init a été exécuté
- Vérifier que l'ENUM etatstock contient 'archiver'

### Page blanche
- `php bin/console cache:clear`
- Vérifier les logs : `var/log/`

---

## 📊 CONFORMITÉ CAHIER DES CHARGES

| Fonctionnalité | Statut |
|---|---|
| Authentification gestionnaire | ✅ |
| Modifier burger | ✅ |
| Modifier menu | ✅ |
| Modifier complément | ✅ |
| Archiver produits | ✅ |
| Lister commandes | ✅ |
| Filtrer commandes | ✅ |
| Annuler commande | ✅ |
| Terminer commande | ✅ |
| Regrouper par zone | ✅ |
| Affecter livreur | ✅ |
| Stats : Commandes en cours | ✅ |
| Stats : Commandes validées | ✅ |
| Stats : Recettes | ✅ |
| Stats : Burgers/Menus vendus | ✅ |
| Stats : Commandes annulées | ✅ |

---

**Version** : 1.0  
**Date** : 30/12/2025  
**Auteur** : Brasil Burger Team  
**Symfony** : 7.3.x
