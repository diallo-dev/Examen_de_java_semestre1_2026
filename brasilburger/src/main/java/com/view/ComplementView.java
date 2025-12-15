package com.view;

import com.example.entity.Complement;
import com.service.ComplementService;
import com.service.ImageService;

import java.io.IOException;     // <-- NOUVEL IMPORT

import java.sql.SQLException;
import java.util.List;
import java.util.Scanner;

public class ComplementView {

    private ComplementService service;
    private ImageService imageService; // <-- DÉCLARATION
    private Scanner scanner;

    public ComplementView(ComplementService service) {
        this.service = service;
        this.imageService = new ImageService(); // <-- INITIALISATION
        this.scanner = new java.util.Scanner(System.in);
    }

    public void afficherMenu() throws SQLException {
        while (true) {
            System.out.println("\n=== MENU COMPLEMENT ===");
            System.out.println("1. Lister tous les compléments");
            System.out.println("2. Créer un complément");
            System.out.println("3. Modifier un complément");
            System.out.println("4. Archiver un complément");
            System.out.println("5. Retour");
            System.out.print("Choix : ");
            
            String input = scanner.nextLine();
            if (input.isEmpty()) continue;

            try {
                int choix = Integer.parseInt(input);

                switch (choix) {
                    case 1 -> listerComplements();
                    case 2 -> creerComplement();
                    case 3 -> modifierComplement();
                    case 4 -> archiverComplement();
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

    private void listerComplements() throws SQLException {
        List<Complement> list = service.listerTous();
        System.out.println("\n--- Liste des compléments ---");
        for (Complement c : list) {
            System.out.println(c);
        }
    }

    private void creerComplement() throws SQLException {
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
            // Upload de l'image pour le complément dans le dossier 'brasil_burger/complements'
            urlImage = imageService.uploadImage(localFilePath, "brasil_burger/complements");
            System.out.println("✅ Image uploadée avec succès. URL : " + urlImage);
        } catch (IOException e) {
            System.out.println("❌ Erreur lors de l'upload de l'image : " + e.getMessage());
            System.out.println("Annulation de la création du complément.");
            return; // Arrêter la création si l'upload échoue
        }
        // --- FIN LOGIQUE D'UPLOAD ---

        // Utilisation de l'URL Cloudinary (urlImage)
        Complement complement = new Complement(nom, prix, urlImage);
        service.creer(complement);
        System.out.println("✅ Complément créé !");
    }

    private void modifierComplement() throws SQLException {
        System.out.print("ID du complément à modifier : ");
        int id = 0;
        try {
            id = Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            System.out.println("❌ ID invalide.");
            return;
        }

        Complement c = service.trouverParId(id);
        if (c == null) {
            System.out.println("❌ Complément non trouvé !");
            return;
        }

        System.out.print("Nouveau nom (" + c.getNom() + ") : ");
        String nom = scanner.nextLine();
        if (!nom.isEmpty()) c.setNom(nom);

        System.out.print("Nouveau prix (" + c.getPrix() + ") : ");
        String prixStr = scanner.nextLine();
        if (!prixStr.isEmpty()) {
            try {
                c.setPrix(Double.parseDouble(prixStr));
            } catch (NumberFormatException e) {
                System.out.println("❌ Prix invalide. Non modifié.");
            }
        }

        
        System.out.println("\nModifier l'image actuelle (" + c.getUrl_image() + ") ? (O/N)");
        String changerImage = scanner.nextLine();

        if (changerImage.equalsIgnoreCase("O")) {
            System.out.print("Chemin COMPLET du nouveau fichier image local : ");
            String localFilePath = scanner.nextLine();
            
            try {
                
                
                String newUrl = imageService.uploadImage(localFilePath, "brasil_burger/complements");
                c.setUrl_image(newUrl);
                System.out.println(" Nouvelle image uploadée et URL mise à jour.");
            } catch (IOException e) {
                System.out.println(" Erreur lors de l'upload de l'image. URL non modifiée. Détails: " + e.getMessage());
            }
        }
        
        

        service.modifier(c);
        System.out.println(" Complément modifié !");
    }

    private void archiverComplement() throws SQLException {
        System.out.print("ID du complément à archiver : ");
        int id = 0;
        try {
            id = Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            System.out.println(" ID invalide.");
            return;
        }
        service.archiver(id);
        System.out.println("📦 Complément archivé !");
    }
}