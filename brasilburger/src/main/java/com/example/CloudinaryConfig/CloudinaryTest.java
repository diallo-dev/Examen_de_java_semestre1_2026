package com.example.CloudinaryConfig;

import com.example.config.CloudinaryConfig;
import com.service.ImageService;


public class CloudinaryTest {
    
    public static void main(String[] args) {
        System.out.println("=== TEST CLOUDINARY ===\n");
        
       
        
        System.out.println("🔍 Test 1: Initialisation de Cloudinary...");
        try {
            CloudinaryConfig.getInstance();
            System.out.println("Cloudinary initialisé avec succès\n");
        } catch (Exception e) {
            System.err.println("Erreur d'initialisation: " + e.getMessage());
            e.printStackTrace();
            return;
        }
        
        
        System.out.println("🔍 Test 2: Test de connexion...");
        boolean connected = CloudinaryConfig.testConnection();
        if (!connected) {
            System.err.println(" Impossible de se connecter à Cloudinary");
            return;
        }
        System.out.println();
        
        
        if (args.length > 0) {
            System.out.println("🔍 Test 3: Test d'upload du fichier: " + args[0]);
            try {
                ImageService imageService = new ImageService();
                String url = imageService.uploadImage(args[0], "test");
                System.out.println(" Upload réussi !");
                System.out.println("📷 URL de l'image: " + url);
            } catch (Exception e) {
                System.err.println(" Erreur d'upload: " + e.getMessage());
                e.printStackTrace();
                return;
            }
        } else {
            System.out.println("ℹ  Pour tester l'upload, relancez avec: java -jar target/brasilburger-app.jar /chemin/vers/image.jpg");
        }
        
        System.out.println("\nTOUS LES TESTS SONT PASSÉS !");
        System.out.println("Vous pouvez maintenant utiliser votre application normalement.");
    }
}
