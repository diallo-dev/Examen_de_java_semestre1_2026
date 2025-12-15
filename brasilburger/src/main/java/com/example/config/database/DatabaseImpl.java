package com.example.config.database;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Map;

public class DatabaseImpl implements Database {

    private static DatabaseImpl instance = null;
    private Connection connection;

    private DatabaseImpl(Map<String, String> config) {
        String driver = config.get("driver");
        String url = config.get("url");
        String user = config.get("user");
        String password = config.get("password");
        connection = openConnection(driver, url, user, password);
    }

    public static DatabaseImpl getInstance(Map<String, String> config) {
        if (instance == null) {
            instance = new DatabaseImpl(config);
        }
        return instance;
    }

    @Override
    public Connection getConnection() {
        return connection;
    }

    @Override
    public void close() {
        if (connection != null) {
            try {
                connection.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
    }

    @Override
    public boolean isConnected() {
        return connection != null;
    }

    private Connection openConnection(String driver, String url, String user, String pwd) {
        try {
            Class.forName(driver);
            return DriverManager.getConnection(url, user, pwd);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }
}
