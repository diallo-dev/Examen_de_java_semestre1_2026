package com.service;

import com.example.entity.Menu;
import com.example.repository.MenuRepository;

import java.sql.SQLException;
import java.util.List;

public class MenuService implements IService<Menu> {

    private MenuRepository repository;

    public MenuService(MenuRepository repository) {
        this.repository = repository;
    }

    @Override
    public Menu creer(Menu menu) throws SQLException {
        return repository.creer(menu);
    }

    @Override
    public List<Menu> listerTous() throws SQLException {
        return repository.listerTous();
    }

    @Override
    public Menu trouverParId(int id) throws SQLException {
        return repository.trouverParId(id);
    }

    @Override
    public boolean modifier(Menu menu) throws SQLException {
        return repository.modifier(menu);
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        return repository.archiver(id);
    }

    
    
    @Override
    public List<Menu> listerParEtat(String etat) throws SQLException {
        return repository.listerParEtat(etat);
    }
}
