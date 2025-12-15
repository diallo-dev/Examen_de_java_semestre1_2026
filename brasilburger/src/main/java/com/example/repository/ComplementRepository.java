package com.example.repository;

import com.example.config.database.Database;
import com.example.entity.Complement;
import com.example.entity.EtatStockEnum;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class ComplementRepository implements IRepository<Complement> {

    private Database database;

    public ComplementRepository(Database database) {
        this.database = database;
    }

    @Override
    public Complement creer(Complement complement) throws SQLException {
        String sql = "INSERT INTO complement (nom, prix, url_image, etatstock) " +
                     "VALUES (?, ?, ?, ?::etatstock)";

        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            stmt.setString(1, complement.getNom());
            stmt.setDouble(2, complement.getPrix());
            stmt.setString(3, complement.getUrl_image());
            stmt.setString(4, complement.getEtatStock().name());

            int rowsAffected = stmt.executeUpdate();
            
            if (rowsAffected > 0) {
                ResultSet keys = stmt.getGeneratedKeys();
                if (keys.next()) {
                    complement.setId(keys.getInt(1));
                    System.out.println(" Complément créé avec l'ID : " + complement.getId());
                }
            }
        }

        return complement;
    }

    @Override
    public List<Complement> listerTous() throws SQLException {
        List<Complement> list = new ArrayList<>();
        String sql = "SELECT * FROM complement";
        Connection connection = database.getConnection();

        try (Statement stmt = connection.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {

            while (rs.next()) {
                list.add(new Complement(
                        rs.getInt("id"),
                        rs.getString("nom"),
                        rs.getDouble("prix"),
                        rs.getString("url_image"),
                        EtatStockEnum.valueOf(rs.getString("etatstock"))
                ));
            }
        }

        return list;
    }

    @Override
    public Complement trouverParId(int id) throws SQLException {
        String sql = "SELECT * FROM complement WHERE id=?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);

            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                return new Complement(
                        rs.getInt("id"),
                        rs.getString("nom"),
                        rs.getDouble("prix"),
                        rs.getString("url_image"),
                        EtatStockEnum.valueOf(rs.getString("etatstock"))
                );
            }
        }

        return null;
    }

    @Override
    public boolean modifier(Complement complement) throws SQLException {
        String sql = "UPDATE complement SET nom=?, prix=?, url_image=?, etatstock=?::etatstock WHERE id=?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setString(1, complement.getNom());
            stmt.setDouble(2, complement.getPrix());
            stmt.setString(3, complement.getUrl_image());
            stmt.setString(4, complement.getEtatStock().name());
            stmt.setInt(5, complement.getId());

            int rowsAffected = stmt.executeUpdate();
            if (rowsAffected > 0) {
                System.out.println(" Complément modifié avec succès");
                return true;
            }
            return false;
        }
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        String sql = "UPDATE complement SET etatstock='archiver'::etatstock WHERE id=?";
        Connection connection = database.getConnection();

        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setInt(1, id);
            int rowsAffected = stmt.executeUpdate();
            if (rowsAffected > 0) {
                System.out.println("📦 Complément archivé avec succès");
                return true;
            }
            return false;
        }
    }

    @Override
    public List<Complement> listerParEtat(String etat) throws SQLException {
        List<Complement> list = new ArrayList<>();
        String sql = "SELECT * FROM complement WHERE etatstock = ?::etatstock";
        Connection connection = database.getConnection();
    
        try (PreparedStatement stmt = connection.prepareStatement(sql)) {
            stmt.setString(1, etat);
            ResultSet rs = stmt.executeQuery();
    
            while (rs.next()) {
                list.add(new Complement(
                    rs.getInt("id"),
                    rs.getString("nom"),
                    rs.getDouble("prix"),
                    rs.getString("url_image"),
                    EtatStockEnum.valueOf(rs.getString("etatstock"))
                ));
            }
        }
    
        return list;
    }
}