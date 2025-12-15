package com.example.entity;

public class Burger {
    private int id;
    private String nom;
    private double prix;
    private String url_image;
    private String description;
    private EtatStockEnum etatStock;

    
    
    public Burger(String nom, double prix, String url_image, String description) {
        this.nom = nom;
        this.prix = prix;
        this.url_image = url_image;
        this.description = description;
        this.etatStock = EtatStockEnum.disponible;
    }

    
    
    public Burger(int id, String nom, double prix, String url_image, String description, EtatStockEnum etatStock) {
        this.id = id;
        this.nom = nom;
        this.prix = prix;
        this.url_image = url_image;
        this.description = description;
        this.etatStock = etatStock;
    }
    

    
    
    public int getId() {
        return id;
    }

    public String getNom() {
        return nom;
    }

    public double getPrix() {
        return prix;
    }

    public String getUrl_image() {
        return url_image;
    }

    public String getDescription() {
        return description;
    }

    public EtatStockEnum getEtatStock() {
        return etatStock;
    }

    
    
    public void setId(int id) {
        this.id = id;
    }

    public void setNom(String nom) {
        this.nom = nom;
    }

    public void setPrix(double prix) {
        this.prix = prix;
    }

    public void setUrl_image(String url_image) {
        this.url_image = url_image;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setEtatStock(EtatStockEnum etatStock) {
        this.etatStock = etatStock;
    }

    
    
    @Override
    public String toString() {
        return "Burger{" +
                "id=" + id +
                ", nom='" + nom + '\'' +
                ", prix=" + prix +
                ", url_image='" + url_image + '\'' +
                ", description='" + description + '\'' +
                ", etatStock=" + etatStock +
                '}';
    }
}
