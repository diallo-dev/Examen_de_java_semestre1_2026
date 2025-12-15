package com.service;

import java.sql.SQLException;
import java.util.List;

import com.example.entity.Menu;

public interface IService<T> {
    T creer(T data) throws SQLException;
    List<T> listerTous() throws SQLException;
    T trouverParId(int id) throws SQLException;
    boolean modifier(T data) throws SQLException;
    boolean archiver(int id) throws SQLException;
 List<Menu> listerParEtat(String etat) throws SQLException;
}
