package com.example.repository;

import com.example.config.database.Database;
import com.example.entity.Menu;
import com.example.entity.Complement;
import com.example.entity.Burger;
import com.example.entity.EtatStockEnum;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class MenuRepository implements IRepository<Menu> {

    private Database database;

    public MenuRepository(Database database) {
        this.database = database;
    }

    @Override
    public Menu creer(Menu menu) throws SQLException {
        Connection connection = database.getConnection();
        
        try {
            connection.setAutoCommit(false); 
            
            
            
            String sqlMenu = "INSERT INTO menu (nom, url_image, description, prix_total, etatstock) " +
                           "VALUES (?, ?, ?, ?, ?::etatstock)";

            int menuId = 0;
            try (PreparedStatement stmt = connection.prepareStatement(sqlMenu, Statement.RETURN_GENERATED_KEYS)) {
                stmt.setString(1, menu.getNom());
                stmt.setString(2, menu.getUrl_image());
                stmt.setString(3, menu.getDescription());
                stmt.setDouble(4, menu.getPrix_total());
                stmt.setString(5, menu.getEtatStock().name());

                int rowsAffected = stmt.executeUpdate();
                
                if (rowsAffected > 0) {
                    ResultSet keys = stmt.getGeneratedKeys();
                    if (keys.next()) {
                        menuId = keys.getInt(1);
                        menu.setId(menuId);
                        System.out.println(" Menu créé avec l'ID : " + menuId);
                    }
                }
            }
            
            
            if (menuId > 0 && menu.getBurger() != null) {
                String sqlBurger = "INSERT INTO menu_burger (id_menu, id_burger, quantite) VALUES (?, ?, ?)";
                
                try (PreparedStatement stmtBurger = connection.prepareStatement(sqlBurger)) {
                    stmtBurger.setInt(1, menuId);
                    stmtBurger.setInt(2, menu.getBurger().getId());
                    stmtBurger.setInt(3, 1); 
                    
                    stmtBurger.executeUpdate();
                    System.out.println(" Burger associé au menu");
                }
            }
            
            
            if (menuId > 0 && menu.getComplements() != null && !menu.getComplements().isEmpty()) {
                String sqlComplement = "INSERT INTO menu_complement (id_menu, id_complement, quantite) VALUES (?, ?, ?)";
                
                try (PreparedStatement stmtComplement = connection.prepareStatement(sqlComplement)) {
                    for (Complement complement : menu.getComplements()) {
                        stmtComplement.setInt(1, menuId);
                        stmtComplement.setInt(2, complement.getId());
                        stmtComplement.setInt(3, 1); 
                        
                        stmtComplement.addBatch();
                    }
                    stmtComplement.executeBatch();
                    System.out.println(" " + menu.getComplements().size() + " complément(s) associé(s) au menu");
                }
            }
            
            connection.commit(); 
            
        } catch (SQLException e) {
            connection.rollback(); 
            
            System.err.println(" Erreur lors de la création du menu: " + e.getMessage());
            throw e;
        } finally {
            connection.setAutoCommit(true);
        }

        return menu;
    }

    @Override
    public List<Menu> listerTous() throws SQLException {
        List<Menu> list = new ArrayList<>();
        String sql = "SELECT * FROM menu ORDER BY id";
        Connection connection = database.getConnection();

        try (Statement stmt = connection.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {

            while (rs.next()) {
                Menu menu = new Menu(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getString("url_image"),
                    rs.getString("description"),
                    rs.getDouble("prix_total"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
                
                
                
                Burger burger = getBurgerByMenuId(menu.getId());
                menu.setBurger(burger);
                
                
                
                List<Complement> complements = getComplementsByMenuId(menu.getId());
                menu.setComplements(complements);
                
                list.add(menu);
            }
        }

        return list;
    }

    @Override
    public Menu trouverParId(int id) throws SQLException {
        String sql = "SELECT * FROM menu WHERE id = ?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);
            ResultSet rs = stmt.executeQuery();
            
            if (rs.next()) {
                Menu menu = new Menu(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getString("url_image"),
                    rs.getString("description"),
                    rs.getDouble("prix_total"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
                
                
                
                Burger burger = getBurgerByMenuId(menu.getId());
                menu.setBurger(burger);
                
                
                
                List<Complement> complements = getComplementsByMenuId(menu.getId());
                menu.setComplements(complements);
                
                return menu;
            }
        }

        return null;
    }

    @Override
    public boolean modifier(Menu menu) throws SQLException {
        Connection connection = database.getConnection();
        
        try {
            connection.setAutoCommit(false);
            
            
            
            String sqlMenu = "UPDATE menu SET nom=?, url_image=?, description=?, prix_total=?, etatstock=?::etatstock WHERE id=?";
            
            try (PreparedStatement stmt = connection.prepareStatement(sqlMenu)) {
                stmt.setString(1, menu.getNom());
                stmt.setString(2, menu.getUrl_image());
                stmt.setString(3, menu.getDescription());
                stmt.setDouble(4, menu.getPrix_total());
                stmt.setString(5, menu.getEtatStock().name());
                stmt.setInt(6, menu.getId());

                int rowsAffected = stmt.executeUpdate();
                
                if (rowsAffected == 0) {
                    connection.rollback();
                    return false;
                }
            }
            
            
            
            String sqlDeleteBurger = "DELETE FROM menu_burger WHERE id_menu = ?";
            try (PreparedStatement stmtDelete = connection.prepareStatement(sqlDeleteBurger)) {
                stmtDelete.setInt(1, menu.getId());
                stmtDelete.executeUpdate();
            }
            
            
            
            if (menu.getBurger() != null) {
                String sqlInsertBurger = "INSERT INTO menu_burger (id_menu, id_burger, quantite) VALUES (?, ?, ?)";
                try (PreparedStatement stmtInsert = connection.prepareStatement(sqlInsertBurger)) {
                    stmtInsert.setInt(1, menu.getId());
                    stmtInsert.setInt(2, menu.getBurger().getId());
                    stmtInsert.setInt(3, 1);
                    stmtInsert.executeUpdate();
                }
            }
            
            
            
            String sqlDeleteComplements = "DELETE FROM menu_complement WHERE id_menu = ?";
            try (PreparedStatement stmtDelete = connection.prepareStatement(sqlDeleteComplements)) {
                stmtDelete.setInt(1, menu.getId());
                stmtDelete.executeUpdate();
            }
            
            
            
            if (menu.getComplements() != null && !menu.getComplements().isEmpty()) {
                String sqlInsertComplement = "INSERT INTO menu_complement (id_menu, id_complement, quantite) VALUES (?, ?, ?)";
                
                try (PreparedStatement stmtInsert = connection.prepareStatement(sqlInsertComplement)) {
                    for (Complement complement : menu.getComplements()) {
                        stmtInsert.setInt(1, menu.getId());
                        stmtInsert.setInt(2, complement.getId());
                        stmtInsert.setInt(3, 1);
                        stmtInsert.addBatch();
                    }
                    stmtInsert.executeBatch();
                }
            }
            
            connection.commit();
            System.out.println(" Menu modifié avec succès");
            return true;
            
        } catch (SQLException e) {
            connection.rollback();
            System.err.println(" Erreur lors de la modification du menu: " + e.getMessage());
            throw e;
        } finally {
            connection.setAutoCommit(true);
        }
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        String sql = "UPDATE menu SET etatstock = 'archiver'::etatstock WHERE id = ?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);
            int rowsAffected = stmt.executeUpdate();
            
            if (rowsAffected > 0) {
                System.out.println("📦 Menu archivé avec succès");
                return true;
            }
            return false;
        }
    }

    @Override
    public List<Menu> listerParEtat(String etat) throws SQLException {
        List<Menu> list = new ArrayList<>();
        String sql = "SELECT * FROM menu WHERE etatstock = ?::etatstock ORDER BY id";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setString(1, etat);
            ResultSet rs = stmt.executeQuery();

            while (rs.next()) {
                Menu menu = new Menu(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getString("url_image"),
                    rs.getString("description"),
                    rs.getDouble("prix_total"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
                
                
                
                Burger burger = getBurgerByMenuId(menu.getId());
                menu.setBurger(burger);
                
                
                
                List<Complement> complements = getComplementsByMenuId(menu.getId());
                menu.setComplements(complements);
                
                list.add(menu);
            }
        }

        return list;
    }

    
    
    private Burger getBurgerByMenuId(int menuId) throws SQLException {
        String sql = "SELECT b.* FROM burger b " +
                    "INNER JOIN menu_burger mb ON b.id = mb.id_burger " +
                    "WHERE mb.id_menu = ?";
        
        Connection connection = database.getConnection();
        
        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, menuId);
            ResultSet rs = stmt.executeQuery();
            
            if (rs.next()) {
                return new Burger(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getDouble("prix"),
                    rs.getString("url_image"),
                    rs.getString("description"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
            }
        }
        
        return null;
    }

    
    
    private List<Complement> getComplementsByMenuId(int menuId) throws SQLException {
        List<Complement> complements = new ArrayList<>();
        
        String sql = "SELECT c.* FROM complement c " +
                    "INNER JOIN menu_complement mc ON c.id = mc.id_complement " +
                    "WHERE mc.id_menu = ? " +
                    "ORDER BY mc.id";
        
        Connection connection = database.getConnection();
        
        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, menuId);
            ResultSet rs = stmt.executeQuery();
            
            while (rs.next()) {
                Complement complement = new Complement(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getDouble("prix"),
                    rs.getString("url_image"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
                complements.add(complement);
            }
        }
        
        return complements;
    }
}
