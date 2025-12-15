package com.service;

import com.cloudinary.Cloudinary;
import com.cloudinary.utils.ObjectUtils;
import com.example.config.CloudinaryConfig;

import java.io.File;
import java.io.IOException;
import java.util.Map;

public class ImageService {

    private final Cloudinary cloudinary;

    public ImageService() {
        this.cloudinary = CloudinaryConfig.getInstance();
    }

    
    
    public String uploadImage(String localFilePath, String folderName) throws IOException {
        File file = new File(localFilePath);
        
        
        if (!file.exists()) {
            throw new IOException(" Fichier non trouvé : " + localFilePath);
        }
        
        if (file.isDirectory()) {
            throw new IOException(" Le chemin spécifié est un dossier, pas un fichier : " + localFilePath);
        }
        
        if (!file.canRead()) {
            throw new IOException(" Impossible de lire le fichier : " + localFilePath);
        }
        
        System.out.println("📤 Upload en cours de : " + file.getName() + "...");
        
        try {
            
            
            Map<String, Object> options = ObjectUtils.asMap(
                "folder", folderName,
                "resource_type", "auto",  
                
                "use_filename", true,     
                
                "unique_filename", true   
                
            );
            
            Map uploadResult = cloudinary.uploader().upload(file, options);
            
            String secureUrl = (String) uploadResult.get("secure_url");
            System.out.println(" Upload réussi ! URL: " + secureUrl);
            
            return secureUrl;
            
        } catch (IOException e) {
            System.err.println(" Erreur lors de l'upload: " + e.getMessage());
            throw e;
        }
    }
    
    
    
    public String uploadImage(String localFilePath) throws IOException {
        return uploadImage(localFilePath, "brasil_burger");
    }
}
