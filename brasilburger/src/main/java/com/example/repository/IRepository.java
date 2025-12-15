package com.example.repository;

import java.sql.SQLException;
import java.util.List;

public interface IRepository<T> {

    
    T creer(T entity) throws SQLException;

    
    List<T> listerTous() throws SQLException;


    List<T> listerParEtat(String etat) throws SQLException;

    
    T trouverParId(int id) throws SQLException;


    boolean modifier(T entity) throws SQLException;

    
    boolean archiver(int id) throws SQLException;
}
