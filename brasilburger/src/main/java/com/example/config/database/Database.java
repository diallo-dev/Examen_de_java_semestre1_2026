package com.example.config.database;

import java.sql.Connection;

public interface Database {
    Connection getConnection();
    void close();
    boolean isConnected();
}