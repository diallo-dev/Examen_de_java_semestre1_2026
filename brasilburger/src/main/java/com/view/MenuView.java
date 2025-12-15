package com.view;

import com.example.entity.Menu;
import com.example.entity.Burger;
import com.example.entity.Complement;
import com.example.entity.EtatStockEnum;
import com.service.MenuService;
import com.service.BurgerService;
import com.service.ComplementService;
import com.service.ImageService;

import java.io.IOException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class MenuView {

    private MenuService menuService;
    private BurgerService burgerService;
    private ComplementService complementService;
    private ImageService imageService;
    private java.util.Scanner scanner;

    public MenuView(MenuService menuService, BurgerService burgerService, ComplementService complementService) {
        this.menuService = menuService;
        this.burgerService = burgerService;
        this.complementService = complementService;
        this.imageService = new ImageService();
        this.scanner = new java.util.Scanner(System.in);
    }

    public void afficherMenu() throws SQLException {
        while (true) {
            System.out.println("\n=== MENU MENU ===");
            System.out.println("1. Lister tous les menus");
            System.out.println("2. Créer un menu");
            System.out.println("3. Modifier un menu");
            System.out.println("4. Archiver un menu");
            System.out.println("5. Retour");
            System.out.print("Choix : ");
            String input = scanner.nextLine();
            if (input.isEmpty()) continue;
            
            try {
                int choix = Integer.parseInt(input);

                switch (choix) {
                    case 1 -> listerMenus();
                    case 2 -> creerMenu();
                    case 3 -> modifierMenu();
                    case 4 -> archiverMenu();
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

    private void listerMenus() throws SQLException {
        List<Menu> list = menuService.listerTous();
        System.out.println("\n--- Liste des menus ---");
        for (Menu m : list) {
            System.out.println(m);
        }
    }

    private void creerMenu() throws SQLException {
        System.out.print("Nom : ");
        String nom = scanner.nextLine();
        System.out.print("Description : ");
        String desc = scanner.nextLine();
        
        
        
        String urlImage = "";
        System.out.print("Chemin COMPLET du fichier image local (ex: /Users/votre_nom/image.jpg) : ");
        String localFilePath = scanner.nextLine();
        
        try {
            urlImage = imageService.uploadImage(localFilePath, "brasil_burger/menus");
            System.out.println(" Image uploadée avec succès. URL : " + urlImage);
        } catch (IOException e) {
            System.out.println(" Erreur lors de l'upload de l'image : " + e.getMessage());
            System.out.println("Annulation de la création du menu.");
            return;
        }
        
        
        
        System.out.println("\n--- Liste des burgers disponibles ---");
        List<Burger> burgersDisponibles = burgerService.listerParEtat(EtatStockEnum.disponible.name());
        if (burgersDisponibles.isEmpty()) {
            System.out.println(" Aucun burger disponible !");
            return;
        }
        for (Burger b : burgersDisponibles) {
            System.out.println(b.getId() + ". " + b.getNom() + " - " + b.getPrix() + " FCFA");
        }
        
        Burger burger = null;
        try {
            System.out.print("ID du burger : ");
            int idBurger = Integer.parseInt(scanner.nextLine());
            burger = burgerService.trouverParId(idBurger);
            
            if (burger == null || burger.getEtatStock() != EtatStockEnum.disponible) {
                System.out.println(" Burger invalide ou non disponible !");
                return;
            }
        } catch (NumberFormatException e) {
            System.out.println(" ID de burger invalide.");
            return;
        }
        
        
        System.out.println("\n--- Liste des compléments disponibles ---");
        List<Complement> complementsDisponibles = complementService.listerComplementsDisponibles(); 
        if (complementsDisponibles.isEmpty()) {
            System.out.println(" Aucun complément disponible !");
            return;
        }
        for (Complement c : complementsDisponibles) {
            System.out.println(c.getId() + ". " + c.getNom() + " - " + c.getPrix() + " FCFA");
        }
        
        
        
        List<Complement> complementsSelectionnes = new ArrayList<>();
        
        while (true) {
            System.out.println("\n Compléments sélectionnés : " + 
                (complementsSelectionnes.isEmpty() ? "Aucun" : complementsSelectionnes.size()));
            
            if (!complementsSelectionnes.isEmpty()) {
                System.out.println("Liste:");
                for (Complement c : complementsSelectionnes) {
                    System.out.println("  - " + c.getNom() + " (" + c.getPrix() + " FCFA)");
                }
            }
            
            System.out.println("\nOptions:");
            System.out.println("1. Ajouter un complément");
            System.out.println("2. Retirer un complément");
            System.out.println("3. Terminer la sélection");
            System.out.print("Choix : ");
            
            String choixComplement = scanner.nextLine();
            
            if (choixComplement.equals("1")) {
                
                
                try {
                    System.out.print("ID du complément à ajouter : ");
                    int idComplement = Integer.parseInt(scanner.nextLine());
                    Complement complement = complementService.trouverParId(idComplement);
                    
                    if (complement == null || complement.getEtatStock() != EtatStockEnum.disponible) {
                        System.out.println(" Complément invalide ou non disponible !");
                        continue;
                    }
                    
                    
                    
                    boolean dejaPresent = complementsSelectionnes.stream()
                        .anyMatch(c -> c.getId() == idComplement);
                    
                    if (dejaPresent) {
                        System.out.println("⚠️  Ce complément est déjà dans la liste !");
                    } else {
                        complementsSelectionnes.add(complement);
                        System.out.println(" " + complement.getNom() + " ajouté !");
                    }
                    
                } catch (NumberFormatException e) {
                    System.out.println(" ID invalide.");
                }
                
            } else if (choixComplement.equals("2")) {
               
                
                if (complementsSelectionnes.isEmpty()) {
                    System.out.println("⚠️  Aucun complément à retirer !");
                    continue;
                }
                
                try {
                    System.out.print("ID du complément à retirer : ");
                    int idComplement = Integer.parseInt(scanner.nextLine());
                    
                    boolean removed = complementsSelectionnes.removeIf(c -> c.getId() == idComplement);
                    
                    if (removed) {
                        System.out.println(" Complément retiré !");
                    } else {
                        System.out.println(" Complément non trouvé dans la liste !");
                    }
                    
                } catch (NumberFormatException e) {
                    System.out.println(" ID invalide.");
                }
                
            } else if (choixComplement.equals("3")) {
                
                if (complementsSelectionnes.isEmpty()) {
                    System.out.println("⚠️  Vous devez sélectionner au moins un complément !");
                    System.out.print("Voulez-vous continuer sans complément ? (O/N) : ");
                    String continuer = scanner.nextLine();
                    if (continuer.equalsIgnoreCase("O")) {
                        break;
                    }
                } else {
                    break;
                }
            } else {
                System.out.println(" Choix invalide !");
            }
        }

        
        Menu menu = new Menu(nom, urlImage, desc, burger, complementsSelectionnes);
        
        System.out.println("\n Récapitulatif du menu :");
        System.out.println("Nom : " + menu.getNom());
        System.out.println("Burger : " + burger.getNom() + " (" + burger.getPrix() + " FCFA)");
        System.out.println("Compléments :");
        for (Complement c : complementsSelectionnes) {
            System.out.println("  - " + c.getNom() + " (" + c.getPrix() + " FCFA)");
        }
        System.out.println("Prix total : " + menu.getPrix_total() + " FCFA");
        
        menuService.creer(menu);
        System.out.println(" Menu créé avec succès !");
    }

    private void modifierMenu() throws SQLException {
        System.out.print("ID du menu à modifier : ");
        String idInput = scanner.nextLine();
        if (idInput.isEmpty()) return;

        int id = 0;
        try {
            id = Integer.parseInt(idInput);
        } catch (NumberFormatException e) {
            System.out.println(" ID invalide.");
            return;
        }
        
        Menu m = menuService.trouverParId(id);
        if (m == null) {
            System.out.println(" Menu non trouvé !");
            return;
        }

        System.out.print("Nouveau nom (" + m.getNom() + ") : ");
        String nom = scanner.nextLine();
        if (!nom.isEmpty()) m.setNom(nom);

        System.out.print("Nouvelle description (" + m.getDescription() + ") : ");
        String desc = scanner.nextLine();
        if (!desc.isEmpty()) m.setDescription(desc);

         
        System.out.println("\nModifier l'image actuelle ? (O/N)");
        String changerImage = scanner.nextLine();

        if (changerImage.equalsIgnoreCase("O")) {
            System.out.print("Chemin COMPLET du nouveau fichier image local : ");
            String localFilePath = scanner.nextLine();
            
            try {
                String newUrl = imageService.uploadImage(localFilePath, "brasil_burger/menus");
                m.setUrl_image(newUrl);
                System.out.println(" Nouvelle image uploadée.");
            } catch (IOException e) {
                System.out.println(" Erreur lors de l'upload : " + e.getMessage());
            }
        }
        
        // Modification du burger
        System.out.println("\nChanger le burger ? (O/N)");
        String changerBurger = scanner.nextLine();
        if (changerBurger.equalsIgnoreCase("O")) {
            List<Burger> burgersDisponibles = burgerService.listerParEtat(EtatStockEnum.disponible.name());
            if (!burgersDisponibles.isEmpty()) {
                for (Burger b : burgersDisponibles) {
                    System.out.println(b.getId() + ". " + b.getNom() + " (" + b.getPrix() + " FCFA)");
                }
                
                try {
                    System.out.print("ID du nouveau burger : ");
                    int idBurger = Integer.parseInt(scanner.nextLine());
                    Burger burger = burgerService.trouverParId(idBurger);
                    
                    if (burger != null && burger.getEtatStock() == EtatStockEnum.disponible) {
                        m.setBurger(burger);
                        System.out.println(" Burger modifié !");
                    }
                } catch (NumberFormatException e) {
                    System.out.println(" ID invalide.");
                }
            }
        }

        
        System.out.println("\nModifier les compléments ? (O/N)");
        String changerComplements = scanner.nextLine();
        if (changerComplements.equalsIgnoreCase("O")) {
            List<Complement> complementsActuels = new ArrayList<>(m.getComplements());
            
            while (true) {
                System.out.println("\n Compléments actuels:");
                if (complementsActuels.isEmpty()) {
                    System.out.println("  Aucun");
                } else {
                    for (Complement c : complementsActuels) {
                        System.out.println("  - " + c.getId() + ". " + c.getNom() + " (" + c.getPrix() + " FCFA)");
                    }
                }
                
                System.out.println("\nOptions:");
                System.out.println("1. Ajouter un complément");
                System.out.println("2. Retirer un complément");
                System.out.println("3. Terminer");
                System.out.print("Choix : ");
                
                String choix = scanner.nextLine();
                
                if (choix.equals("1")) {
                    List<Complement> complementsDisponibles = complementService.listerComplementsDisponibles();
                    for (Complement c : complementsDisponibles) {
                        System.out.println(c.getId() + ". " + c.getNom() + " - " + c.getPrix() + " FCFA");
                    }
                    
                    try {
                        System.out.print("ID du complément : ");
                        int idC = Integer.parseInt(scanner.nextLine());
                        Complement c = complementService.trouverParId(idC);
                        
                        if (c != null && c.getEtatStock() == EtatStockEnum.disponible) {
                            boolean dejaPresent = complementsActuels.stream()
                                .anyMatch(comp -> comp.getId() == idC);
                                
                            if (!dejaPresent) {
                                complementsActuels.add(c);
                                System.out.println(" Ajouté !");
                            } else {
                                System.out.println("⚠️  Déjà présent !");
                            }
                        }
                    } catch (NumberFormatException e) {
                        System.out.println(" ID invalide.");
                    }
                    
                } else if (choix.equals("2")) {
                    try {
                        System.out.print("ID du complément à retirer : ");
                        int idC = Integer.parseInt(scanner.nextLine());
                        complementsActuels.removeIf(c -> c.getId() == idC);
                        System.out.println(" Retiré !");
                    } catch (NumberFormatException e) {
                        System.out.println(" ID invalide.");
                    }
                    
                } else if (choix.equals("3")) {
                    break;
                }
            }
            
            m.setComplements(complementsActuels);
        }

        
        m.recalculerPrixTotal();

        menuService.modifier(m);
        System.out.println(" Menu modifié !");
    }

    private void archiverMenu() throws SQLException {
        System.out.print("ID du menu à archiver : ");
        String idInput = scanner.nextLine();
        if (idInput.isEmpty()) return;
        
        try {
            int id = Integer.parseInt(idInput);
            menuService.archiver(id);
            System.out.println(" Menu archivé !");
        } catch (NumberFormatException e) {
            System.out.println(" ID invalide.");
        }
    }
}
