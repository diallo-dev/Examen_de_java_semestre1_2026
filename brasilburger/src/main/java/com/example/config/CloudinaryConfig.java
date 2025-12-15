package com.example.config;

import com.cloudinary.Cloudinary;
import com.cloudinary.utils.ObjectUtils;

public class CloudinaryConfig {

    
    private static final String CLOUD_NAME = "dytumroa3";  
    
    private static final String API_KEY = "272995437721235";
    private static final String API_SECRET = "pSobvM00CQQO33hXGLUxROfKLKw";
    
    private static Cloudinary cloudinary;

    public static Cloudinary getInstance() {
        if (cloudinary == null) {
            cloudinary = new Cloudinary(ObjectUtils.asMap(
                "cloud_name", CLOUD_NAME,
                "api_key", API_KEY,
                "api_secret", API_SECRET,
                "secure", true  
                
            ));
            
            System.out.println(" Cloudinary initialisé (Cloud Name: " + CLOUD_NAME + ")");
        }
        return cloudinary;
    }
    
   
    
    public static boolean testConnection() {
        try {
            Cloudinary cloud = getInstance();
            cloud.api().ping(null);
            System.out.println(" Connexion Cloudinary réussie !");
            return true;
        } catch (Exception e) {
            System.err.println(" Erreur de connexion Cloudinary: " + e.getMessage());
            return false;
        }
    }
}
