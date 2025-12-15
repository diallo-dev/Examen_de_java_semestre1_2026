package com.example.entity;

import java.util.ArrayList;
import java.util.List;

public class Menu {
    private int id;
    private String nom;
    private String url_image;
    private String description;
    private double prix_total;
    private EtatStockEnum etatStock;
    private Burger burger;
    private Complement complement; 
    
    private List<Complement> complements; 
    

    
    
    public Menu(String nom, String url_image, String description, double prix_total, Burger burger, Complement complement) {
        this.nom = nom;
        this.url_image = url_image;
        this.description = description;
        this.prix_total = prix_total;
        this.etatStock = EtatStockEnum.disponible;
        this.burger = burger;
        this.complement = complement;
        this.complements = new ArrayList<>();
        if (complement != null) {
            this.complements.add(complement);
        }
    }

    
    
    public Menu(int id, String nom, String url_image, String description, double prix_total, EtatStockEnum etatStock) {
        this.id = id;
        this.nom = nom;
        this.url_image = url_image;
        this.description = description;
        this.prix_total = prix_total;
        this.etatStock = etatStock;
        this.burger = null;
        this.complement = null;
        this.complements = new ArrayList<>();
    }

    
    
    public Menu(String nom, String url_image, String description, Burger burger, List<Complement> complements) {
        this.nom = nom;
        this.url_image = url_image;
        this.description = description;
        this.burger = burger;
        this.complements = complements != null ? new ArrayList<>(complements) : new ArrayList<>();
        this.etatStock = EtatStockEnum.disponible;
        
        
        
        this.prix_total = calculerPrixTotal();
        
        
        
        if (!this.complements.isEmpty()) {
            this.complement = this.complements.get(0);
        }
    }

    
    
    private double calculerPrixTotal() {
        double total = 0;
        
        if (burger != null) {
            total += burger.getPrix();
        }
        
        for (Complement c : complements) {
            if (c != null) {
                total += c.getPrix();
            }
        }
        
        return total;
    }

    
    
    public void recalculerPrixTotal() {
        this.prix_total = calculerPrixTotal();
    }

    
    
    public void ajouterComplement(Complement complement) {
        if (complement != null) {
            this.complements.add(complement);
            recalculerPrixTotal();
        }
    }

    
    public boolean retirerComplement(int complementId) {
        boolean removed = this.complements.removeIf(c -> c.getId() == complementId);
        if (removed) {
            recalculerPrixTotal();
        }
        return removed;
    }

    
    public List<Complement> getComplements() {
        return new ArrayList<>(complements); 
    }

    
    public void setComplements(List<Complement> complements) {
        this.complements = complements != null ? new ArrayList<>(complements) : new ArrayList<>();
        recalculerPrixTotal();
        
        
        if (!this.complements.isEmpty()) {
            this.complement = this.complements.get(0);
        } else {
            this.complement = null;
        }
    }

    
    
    public int getId() { return id; }
    public String getNom() { return nom; }
    public String getUrl_image() { return url_image; }
    public String getDescription() { return description; }
    public double getPrix_total() { return prix_total; }
    public EtatStockEnum getEtatStock() { return etatStock; }
    public Burger getBurger() { return burger; }
    public Complement getComplement() { return complement; } 
    

    
    
    public void setId(int id) { this.id = id; }
    public void setNom(String nom) { this.nom = nom; }
    public void setUrl_image(String url_image) { this.url_image = url_image; }
    public void setDescription(String description) { this.description = description; }
    
    public void setPrix_total(double prix_total) { 
        this.prix_total = prix_total; 
    }
    
    public void setEtatStock(EtatStockEnum etatStock) { 
        this.etatStock = etatStock; 
    }

    public void setBurger(Burger burger) { 
        this.burger = burger;
        recalculerPrixTotal();
    }

    public void setComplement(Complement complement) { 
        this.complement = complement;
        
        
        if (complement != null && !complements.contains(complement)) {
            complements.clear();
            complements.add(complement);
            recalculerPrixTotal();
        }
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        sb.append("Menu{");
        sb.append("id=").append(id);
        sb.append(", nom='").append(nom).append('\'');
        sb.append(", url_image='").append(url_image).append('\'');
        sb.append(", description='").append(description).append('\'');
        sb.append(", prix_total=").append(prix_total);
        sb.append(", etatStock=").append(etatStock);
        
        if (burger != null) {
            sb.append(", burger=").append(burger.getNom());
        }
        
        if (!complements.isEmpty()) {
            sb.append(", complements=[");
            for (int i = 0; i < complements.size(); i++) {
                sb.append(complements.get(i).getNom());
                if (i < complements.size() - 1) {
                    sb.append(", ");
                }
            }
            sb.append("]");
        }
        
        sb.append('}');
        return sb.toString();
    }
}