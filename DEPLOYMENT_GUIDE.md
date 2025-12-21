# 🚀 GUIDE DE DÉPLOIEMENT - BRASIL BURGER

## 📝 Checklist avant déploiement

- [ ] Code testé localement
- [ ] Base de données Neon configurée
- [ ] Fichiers .gitignore créé
- [ ] README.md à jour
- [ ] Variables d'environnement documentées

## 🔧 Étape 1 : Préparation Git

```bash
cd BrasilBurger.Web

# Initialiser Git
git init

# Ajouter tous les fichiers
git add .

# Premier commit
git commit -m "Initial commit - Brasil Burger C# MVC"

# Créer la branche csharp
git branch csharp
git checkout csharp

# Connecter au repository GitHub
git remote add origin https://github.com/VOTRE_USERNAME/VOTRE_REPO.git

# Pusher
git push -u origin csharp
```

## 🌐 Étape 2 : Déploiement sur Render

### A. Créer le Web Service

1. Aller sur [render.com](https://render.com)
2. Cliquer sur **"New +"** → **"Web Service"**
3. Connecter votre repository GitHub
4. Sélectionner la branche **csharp**

### B. Configuration du Service

**Name**: `brasil-burger-csharp`

**Environment**: `Docker` (ou .NET si disponible)

**Build Command**:
```bash
dotnet publish -c Release -o out
```

**Start Command**:
```bash
cd out && dotnet BrasilBurger.Web.dll
```

**Instance Type**: `Free`

### C. Variables d'Environnement

Ajouter ces variables dans l'onglet "Environment" :

```
ConnectionStrings__DefaultConnection=Host=ep-dawn-frog-advivkhj-pooler.us-east-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_GD5TPeqL6cYK;SSL Mode=Require;Trust Server Certificate=true

ASPNETCORE_ENVIRONMENT=Production

ASPNETCORE_URLS=http://0.0.0.0:10000

Cloudinary__CloudName=dyylpe9yy
Cloudinary__ApiKey=513196516892715
Cloudinary__ApiSecret=rRlU3V0bCXeICpwYHB_-tBfwPVs
```

### D. Déployer

Cliquer sur **"Create Web Service"**

⏳ Le déploiement prend environ 5-10 minutes.

## ✅ Étape 3 : Vérification

Une fois déployé, votre application sera disponible sur :
```
https://brasil-burger-csharp.onrender.com
```

### Tests à effectuer :

1. ✅ Page d'accueil chargée
2. ✅ Catalogue visible
3. ✅ Inscription d'un client
4. ✅ Connexion
5. ✅ Ajout au panier
6. ✅ Passer une commande
7. ✅ Paiement simulé

## 🔍 Debugging

### Logs Render

Aller dans l'onglet **"Logs"** de votre service pour voir les erreurs.

### Erreurs courantes

**1. Erreur de connexion BDD**
```
Vérifier que la chaîne de connexion est correcte
Vérifier que Neon autorise les connexions externes
```

**2. Erreur "Unable to bind to..."**
```
Ajouter ASPNETCORE_URLS=http://0.0.0.0:10000
```

**3. Erreur 502 Bad Gateway**
```
Le service ne démarre pas correctement
Vérifier les logs
Vérifier que le port 10000 est utilisé
```

## 📊 Monitoring

Render fournit :
- CPU usage
- Memory usage
- Request metrics
- Logs en temps réel

## 🔄 Mises à jour

Pour déployer des modifications :

```bash
git add .
git commit -m "Description des modifications"
git push origin csharp
```

Render redéploiera automatiquement.

## 🎯 URLs importantes

- **Application**: https://brasil-burger-csharp.onrender.com
- **Dashboard Render**: https://dashboard.render.com
- **Base de données Neon**: https://console.neon.tech

## 📧 Support

En cas de problème :
1. Vérifier les logs Render
2. Tester localement avec `dotnet run`
3. Vérifier la connexion à Neon

## ✨ Optimisations (optionnelles)

### Activer HTTPS
Render active HTTPS automatiquement.

### Custom Domain
Dans Render : Settings → Custom Domain

### Scaling
Dans Render : Settings → Instance Type (upgrade vers plan payant)

## 🎉 Félicitations !

Votre application Brasil Burger est maintenant en ligne ! 🍔

---

**Date**: Décembre 2024
**Version**: 1.0
**Auteur**: L3 ISM - Projet Semestre 1
