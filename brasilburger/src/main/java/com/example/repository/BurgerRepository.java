package com.example.repository;

import com.example.config.database.Database;
import com.example.entity.Burger;
import com.example.entity.EtatStockEnum;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class BurgerRepository implements IRepository<Burger> {

    private Database database;

    public BurgerRepository(Database database) {
        this.database = database;
    }

    @Override
    public Burger creer(Burger burger) throws SQLException {
        
        String sql = "INSERT INTO burger(nom, prix, url_image, description, etatstock) VALUES (?, ?, ?, ?, ?::etatstock)";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            stmt.setString(1, burger.getNom());
            stmt.setDouble(2, burger.getPrix());
            stmt.setString(3, burger.getUrl_image());
            stmt.setString(4, burger.getDescription());
            stmt.setString(5, burger.getEtatStock().name());
            
            int rowsAffected = stmt.executeUpdate();

            if (rowsAffected > 0) {
                ResultSet keys = stmt.getGeneratedKeys();
                if (keys.next()) {
                    burger.setId(keys.getInt(1));
                    System.out.println(" Burger créé avec l'ID : " + burger.getId());
                }
            }
        }

        return burger;
    }

    @Override
    public List<Burger> listerTous() throws SQLException {
        List<Burger> burgers = new ArrayList<>();
        String sql = "SELECT * FROM burger";
        Connection connection = database.getConnection();

        try (Statement stmt = connection.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {

            while (rs.next()) {
                burgers.add(new Burger(
                        rs.getInt("id"),
                        rs.getString("nom"),
                        rs.getDouble("prix"),
                        rs.getString("url_image"),
                        rs.getString("description"),
                        EtatStockEnum.valueOf(rs.getString("etatstock"))
                ));
            }
        }

        return burgers;
    }

    @Override
    public Burger trouverParId(int id) throws SQLException {
        String sql = "SELECT * FROM burger WHERE id = ?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);

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

    @Override
    public boolean modifier(Burger burger) throws SQLException {
        String sql = "UPDATE burger SET nom=?, prix=?, url_image=?, description=?, etatstock=?::etatstock WHERE id=?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setString(1, burger.getNom());
            stmt.setDouble(2, burger.getPrix());
            stmt.setString(3, burger.getUrl_image());
            stmt.setString(4, burger.getDescription());
            stmt.setString(5, burger.getEtatStock().name());
            stmt.setInt(6, burger.getId());

            int rowsAffected = stmt.executeUpdate();
            if (rowsAffected > 0) {
                System.out.println(" Burger modifié avec succès");
                return true;
            }
            return false;
        }
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        String sql = "UPDATE burger SET etatstock='archiver'::etatstock WHERE id=?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);
            int rowsAffected = stmt.executeUpdate();
            if (rowsAffected > 0) {
                System.out.println("📦 Burger archivé avec succès");
                return true;
            }
            return false;
        }
    }

    @Override
    public List<Burger> listerParEtat(String etat) throws SQLException {
        List<Burger> burgers = new ArrayList<>();
        String sql = "SELECT * FROM burger WHERE etatstock = ?::etatstock";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setString(1, etat);
            ResultSet rs = stmt.executeQuery();

            while (rs.next()) {
                burgers.add(new Burger(
                        rs.getInt("id"),
                        rs.getString("nom"),
                        rs.getDouble("prix"),
                        rs.getString("url_image"),
                        rs.getString("description"),
                        EtatStockEnum.valueOf(rs.getString("etatstock"))
                ));
            }
        }

        return burgers;
    }
}