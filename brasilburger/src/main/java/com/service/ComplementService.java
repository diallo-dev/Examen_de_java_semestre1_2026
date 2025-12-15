package com.service;

import com.example.entity.Complement;
import com.example.repository.ComplementRepository;

import java.sql.SQLException;
import java.util.List;

public class ComplementService implements IService<Complement> {

    private ComplementRepository repository;

    public ComplementService(ComplementRepository repository) {
        this.repository = repository;
    }

    @Override
    public Complement creer(Complement complement) throws SQLException {
        return repository.creer(complement);
    }

    @Override
    public List<Complement> listerTous() throws SQLException {
        return repository.listerTous();
    }

    @Override
    public Complement trouverParId(int id) throws SQLException {
        return repository.trouverParId(id);
    }

    @Override
    public boolean modifier(Complement complement) throws SQLException {
        return repository.modifier(complement);
    }

    @Override
    public boolean archiver(int id) throws SQLException {
        return repository.archiver(id);
    }

    @Override
    public List listerParEtat(String etat) throws SQLException {
        return repository.listerParEtat(etat);
    }


    public List<Complement> listerComplementsDisponibles() throws SQLException {
        return listerParEtat("disponible");
    }
}
