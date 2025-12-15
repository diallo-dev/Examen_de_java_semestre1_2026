package com.service;

import com.example.entity.Burger;
import com.example.repository.BurgerRepository;

import java.sql.SQLException;
import java.util.List;

public class BurgerService implements IService<Burger> {

    private BurgerRepository repository;

    public BurgerService(BurgerRepository repository) {
        this.repository = repository;
    }

    @Override
    public Burger creer(Burger burger) throws SQLException {
        return repository.creer(burger);
    }

    @Override
    public List<Burger> listerTous() throws SQLException {
        return repository.listerTous();
    }

    @Override
    public Burger trouverParId(int id) throws SQLException {
        return repository.trouverParId(id);
    }

    @Override
    public boolean modifier(Burger burger) throws SQLException {
        return repository.modifier(burger);
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        return repository.archiver(id);
    }

    
    
    @Override
    public List listerParEtat(String etat) throws SQLException {
        return repository.listerParEtat(etat);
    }
}
