
package com.example.entity;

public class Complement {
    private int id;
    private String nom;
    private double prix;
    private String url_image;
    private EtatStockEnum etatStock;

    
    
    public Complement(String nom, double prix, String url_image) {
        this.nom = nom;
        this.prix = prix;
        this.url_image = url_image;
        this.etatStock = EtatStockEnum.disponible;
    }

   
    
    public Complement(int id, String nom, double prix, String url_image, EtatStockEnum etatStock) {
        this.id = id;
        this.nom = nom;
        this.prix = prix;
        this.url_image = url_image;
        this.etatStock = etatStock;
    }

    
    
    public int getId() { return id; }
    public String getNom() { return nom; }
    public double getPrix() { return prix; }
    public String getUrl_image() { return url_image; }
    public EtatStockEnum getEtatStock() { return etatStock; }

    
    
    public void setId(int id) { this.id = id; }
    public void setNom(String nom) { this.nom = nom; }
    public void setPrix(double prix) { this.prix = prix; }
    public void setUrl_image(String url_image) { this.url_image = url_image; }
    public void setEtatStock(EtatStockEnum etatStock) { this.etatStock = etatStock; }

    @Override
    public String toString() {
        return "Complement{" +
                "id=" + id +
                ", nom='" + nom + '\'' +
                ", prix=" + prix +
                ", url_image='" + url_image + '\'' +
                ", etatStock=" + etatStock +
                '}';
    }
}
