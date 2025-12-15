package com.example;

import java.sql.SQLException;
import java.util.Scanner;

import com.example.config.factories.ApplicationFactory;

public class App {

    public static void main(String[] args) throws SQLException {

        ApplicationFactory factory = ApplicationFactory.getInstance();

        try (Scanner scanner = new Scanner(System.in)) {
            while (true) {
                System.out.println("\n=== BRASIL BURGER APP ===");
                System.out.println("1. Gérer les Burgers");
                System.out.println("2. Gérer les Compléments");
                System.out.println("3. Gérer les Menus");
                System.out.println("4. Quitter");
                System.out.print("Choix : ");

                int choix = Integer.parseInt(scanner.nextLine());

                switch (choix) {
                    case 1 -> factory.getBurgerView().afficherMenu();
                    case 2 -> factory.getComplementView().afficherMenu();
                    case 3 -> factory.getMenuView().afficherMenu();
                    case 4 -> {
                        System.out.println(" Fermeture de l'application...");
                        factory.close();
                        System.exit(0);
                    }
                    default -> System.out.println(" Choix invalide !");
                }
            }
        } catch (NumberFormatException e) {
            
            
            e.printStackTrace();
        }
    }
}
