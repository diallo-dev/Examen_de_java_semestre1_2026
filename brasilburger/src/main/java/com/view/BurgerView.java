package com.view;

import com.example.entity.Burger;
import com.service.BurgerService;
import com.service.ImageService;

import java.io.IOException;     // <-- NOUVEL IMPORT

import java.sql.SQLException;
import java.util.List;
import java.util.Scanner;



public class BurgerView {

    private BurgerService service;
    private ImageService imageService; // <-- DÉCLARATION
    private Scanner scanner;

    public BurgerView(BurgerService service) {
        this.service = service;
        this.imageService = new ImageService(); // <-- INITIALISATION
        this.scanner = new java.util.Scanner(System.in);
    }

    public void afficherMenu() throws SQLException {
        while (true) {
            System.out.println("\n=== MENU BURGER ===");
            System.out.println("1. Lister tous les burgers");
            System.out.println("2. Créer un burger");
            System.out.println("3. Modifier un burger");
            System.out.println("4. Archiver un burger");
            System.out.println("5. Retour");
            System.out.print("Choix : ");
            
            String input = scanner.nextLine();
            if (input.isEmpty()) continue;
            
            try {
                int choix = Integer.parseInt(input);
                switch (choix) {
                    case 1 -> listerBurgers();
                    case 2 -> creerBurger();
                    case 3 -> modifierBurger();
                    case 4 -> archiverBurger();
                    case 5 -> { return; }
                    default -> System.out.println("Choix invalide !");
                }
            } catch (NumberFormatException e) {
                 System.out.println("Saisie invalide. Veuillez entrer un numéro.");
            } catch (SQLException e) {
                System.out.println("Erreur SQL: " + e.getMessage());
            } 
        }
    }

    private void listerBurgers() throws SQLException {
        List<Burger> burgers = service.listerTous();
        System.out.println("\n--- Liste des burgers ---");
        for (Burger b : burgers) {
            System.out.println(b);
        }
    }

    private void creerBurger() throws SQLException {
        System.out.print("Nom : ");
        String nom = scanner.nextLine();
        
        System.out.print("Prix : ");
        double prix = 0;
        try {
            prix = Double.parseDouble(scanner.nextLine());
        } catch (NumberFormatException e) {
            System.out.println("❌ Prix invalide.");
            return;
        }

        // --- NOUVELLE LOGIQUE D'UPLOAD D'IMAGE CLOUDINARY ---
        String urlImage = "";
        System.out.print("Chemin COMPLET du fichier image local (ex: /Users/votre_nom/image.jpg) : ");
        String localFilePath = scanner.nextLine();
        
        try {
            // Upload de l'image pour le burger dans le dossier 'brasil_burger/burgers'
            urlImage = imageService.uploadImage(localFilePath, "brasil_burger/burgers");
            System.out.println("✅ Image uploadée avec succès. URL : " + urlImage);
        } catch (IOException e) {
            System.out.println("❌ Erreur lors de l'upload de l'image : " + e.getMessage());
            System.out.println("Annulation de la création du burger.");
            return; // Arrêter la création si l'upload échoue
        }
        // --- FIN LOGIQUE D'UPLOAD ---
        
        System.out.print("Description : ");
        String desc = scanner.nextLine();

        // Utilisation de l'URL Cloudinary (urlImage)
        Burger burger = new Burger(nom, prix, urlImage, desc);
        service.creer(burger);
        System.out.println("✅ Burger créé avec succès !");
    }

    private void modifierBurger() throws SQLException {
        System.out.print("ID du burger à modifier : ");
        int id = 0;
        try {
            id = Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            System.out.println("❌ ID invalide.");
            return;
        }

        Burger burger = service.trouverParId(id);
        if (burger == null) {
            System.out.println("❌ Burger non trouvé !");
            return;
        }

        System.out.print("Nouveau nom (" + burger.getNom() + ") : ");
        String nom = scanner.nextLine();
        if (!nom.isEmpty()) burger.setNom(nom);

        System.out.print("Nouveau prix (" + burger.getPrix() + ") : ");
        String prixStr = scanner.nextLine();
        if (!prixStr.isEmpty()) {
            try {
                burger.setPrix(Double.parseDouble(prixStr));
            } catch (NumberFormatException e) {
                System.out.println("❌ Prix invalide. Non modifié.");
            }
        }

        // --- NOUVELLE LOGIQUE DE MODIFICATION D'IMAGE CLOUDINARY ---
        System.out.println("\nModifier l'image actuelle (" + burger.getUrl_image() + ") ? (O/N)");
        String changerImage = scanner.nextLine();

        if (changerImage.equalsIgnoreCase("O")) {
            System.out.print("Chemin COMPLET du nouveau fichier image local : ");
            String localFilePath = scanner.nextLine();
            
            try {
                
                
                String newUrl = imageService.uploadImage(localFilePath, "brasil_burger/burgers");
                burger.setUrl_image(newUrl);
                System.out.println(" Nouvelle image uploadée et URL mise à jour.");
            } catch (IOException e) {
                System.out.println(" Erreur lors de l'upload de l'image. URL non modifiée. Détails: " + e.getMessage());
            }
        }
      

        System.out.print("Nouvelle description (" + burger.getDescription() + ") : ");
        String desc = scanner.nextLine();
        if (!desc.isEmpty()) burger.setDescription(desc);

        service.modifier(burger);
        System.out.println("Burger modifié !");
    }

    private void archiverBurger() throws SQLException {
        System.out.print("ID du burger à archiver : ");
        int id = 0;
        try {
            id = Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            System.out.println(" ID invalide.");
            return;
        }
        service.archiver(id);
        System.out.println("📦 Burger archivé !");
    }
}