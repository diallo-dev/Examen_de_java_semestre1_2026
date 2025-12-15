package com.example.config.factories;

import com.example.config.ConfigLoader;
import com.example.config.database.Database;
import com.example.config.database.DatabaseImpl;
import com.example.repository.BurgerRepository;
import com.example.repository.ComplementRepository;
import com.example.repository.MenuRepository;
import com.service.BurgerService;
import com.service.ComplementService;
import com.service.MenuService;
import com.view.BurgerView;
import com.view.ComplementView;
import com.view.MenuView;

import java.sql.SQLException;
import java.util.Map;

public class ApplicationFactory {

    private static ApplicationFactory instance;

    private Database database;

    private BurgerRepository burgerRepository;
    private ComplementRepository complementRepository;
    private MenuRepository menuRepository;

    private BurgerService burgerService;
    private ComplementService complementService;
    private MenuService menuService;

    private BurgerView burgerView;
    private ComplementView complementView;
    private MenuView menuView;

    private ApplicationFactory() throws SQLException {

        
        Map<String, String> config = ConfigLoader.loadConfig("database.properties");
        this.database = DatabaseImpl.getInstance(config);

        
        initRepositories();
        initServices();
        initViews();
    }

    public static ApplicationFactory getInstance() throws SQLException {
        if (instance == null) {
            instance = new ApplicationFactory();
        }
        return instance;
    }

    
    

    private void initRepositories() {
        this.burgerRepository = new BurgerRepository(database);
        this.complementRepository = new ComplementRepository(database);
        this.menuRepository = new MenuRepository(database);
    }

    private void initServices() {
        this.burgerService = new BurgerService(burgerRepository);
        this.complementService = new ComplementService(complementRepository);
        this.menuService = new MenuService(menuRepository);
    }

    private void initViews() {
        this.burgerView = new BurgerView(burgerService);
        this.complementView = new ComplementView(complementService);
        
        this.menuView = new MenuView(menuService, burgerService, complementService);
    }
    

    
    

    public BurgerView getBurgerView() {
        return burgerView;
    }

    public ComplementView getComplementView() {
        return complementView;
    }

    public MenuView getMenuView() {
        return menuView;
    }

    public void close() throws SQLException {
        if (database != null) {
            database.close();
        }
    }
}
